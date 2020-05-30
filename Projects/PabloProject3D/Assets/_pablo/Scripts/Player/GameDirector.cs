
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pablo
{
  public class GameDirector : BaseDirector
  {
    [Header("Map")]
    // Debug
    //[InlineButton("SetMap")]
    //[SerializeField] private int mapToLoad;
    [SerializeField] private Constants.Map mapCurrent = Constants.Map.Debug0;
    [SerializeField, ReadOnly] private bool isLoadingMap = false;


    protected override void Log( string text ) => Debug.Log($"<color=orange>[Game] {text}</color>");

    private void Awake()
    {
      //if (player == null) player = FindObjectOfType<PlayerController>().transform;
      //if (cam == null) cam = FindObjectOfType<OrbitCamera>();
      Begin();
    }

    #region Direction

    [ContextMenu("Lock Cursor")]
    private void LockCursor() => Cursor.lockState = CursorLockMode.Locked;

    public override void Begin()
    {
      Log(" --- Begin");
      Main.Instance.SetScene(Constants.Scene.Game, this);
      //cam.SetFollowTransform(player);
      LoadMap(mapCurrent);

      LockCursor();
    }

    public override void End()
    {
      Log(" --- End");
    }

    #endregion


    #region Map

    //[Button("Set Map")]
    //private void SetMap(Constants.Map mapToLoad)
    //{
    //  if (!isLoadingMap && mapCurrent != mapToLoad)
    //  {
    //    UnloadMap(mapCurrent);
    //    LoadMap(mapToLoad);
    //  }
    //}

    [ContextMenu("Unload Map")]
    private void UnloadMap( Constants.Map which )
    {

      Scene asd = SceneManager.GetSceneByName($"Map_{which}");
      if (asd.IsValid())
      {
        Log($"Unload {which}");
        SceneManager.UnloadSceneAsync(asd);
      }
    }

    [ContextMenu("Load Map")]
    private void LoadMap( Constants.Map which )
    {
      Log($"Gonna load {which}");
      isLoadingMap = true;
      SceneManager.LoadSceneAsync($"Map_{which}", LoadSceneMode.Additive).completed += LoadMap_Async;
      mapCurrent = which;
    }

    private void LoadMap_Async( AsyncOperation obj )
    {
      obj.allowSceneActivation = true;
      isLoadingMap = false;
      Log($"Loaded {mapCurrent}");
    }

    #endregion

  }
}
