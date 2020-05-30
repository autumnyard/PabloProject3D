using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pablo
{
  public class Main : Singleton<Main>
  {

    [ShowInInspector, ReadOnly] private Constants.Scene currentScene = Constants.Scene.None;
    [ShowInInspector, ReadOnly] private BaseDirector director;

    public Constants.Scene CurrentScene => currentScene;

    private void Awake()
    {
      SceneHandler.Instance.CheckState();
    }

    public void SetScene(Constants.Scene newScene, BaseDirector newDirector)
    {
      currentScene = newScene;
      director = newDirector;
    }
  }
}