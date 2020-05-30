using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Pablo
{

  public abstract class BaseDirector : MonoBehaviour
  {
    //[Header("Director")]

    // [SerializeField] private Constants.Scene scene = Constants.Scene.Menu;

    // private void Awake() => MainActivity.Instance.InitializeCurrentScene();

    // protected void Log(string text) => Debug.Log($"<color=orange>[Director] {text}</color>");
    protected abstract void Log(string text);


    public abstract void Begin();

    public abstract void End();

  }
}