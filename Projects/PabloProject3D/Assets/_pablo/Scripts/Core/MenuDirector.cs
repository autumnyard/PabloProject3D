using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pablo
{
  public class MenuDirector : BaseDirector
  {

    protected override void Log(string text) => Debug.Log($"<color=orange>[Menu] {text}</color>");

    private void Awake() => Begin();

    public override void Begin()
    {
      Log(" --- Begin");
      Main.Instance.SetScene(Constants.Scene.Menu, this);
    }

    public override void End()
    {
      Log(" --- End");
    }

  }
}
