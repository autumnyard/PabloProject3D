using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace Pablo
{
  [RequireComponent(typeof(Collider)), DisallowMultipleComponent]
  public class MapArea : MonoBehaviour
  {

    [SerializeField, DisableInPlayMode] private Constants.Map map = Constants.Map.Debug0;
    [SerializeField, ReadOnly] private bool isLoadingMap = false;
    [SerializeField, ReadOnly] private bool loadedScene = false;


    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        LoadMap();
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        UnloadMap();
      }
    }



    private void Log(string text) => Debug.Log($"<color=orange>[Game] {text}</color>");
    private void LogError(string text) => Debug.LogError($"<color=orange>[Game]<color=red> {text}</color></color>");


    [ContextMenu("Load Map")]
    private void LoadMap()
    {
      if (loadedScene) return;

      if (!isLoadingMap)
      {
        //Log($"Gonna load {map}");
        isLoadingMap = true;
        SceneManager.LoadSceneAsync($"Map_{map}", LoadSceneMode.Additive).completed += LoadMap_Async;
      }
    }

    private void LoadMap_Async(AsyncOperation obj)
    {
      isLoadingMap = false;
      loadedScene = true;
      Log($"Loaded {map}");
    }

    [ContextMenu("Unload Map")]
    private void UnloadMap()
    {
      if (!loadedScene) return;

      if (!isLoadingMap)
      {
        Scene asd = SceneManager.GetSceneByName($"Map_{map}");
        if (asd.IsValid())
        {
          isLoadingMap = true;
          SceneManager.UnloadSceneAsync(asd).completed += UnloadMap_Async;
        }
        else
        {
          LogError($"Error unloading {map}");
        }
      }
    }

    private void UnloadMap_Async(AsyncOperation obj)
    {
      isLoadingMap = false;
      loadedScene = false;
      Log($"Unload {map}");
    }

  }
}
