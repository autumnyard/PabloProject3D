using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace Pablo
{
  public class PabloMenuWindow : EditorWindow
  {

    [MenuItem("Pablo/Windows/Menu", priority = 1)]
    public static void ShowWindow() => GetWindow<PabloMenuWindow>("Menu");


    private Color Beige() => new Color32(200, 150, 100, 255); // 
    private Color Green() => new Color32(100, 150, 100, 255); // 
    private Color Purple() => new Color32(200, 100, 200, 255); // Quest
    private Color Blue() => new Color32(100, 150, 200, 255); // 


    //[TitleGroup("Windows")]

    //[ButtonGroup("Windows/windows"), Button(ButtonSizes.Large)]
    public void ProjectSettings() => EditorApplication.ExecuteMenuItem("Edit/Project Settings...");


    //[TitleGroup("Game")]

    //[ButtonGroup("Game/game"), Button("Input", ButtonSizes.Large)]
    //public void OpenInputMap() => Selection.activeObject = InputMap.Instance;

    //[ButtonGroup("Game/game"), Button("Debug", ButtonSizes.Large)]
    //public void OpenDebugManager() => Selection.activeObject = DebugManager.Instance;


    //[TitleGroup("World")]

    //[ButtonGroup("World/world"), Button("Quests", ButtonSizes.Large), GUIColor("Purple")]
    //public void Quests() => Selection.activeObject = QuestManager.Instance;

    //[ButtonGroup("World/world"), Button("Assets", ButtonSizes.Large)]
    //public void Assets() => Selection.activeObject = AssetManager.Instance;
  }
}
