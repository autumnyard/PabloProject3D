
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chibig
{
  public class InteractableSwitch : MonoBehaviour, IInteractable
  {
    public bool IsActive { get; set; } = true;
    public bool CanInteract { get; set; } = true;
    public bool HasInteraction { get; set; } = true;

    [SerializeField] private GameObject objectToSwitch;
    [SerializeField, DisableInPlayMode] private bool initialSwitchState;
    [ShowInInspector, ReadOnly] private bool switchState;


    private void Start()
    {
      if (switchState == true)
        SwitchOn();
      else
        SwitchOff();
    }


    public Vector3 GetDistanceWithTarget(Vector3 target) => transform.position - target;
    public float GetDistanceWithTargetMagnitude(Vector3 target) => GetDistanceWithTarget(target).magnitude;

    public bool Interact()
    {
      SwitchToggle();
      return true;
    }

    public void Target()
    {
    }

    public void Untarget()
    {

    }


    private void SwitchOn()
    {
      objectToSwitch.SetActive(true);
      switchState = true;
    }

    private void SwitchOff()
    {
      objectToSwitch.SetActive(false);
      switchState = false;
    }

    private void SwitchToggle()
    {
      if (switchState == true)
        SwitchOff();
      else
        SwitchOn();
    }
  }
}
