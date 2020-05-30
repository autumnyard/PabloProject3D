using UnityEngine;

namespace Pablo
{
  public class InteractionTarget : MonoBehaviour
  {

    private void OnBecameInvisible()
    {
      Debug.Log("OnBecameInvisible" + name);
    }

    private void OnBecameVisible()
    {
      Debug.Log("OnBecameVisible" + name);
    }

  }
}
