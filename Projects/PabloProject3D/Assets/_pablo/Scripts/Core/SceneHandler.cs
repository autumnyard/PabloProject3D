using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Pablo
{
  public class SceneHandler : Singleton<SceneHandler>
  {
    private void Log(string text) => Debug.Log($"<color=yellow>[Scene Handler] {text}</color>");


    public void CheckState()
    {

      if (SceneManager.sceneCount == 1)
      {
        Log("Started with 1 scene");
      }
      else
      {
        Log("Started with more than 1 scenes");
      }
    }

#if UNITY_EDITOR
    /*[Button]*/ private void OpenUIDialogue() => SceneManager.LoadSceneAsync("UIDialogue", LoadSceneMode.Additive);
    /*[Button]*/ private void CloseUIDialogue() => SceneManager.UnloadSceneAsync("UIDialogue");
#endif
  }
}