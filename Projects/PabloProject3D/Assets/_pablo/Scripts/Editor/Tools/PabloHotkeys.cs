#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.Compilation;
using System.Text;

public static class PabloHotkeys
{

  [MenuItem("Pablo/Toggle selected GameObject %g")]
  static void ToggleGameObject()
  {
    //var selected = Selection.transforms;
    if (Selection.transforms != null)
    {
      foreach (Transform t in Selection.transforms)
        t.gameObject.SetActive(!t.gameObject.activeInHierarchy);
    }
  }

  [MenuItem("Pablo/Toggle selected GameObject: All false %#g")]
  static void ToggleGameObjectFalse()
  {
    //var selected = Selection.transforms;
    if (Selection.transforms != null)
    {
      foreach (Transform t in Selection.transforms)
        t.gameObject.SetActive(false);
    }
  }

  [MenuItem("Pablo/Toggle selected GameObject: All true %#&g")]
  static void ToggleGameObjectTrue()
  {
    //var selected = Selection.transforms;
    if (Selection.transforms != null)
    {
      foreach (Transform t in Selection.transforms)
        t.gameObject.SetActive(true);
    }
  }

  [MenuItem("Pablo/Clear console %t")]
  static void ClearConsole()
  {
    // This simply does "LogEntries.Clear()" the long way:
    //var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
    var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
    var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
    clearMethod.Invoke(null, null);
  }

  [MenuItem("Pablo/List Player Assemblies in Console")]
  static void PrintAssemblyNames()
  {
    UnityEngine.Debug.Log("Listing player assemblies:");
    Assembly[] playerAssemblies =
        CompilationPipeline.GetAssemblies(AssembliesType.PlayerWithoutTestAssemblies);

    foreach (var assembly in playerAssemblies.OrderBy((ass) => ass.name))
    {
      //UnityEngine.Debug.Log($" + {assembly.name}");
      //UnityEngine.Debug.Log($"{assembly.outputPath}: {assembly.name}");
      //var refs = assembly.assemblyReferences;
      //foreach (var refe in refs)
      //{
      //  UnityEngine.Debug.Log($"   + {refe.name}");
      //}

      StringBuilder buil = new StringBuilder();
      buil.Append($"  {assembly.name}");
      var refs = assembly.assemblyReferences;
      if (refs.Length > 0)
      {
        buil.Append(" (");
        buil.Append($"{refs[0].name} ");
        for (int i = 1; i < refs.Length; i++)
        {
          buil.Append($"{refs[i].name} ");
        }
        buil.Append(")");
      }

      Debug.Log(buil.ToString());
    }

  }

  //[MenuItem("Pablo/Apply prefab changes %h")]
  //static void ApplyPrefabChanges()
  //{
  //    var obj = Selection.activeGameObject;
  //    if (obj != null)
  //    {
  //        //var prefab_root = PrefabUtility.FindPrefabRoot(obj);
  //        var prefab_root = PrefabUtility.GetOutermostPrefabInstanceRoot(obj);
  //        //var prefab_src = PrefabUtility.GetPrefabParent(prefab_root);
  //        var prefab_src = PrefabUtility.GetCorrespondingObjectFromSource(prefab_root);
  //        if (prefab_src != null)
  //        {
  //            //PrefabUtility.ReplacePrefab(prefab_root, prefab_src, ReplacePrefabOptions.ConnectToPrefab);
  //            //PrefabUtility.SaveAsPrefabAssetAndConnect((prefab_root, prefab_src., ReplacePrefabOptions.ConnectToPrefab);
  //            Debug.Log("Updating prefab : " + AssetDatabase.GetAssetPath(prefab_src));
  //        }
  //        else
  //        {
  //            Debug.Log("Selected has no prefab");
  //        }
  //    }
  //    else
  //    {
  //        Debug.Log("Nothing selected");
  //    }
  //}

  /*
[MenuItem( "GameObject/Thingie with prefab %a" )]
static void PrefabThingie()
{
  //var selected = Selection.transforms;
  if( Selection.transforms != null )
  {
    foreach( Transform t in Selection.transforms )
      //t.gameObject.SetActive( !t.gameObject.activeInHierarchy );
      UnityEditor.PrefabUtility.
  }
}
*/
}
#endif