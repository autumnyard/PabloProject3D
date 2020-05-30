using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chibig
{
  public interface IInteractable
  {
    bool IsActive { get; set; }
    bool CanInteract { get; set; }
    bool HasInteraction { get; set; }

    float GetDistanceWithTargetMagnitude( Vector3 target);
    Vector3 GetDistanceWithTarget( Vector3 target);

    bool Interact();
    void Target();
    void Untarget();
  }
}
