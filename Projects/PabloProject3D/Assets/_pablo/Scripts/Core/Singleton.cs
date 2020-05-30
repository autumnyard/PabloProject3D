using UnityEngine;


namespace Pablo
{
  // Pablo 201901: I get the basic implementation from: http://wiki.unity3d.com/index.php/Singleton.
  public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
  {

    protected static T instance;
    private static bool isShuttingDown;

    public static T Instance
    {
      get
      {
        if (isShuttingDown)
        {
          //Debug.LogWarningFormat( "[Singleton] Instance ' {0}' already destroyed. Returning null.", typeof( T ) );
          return null;
        }

        if (instance == null)
        {
          // Search for existing instance.
          instance = (T)FindObjectOfType(typeof(T));

          // Create new instance if one doesn't already exist.
          if (instance == null)
          {
            // Need to create a new GameObject to attach the singleton to.
            var singletonObject = new GameObject();
            instance = singletonObject.AddComponent<T>();
            singletonObject.name = typeof(T).ToString() + " (Singleton)";

            // Make instance persistent.
            DontDestroyOnLoad(singletonObject);
          }
        }

        return instance;
      }
    }

    private void OnApplicationQuit()
    {
      isShuttingDown = true;
    }

    private void OnDestroy()
    {
      isShuttingDown = true;
    }
  }
}