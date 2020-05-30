using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chibig
{
  public class SignPost : MonoBehaviour, IInteractable
  {
    public bool IsActive { get; set; } = true;
    public bool CanInteract { get; set; } = true;
    public bool HasInteraction { get; set; } = true;

    [SerializeField] private TalkBubble talkBubble;


    public Vector3 GetDistanceWithTarget(Vector3 target) => transform.position - target;
    public float GetDistanceWithTargetMagnitude(Vector3 target) => GetDistanceWithTarget(target).magnitude;


    public bool Interact()
    {
      return true;
    }

    public void Target()
    {
      talkBubble.Show();
    }

    public void Untarget()
    {
      talkBubble.Hide();
    }
  }
}
