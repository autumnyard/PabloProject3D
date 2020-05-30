#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

using Sirenix.OdinInspector.Editor;
using UnityEditor.Build.Reporting;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace Pablo
{
  public class PabloBuildWindow : OdinEditorWindow
  {

    #region UI

    [MenuItem("Chibig/Windows/Build", priority = 2)]
    public static void ShowWindow() => GetWindow<PabloBuildWindow>("Build");

    [Button(ButtonSizes.Large), GUIColor(1, 0, 0, 1)]
    public void Build() => MakeBuild();

    #endregion


    public enum BuildSystem
    {
      Windows,
      Switch,
      PS4
    }

    public enum BuildType
    {
      Debug,
      Master
    }

    public enum Color
    {
      Normal,
      Success,
      Failure
    }

    [Header("Build")]
    public BuildSystem buildSystem;
    public BuildType buildType;

    [Header("Chibig settings")]
    [InlineButton("SetCheats")] public bool cheats = false;

    [Header("Unity settings")]
    public bool autoRunPlayer = false;
    public bool autoOpenFolder = false;
    public bool scriptsOnly = false;
    public bool writeDate = true;
    public string unityVersion = "1.0";

#if UNITY_SWITCH
    [Header( "Switch" )]
    public int switchVersion = 0;
#endif
    [Header("Other")]
    public string comment = "";


    private void MakeBuild()
    {
      switch (buildSystem)
      {
        case PabloBuildWindow.BuildSystem.Windows:
          switch (buildType)
          {
            case BuildType.Debug:
              MakeBuild_Windows_Debug();
              break;

            case BuildType.Master:
              MakeBuild_Windows_Release();
              break;
          }
          break;

        case PabloBuildWindow.BuildSystem.Switch:
          switch (buildType)
          {
            case BuildType.Debug:
#if UNITY_SWITCH
              MakeBuild_Switch_Debug();
#endif
              break;

            case BuildType.Master:
#if UNITY_SWITCH
              MakeBuild_Switch_Release();
#endif
              break;
          }
          break;

        case PabloBuildWindow.BuildSystem.PS4:
          switch (buildType)
          {
            case BuildType.Debug:
#if UNITY_PS4
              MakeBuild_PS4_Debug();
#endif
              break;

            case BuildType.Master:
#if UNITY_PS4
              MakeBuild_PS4_Release();
#endif
              break;
          }
          break;
      }
    }

    private void Print(string text, Color color = Color.Normal)
    {
      switch (color)
      {
        case Color.Normal:
          Debug.Log($"<color=white>PabloBuilder: {text}</color>");
          break;
        case Color.Success:
          Debug.Log($"<b><color=green>PabloBuilder: {text}</color></b>");
          break;
        case Color.Failure:
          Debug.Log($"<b><color=red>PabloBuilder: {text}</color></b>");
          break;
      }
    }

    [Button, GUIColor(0, 1, 0, 1)]
    private void PrintData()
    {
      Print($" ---- Unity build data ----");
      Print($"{PlayerSettings.applicationIdentifier}: {PlayerSettings.productGUID}, {PlayerSettings.companyName} and {PlayerSettings.productName}");
#if UNITY_SWITCH
      Print($"Defines: {PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Switch)}");
#elif UNITY_STANDALONE
      Print($"Defines: {PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone)}");
#elif UNITY_PS4
      Print($"Defines: {PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.PS4)}");
#elif UNITY_XBOX
#endif
      Print($"Unity version: {PlayerSettings.bundleVersion}");
#if UNITY_SWITCH
      Print( $" ---- Switch specifics ----" );
      Print( $"{PlayerSettings.Switch.applicationID}, Display: {PlayerSettings.Switch.displayVersion}. Switch version: {PlayerSettings.Switch.releaseVersion}" );
#endif
    }


    [Button, HorizontalGroup("Cleaner")]
    private void ClearReferences()
    {
      //WeatherManager.Instance.DataUnload();
      //AssetManager.Instance.DataUnload();
      //DataManager.Instance.DataUnload();
      //WeatherManager.Instance.Skybox.SetTexture("_Tex", null);
      //WeatherManager.Instance.Skybox.SetTexture("_Tex2", null);
    }

    [Button, HorizontalGroup("Cleaner")]
    private void ClearSaves()
    {
      //FileUtil.DeleteFileOrDirectory("Assets/StreamingAssets/Save/Abraham/");
      //FileUtil.DeleteFileOrDirectory("Assets/StreamingAssets/Save/Main");
      //FileUtil.DeleteFileOrDirectory("Assets/StreamingAssets/Save/Pablo");
    }


    private void SetDefineSymbol(string define, bool to)
    {
#if UNITY_SWITCH
      BuildTargetGroup target = BuildTargetGroup.Switch;
#elif UNITY_PS4
      BuildTargetGroup target = BuildTargetGroup.PS4;
#else
      BuildTargetGroup target = BuildTargetGroup.Standalone;
#endif
      if (to)
      {
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
        if (!defines.Contains($"{define}"))
        {
          defines += $";{define}";
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(target, defines);
      }
      else
      {
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
        if (defines.Contains($"{define}"))
        {
          defines = defines.Replace($";{define}", "");
          defines = defines.Replace($"; {define}", "");
          defines = defines.Replace($"{define};", ""); // DEMO se suele poner al principio, el maldito
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(target, defines);
      }
    }

    private void SetCheats(bool forceNoCheats = false) => SetDefineSymbol("FANTASIA", !(!cheats || forceNoCheats));

    private void SetCheats() => SetDefineSymbol("FANTASIA", cheats);


    private string[] GetScenes(bool custom = true)
    {
      if (custom)
      {
        return new[] {
          "Assets/_chibig/Scenes/Game.unity",
          "Assets/_chibig/Scenes/Menu.unity",
          "Assets/_chibig/Scenes/UIDialogue.unity",
          "Assets/_chibig/Scenes/UIStatus.unity",
          "Assets/_chibig/Scenes/Map_Debug0.unity",
          "Assets/_chibig/Scenes/Map_Debug1.unity",
          "Assets/_chibig/Scenes/Map_Debug2.unity",
        };
      }
      else
      {
        //return .to;
        return null;
      }
    }


    private void MakeBuild_Windows_Debug()
    {
      SetCheats();

      PlayerSettings.bundleVersion = unityVersion;

      // Configure options
      BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
      buildPlayerOptions.scenes = GetScenes();
      //buildPlayerOptions.scenes = EditorBuildSettings.scenes;

      string fileName = "../../Build/b";
      fileName += buildType == BuildType.Debug ? "d" : "r";
      fileName += cheats ? "c" : "";
      string date = System.DateTime.Now.ToString("yyyyMMdd-HHmm");
      if (writeDate)
      {
        fileName += $"-{date}";
      }
      fileName += $"-v{unityVersion}";
      if (!comment.Equals(""))
      {
        fileName += $"-{comment}";
      }
      buildPlayerOptions.locationPathName = fileName + "/Witch.exe";
      buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
      if (buildType == BuildType.Debug)
      {
        buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.StrictMode;
      }
      else
      {
        buildPlayerOptions.options = BuildOptions.None;
      }

      if (autoRunPlayer)
      {
        buildPlayerOptions.options |= BuildOptions.AutoRunPlayer;
      }

      // Configure environment
      EditorUserBuildSettings.development = buildType == BuildType.Debug;
      EditorUserBuildSettings.buildScriptsOnly = scriptsOnly;

      DoBuildPlayer(buildPlayerOptions);
    }

    private void MakeBuild_Windows_Release()
    {
      SetCheats(true);

      PlayerSettings.bundleVersion = unityVersion;

      // Configure options
      BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
      buildPlayerOptions.scenes = GetScenes();

      buildType = BuildType.Master;

      string fileName = "../../Build/b";
      fileName += buildType == BuildType.Debug ? "d" : "r";
      fileName += cheats ? "c" : "";
      string date = System.DateTime.Now.ToString("yyyyMMdd-HHmm");
      if (writeDate)
      {
        fileName += $"-{date}";
      }
      fileName += $"-v{unityVersion}";
      if (!comment.Equals(""))
      {
        fileName += $"-{comment}";
      }
      buildPlayerOptions.locationPathName = fileName + "/Witch.exe";
      buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
      if (buildType == BuildType.Debug)
      {
        buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.StrictMode;
      }
      else
      {
        buildPlayerOptions.options = BuildOptions.None;
      }

      if (autoRunPlayer)
      {
        buildPlayerOptions.options |= BuildOptions.AutoRunPlayer;
      }

      // Configure environment
      EditorUserBuildSettings.development = buildType == BuildType.Debug;
      EditorUserBuildSettings.buildScriptsOnly = scriptsOnly;

      DoBuildPlayer(buildPlayerOptions);
    }


#if UNITY_SWITCH
    private void ConfigureSwitch()
    {
      // Configure publishing settings
#if DEMO
      PlayerSettings.Switch.applicationID = "0x0100c7a012688000";
#else
      PlayerSettings.Switch.applicationID = "0x0100a130109b2000";
#endif
      PlayerSettings.bundleVersion = unityVersion;
      PlayerSettings.Switch.releaseVersion = switchVersion.ToString();
      PlayerSettings.Switch.startupUserAccount = PlayerSettings.Switch.StartupUserAccount.Required;
      PlayerSettings.Switch.touchScreenUsage = (buildType == BuildType.Debug) ? PlayerSettings.Switch.TouchScreenUsage.Supported : PlayerSettings.Switch.TouchScreenUsage.None;
      PlayerSettings.Switch.logoType = PlayerSettings.Switch.LogoType.LicensedByNintendo;
      PlayerSettings.Switch.applicationAttribute = PlayerSettings.Switch.ApplicationAttribute.None;

      PlayerSettings.Switch.userAccountSaveDataSize = 4194304; // 2097152;
      PlayerSettings.Switch.userAccountSaveDataJournalSize = 4194304; // 2097152;

      PlayerSettings.Switch.screenResolutionBehavior = PlayerSettings.Switch.ScreenResolutionBehavior.Both;

      PlayerSettings.Switch.useSwitchCPUProfiler = false;
      PlayerSettings.Switch.networkInterfaceManagerInitializeEnabled = true;
      PlayerSettings.Switch.socketInitializeEnabled = true;
      PlayerSettings.Switch.useSwitchCPUProfiler = false;

      PlayerSettings.Switch.supportedNpadCount = 1;
      PlayerSettings.Switch.supportedNpadStyles = PlayerSettings.Switch.SupportedNpadStyle.FullKey | PlayerSettings.Switch.SupportedNpadStyle.Handheld | PlayerSettings.Switch.SupportedNpadStyle.JoyDual;

    }

    private void MakeBuild_Switch_Debug()
    {
      SetCheats();
      ConfigureSwitch();
    
      // Configure options
      BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
      buildPlayerOptions.scenes = GetScenes();

      string fileName = "../../Build/b";
      fileName += buildType == BuildType.Debug ? "d" : "r";
      fileName += cheats ? "c" : "";
      string date = System.DateTime.Now.ToString( "yyyyMMdd-HHmm" );
      if( writeDate )
      {
        fileName += $"-{date}";
      }
      fileName += $"-v{switchVersion}({unityVersion})";
      if( !comment.Equals( "" ) )
      {
        fileName += $"-{comment}";
      }
      buildPlayerOptions.locationPathName = fileName + ".nsp";
      buildPlayerOptions.target = BuildTarget.Switch;
      if( buildType == BuildType.Debug )
      {
        buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.StrictMode;
      }
      else
      {
        buildPlayerOptions.options = BuildOptions.None;
      }

      if( autoRunPlayer )
      {
        buildPlayerOptions.options |= BuildOptions.AutoRunPlayer;
      }

      // Configure environment
      EditorUserBuildSettings.development = buildType == BuildType.Debug;
      EditorUserBuildSettings.switchCreateSolutionFile = false;
      EditorUserBuildSettings.switchCreateRomFile = true;
      EditorUserBuildSettings.buildScriptsOnly = scriptsOnly;

      DoBuildPlayer( buildPlayerOptions );
    }
    
    private void MakeBuild_Switch_Release()
    {
      SetCheats(true);
      ConfigureSwitch();
    
      // Configure options
      BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
      buildPlayerOptions.scenes = GetScenes();

      buildType = BuildType.Master;

      string fileName = "../../Build/b";
      fileName += buildType == BuildType.Debug ? "d" : "r";
      fileName += cheats ? "c" : "";
      string date = System.DateTime.Now.ToString( "yyyyMMdd-HHmm" );
      if( writeDate )
      {
        fileName += $"-{date}";
      }
      fileName += $"-v{switchVersion}({unityVersion})";
      if( !comment.Equals( "" ) )
      {
        fileName += $"-{comment}";
      }
      buildPlayerOptions.locationPathName = fileName + ".nsp";
      buildPlayerOptions.target = BuildTarget.Switch;
      if( buildType == BuildType.Debug )
      {
        buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.StrictMode;
      }
      else
      {
        buildPlayerOptions.options = BuildOptions.None;
      }

      if( autoRunPlayer )
      {
        buildPlayerOptions.options |= BuildOptions.AutoRunPlayer;
      }

      // Configure environment
      EditorUserBuildSettings.development = buildType == BuildType.Debug;
      EditorUserBuildSettings.switchCreateSolutionFile = false;
      EditorUserBuildSettings.switchCreateRomFile = true;
      EditorUserBuildSettings.buildScriptsOnly = scriptsOnly;

      DoBuildPlayer( buildPlayerOptions );
    }

#endif

#if UNITY_PS4
    private void MakeBuild_PS4_Debug()
    {
      SetCheats();

      PlayerSettings.bundleVersion = unityVersion;

      // Configure options
      BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
      //buildPlayerOptions.scenes = EditorBuildSettings.scenes;

      string fileName = "../../Build/PS4-b";
      fileName += buildType == BuildType.Debug ? "d" : "r";
      fileName += cheats ? "c" : "";
      string date = System.DateTime.Now.ToString("yyyyMMdd-HHmm");
      if (writeDate)
      {
        fileName += $"-{date}";
      }
      fileName += $"-v{unityVersion}";
      if (!comment.Equals(""))
      {
        fileName += $"-{comment}";
      }
      buildPlayerOptions.locationPathName = fileName + "/"/*Witch.exe"*/;
      buildPlayerOptions.target = BuildTarget.PS4;
      if (buildType == BuildType.Debug)
      {
        buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.StrictMode;
      }
      else
      {
        buildPlayerOptions.options = BuildOptions.None;
      }

      if (autoRunPlayer)
      {
        buildPlayerOptions.options |= BuildOptions.AutoRunPlayer;
      }

      // Configure environment
      EditorUserBuildSettings.development = buildType == BuildType.Debug;
      EditorUserBuildSettings.buildScriptsOnly = scriptsOnly;

      EditorUserBuildSettings.ps4BuildSubtarget = PS4BuildSubtarget.Package;
      EditorUserBuildSettings.ps4HardwareTarget = PS4HardwareTarget.ProAndBase;

      DoBuildPlayer(buildPlayerOptions);
    }

    private void MakeBuild_PS4_Release()
    {
      SetCheats(true);

      PlayerSettings.bundleVersion = unityVersion;

      // Configure options
      BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
      buildPlayerOptions.scenes = GetScenes();

      buildType = BuildType.Master;

      string fileName = "../../Build/PS4-b";
      fileName += buildType == BuildType.Debug ? "d" : "r";
      fileName += cheats ? "c" : "";
      string date = System.DateTime.Now.ToString("yyyyMMdd-HHmm");
      if (writeDate)
      {
        fileName += $"-{date}";
      }
      fileName += $"-v{unityVersion}";
      if (!comment.Equals(""))
      {
        fileName += $"-{comment}";
      }
      buildPlayerOptions.locationPathName = fileName + "/"/*Witch.exe"*/;
      buildPlayerOptions.target = BuildTarget.PS4;
      if (buildType == BuildType.Debug)
      {
        buildPlayerOptions.options = BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.StrictMode;
      }
      else
      {
        buildPlayerOptions.options = BuildOptions.None;
      }

      if (autoRunPlayer)
      {
        buildPlayerOptions.options |= BuildOptions.AutoRunPlayer;
      }

      // Configure environment
      EditorUserBuildSettings.development = buildType == BuildType.Master;
      EditorUserBuildSettings.buildScriptsOnly = scriptsOnly;

      EditorUserBuildSettings.ps4BuildSubtarget = PS4BuildSubtarget.Package;
      EditorUserBuildSettings.ps4HardwareTarget = PS4HardwareTarget.ProAndBase;

      DoBuildPlayer(buildPlayerOptions);
    }


#endif

    private void DoBuildPlayer(BuildPlayerOptions buildPlayerOptions)
    {
      ClearReferences(); // Clear hard references
      ClearSaves();

      Print($" ==== Gonna build for {buildSystem}-{buildType} ====");
      Print($"Output file:{buildPlayerOptions.locationPathName }");
      PrintData();

      EditorApplication.delayCall += () =>
      {
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        Print($"==== Report ====");

        BuildSummary summary = report.summary;

        switch (summary.result)
        {
          case BuildResult.Unknown:
            Print("Build failed CON EXTRANYAS CONSECUENCIAS", Color.Failure);
            break;

          case BuildResult.Succeeded:
            Print("Build succeeded: " + summary.totalSize + " bytes", Color.Success);
            if (autoOpenFolder)
            {
              string itemPath = buildPlayerOptions.locationPathName.Replace(@"/", @"\");   // explorer doesn't like front slashes
              System.Diagnostics.Process.Start("explorer.exe", "/select," + itemPath);
            }
            break;

          case BuildResult.Failed:
            Print("Build failed", Color.Failure);
            break;

          case BuildResult.Cancelled:
            Print("Build cancelled", Color.Normal);
            break;
        }
      };


      //GUIUtility.ExitGUI();

    }

  }
}
#endif