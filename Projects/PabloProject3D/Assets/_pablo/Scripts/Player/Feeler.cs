using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pablo
{

  [RequireComponent(typeof(SphereCollider)), DisallowMultipleComponent]
  public class Feeler : MonoBehaviour
  {
    [Header("Configuration")]

    [SerializeField] private float angleDirectRange = 10f; // DefaultKoa: 10
    [SerializeField] private float angleNormalRange = 17f; // DefaultKoa: 17
    [SerializeField] private float angleMaxRange = 50f; // DefaultKoa: 50
    [SerializeField] private string tagToCheck = "Interactable";

    [Header("Current polling")]
    [SerializeField, ReadOnly] private IInteractable target = null;
    [SerializeField, ReadOnly] private List<IInteractable> currentInteractables = new List<IInteractable>();

    [Header("Internal")]
    [SerializeField, ReadOnly] private List<(IInteractable interactable, float angle)> angles = new List<(IInteractable, float)>();
    [SerializeField, ReadOnly] private List<(IInteractable interactable, float distance)> anglesPolledByDistance = new List<(IInteractable, float)>();
    [SerializeField] private bool canDetect = true;
    [SerializeField] private bool checkIfCurrentTargetChanges = false;
    private new SphereCollider collider;

    // Properties
    public bool CanDetect { get { return canDetect; } set { canDetect = value; } }
    public IInteractable Target => target;
    public bool HasTarget => target != null;


    private void Awake()
    {
      collider = GetComponent<SphereCollider>();
      collider.isTrigger = true;
    }


    private void Log(string text)
    {
      Debug.Log($"<color=green>[Feeler] {text}</color>");
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag(tagToCheck))
      {
        GameObject with = other.gameObject;
        if (with.activeInHierarchy)
        {
          IInteractable script = with.GetComponent<IInteractable>();
          if (!currentInteractables.Contains(script))
          {
            currentInteractables.Add(script);
            Log("Add interactable");
          }
        }
      }

    }

    private void OnTriggerExit(Collider other)
    {
      if (other.CompareTag(tagToCheck))
      {
        //Debug.LogFormat( "OnTriggerExit " + other.name );
        IInteractable script = other.gameObject.GetComponent<IInteractable>();
        currentInteractables.Remove(script);
        Log("Remove interactable");
        if (target != null && target == script)
        {
          target.Untarget();
          target = null;
        }
      }
    }

    private void Update()
    {
      if (canDetect)
      {
        if (target != null)
        {
          float dist = 20f;

          //Comprobación por distancia para quitarnos el target
          if (target.GetDistanceWithTargetMagnitude(transform.position) > dist)
          {
            currentInteractables.Remove(target);
            target.Untarget();
            target = null;
          }
        }

        if (checkIfCurrentTargetChanges)
        {
          CheckIfCurrentTargetChanges();
          checkIfCurrentTargetChanges = false;
        }
      }
    }

    private void OnTriggerStay(Collider other)
    {
      //if (Main.Instance.Game.Mode == Constants.PlayMode.Blocked)
      //{
      //  return;
      //}

      if (other.CompareTag(tagToCheck))
      {
        GameObject with = other.gameObject;
        if (with.activeInHierarchy)
        {
          checkIfCurrentTargetChanges = true;
        }
      }
    }

    public void Clear()
    {
      if (target != null)
      {
        target.Untarget();
        target = null;
      }
    }


    private void CheckIfCurrentTargetChanges()
    {
      if (CanDetect)
      {
        CheckForNewTarget(); // Comprobar cual es el interactable mas cercano

        // Asegurarnos que el target escogido esta activo en la jerarquia
        if ((target != null) && !target.IsActive)
        {
          target.Untarget();
        }
      }
      else
      {
        if (target != null)
        {
          target.Untarget();
        }
      }
    }

    private IInteractable GetClosest()
    {
      // Check for null references between interactables
      // This is for the ones that were removed or destroyed during the last iteration
      currentInteractables.RemoveAll(i => i == null);

      // First, calculate the angles
      if (angles == null)
      {
        angles = new List<(IInteractable, float)>();
      }
      else
      {
        angles.Clear();
      }

      for (int i = 0; i < currentInteractables.Count; i++)
      {
        IInteractable thingie = currentInteractables[i];
        if (thingie == null)
        {
          Debug.LogError("WAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
          currentInteractables.Remove(thingie);
          continue;
        }

        if (!thingie.CanInteract || !thingie.HasInteraction)
        {
          continue;
        }

        // Remove the koa position to normalize
        Vector3 koa = transform.forward * 1f;
        //Vector3 target = thingie.transform.position - transform.position;
        Vector3 target = thingie.GetDistanceWithTarget(transform.position);

        // Remove the y to project on the x-z plane
        koa.Set(koa.x, 0f, koa.z); // Le quito la y, proyectar en un plano
        target.Set(target.x, 0f, target.z); // Le quito la y, proyectar en un plano

        float angle = Vector3.Angle(koa, target);

        // Ignore interactables behind Koa
        if (angle > angleMaxRange)
        {
          continue;
        }

        angles.Add((thingie, angle));
        //Debug.LogFormat( " - Angle: {0} ({1})", angle, thingie.name );
      }

      if (angles.Count > 0)
      {
        // Check the interactables under certain angle:
        // And save that angle for sorting later
        if (anglesPolledByDistance == null)
        {
          anglesPolledByDistance = new List<(IInteractable, float)>();
        }
        else
        {
          anglesPolledByDistance.Clear();
        }

        // Poll by normal angle
        {
          int length = angles.Count;
          for (int i = 0; i < length; i++)
          {
            if (angles[i].angle < angleNormalRange)
            {
              //float distance = (angles[i].interactable.transform.position - transform.position).magnitude;
              float distance = angles[i].interactable.GetDistanceWithTargetMagnitude(transform.position);
              anglesPolledByDistance.Add((angles[i].interactable, distance));
            }
          }
        }

        // Return the closest one
        if (anglesPolledByDistance.Count > 0)
        {
          float smallerValue = 99f;
          int smallerIndex = -1;
          int length = anglesPolledByDistance.Count;
          for (int i = 0; i < length; i++)
          {
            if (anglesPolledByDistance[i].distance < smallerValue)
            {
              smallerIndex = i;
            }
          }
          return smallerIndex == -1 ? null : anglesPolledByDistance[smallerIndex].interactable;
        }
        else
        {
          // If none was polled by normalAngle, look for the one with less angle
          float smallerValue = 99f;
          int smallerIndex = -1;
          int length = angles.Count;
          for (int i = 0; i < length; i++)
          {
            if (angles[i].angle < smallerValue)
            {
              smallerIndex = i;
            }
          }
          return smallerIndex == -1 ? null : angles[smallerIndex].interactable;
        }

      }

      return null;
    }

    public bool TryInteract()
    {
      //if (MainActivity.Instance.Game.Mode == Constants.PlayMode.Blocked)
      //  return Constants.Interaction.Nothing;

      if (currentInteractables == null || currentInteractables.Count < 1)
      {
        // Koa no tiene nada cerca
        //Debug.LogFormat( "<color=orange>{0}.Interact FAILED, nothing to interact.</color>", name );
        return false;
      }

      if (target == null)
      {
        // Koa no llega
        //Debug.LogFormat( "<color=orange>{0}.Interact FAILED, nothing inside the range.</color>", name );
        return false;
      }

      //Debug.LogFormat( "<color=green>{0}.Interact with {1}</color>", name, interactable.name );
      bool interaction = target.Interact();
      if (target == null || !target.IsActive)
      {
        CheckForNewTarget();
      }
      return interaction;
    }

    private void CheckForNewTarget()
    {
      var newTarget = GetClosest(); // Cojo el mas apropiado

      if (newTarget == null)
      {
        // Si no estamos apuntando nada, quitamos todo
        if (target != null)
        {
          target.Untarget();
          target = null;
        }
      }
      else
      {
        if (target != null) // Si estamos apuntando a algo
        {
          if (newTarget != target) // ... y es nuevo, quito lo de antes
          {
            target.Untarget();
            target = newTarget;
            target.Target();
          }
          else if (newTarget == target)
          {
            // Si estamos en modo Koa
            // Esto es para que se retargetee despues de estar en otro modo de juego
            // UIs por ejemplo
            //if (MainActivity.Instance.Game.Mode == Constants.PlayMode.Koa
            //  || MainActivity.Instance.Game.Mode == Constants.PlayMode.Swim)
            {
              target.Target();
            }
          }
        }
        else
        {
          target = newTarget;
          target.Target();
        }
      }

    }


    // #if UNITY_EDITOR
    //     private void OnDrawGizmos()
    //     {
    //       if (DebugManager.Instance.drawGizmosInteracter && enabled)
    //       {
    //         // Draws a blue line forward
    //         Gizmos.color = Color.blue;
    //         Vector3 palante1 = transform.position;
    //         Vector3 palante2 = transform.forward * 2 + transform.position;
    //         Gizmos.DrawLine(palante1, palante2);

    //         // Bounding sphere
    //         //Gizmos.color = Color.magenta;
    //         Gizmos.color = new Color32(255, 0, 255, 60);
    //         Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().radius * transform.localScale.x);

    //         //// Activate the sphere MeshRenderer
    //         //if( !GetComponent<Renderer>().enabled )
    //         //{
    //         //	GetComponent<Renderer>().enabled = true;
    //         //}

    //         // Draw the whiskers connecting all the interactables within this interacter
    //         Gizmos.color = Color.red;
    //         if (currentInteractables != null || currentInteractables.Count > 0)
    //         {
    //           foreach (var thingie in currentInteractables)
    //           {
    //             if (thingie != null) // Make sure it wasn't destroyed by Axe Tool this very frame
    //             {
    //               //Vector3 thingiePos = thingie.transform.position;
    //               //Gizmos.DrawLine(palante1, thingiePos);
    //             }
    //           }
    //         }

    //         // Draw the cone of angleNormalInteraction
    //         Gizmos.color = Color.cyan;
    //         Vector3 angleLeft = palante1 + Quaternion.Euler(0, angleNormalRange, 0) * (palante2 - transform.position);
    //         Vector3 angleRight = palante1 + Quaternion.Euler(0, -angleNormalRange, 0) * (palante2 - transform.position);
    //         Gizmos.DrawLine(palante1, angleLeft);
    //         Gizmos.DrawLine(palante1, angleRight);

    //         // Draw the cone of angleNormalInteraction
    //         Gizmos.color = Color.magenta;
    //         Vector3 angleMaxLeft = palante1 + Quaternion.Euler(0, angleMaxRange, 0) * (palante2 - transform.position);
    //         Vector3 angleMaxRight = palante1 + Quaternion.Euler(0, -angleMaxRange, 0) * (palante2 - transform.position);
    //         Gizmos.DrawLine(palante1, angleMaxLeft);
    //         Gizmos.DrawLine(palante1, angleMaxRight);

    //       }
    //       else
    //       {
    //         //if( GetComponent<Renderer>().enabled )
    //         //{
    //         //	GetComponent<Renderer>().enabled = false;
    //         //}
    //       }
    //     }
    // #endif

  }
}
