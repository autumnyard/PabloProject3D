using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace Pablo
{
  [RequireComponent(typeof(Rigidbody)), DisallowMultipleComponent]
  public class CameraFirstPerson : MonoBehaviour
  {
    public float moveSpeed;
    public float rotateSpeed;
    public float burstSpeed;

    private bool m_Charging;
    private Vector2 m_Rotation;
    private Vector2 m_Look;
    private Vector2 m_Move;
    private new Rigidbody rigidbody;

    private void Awake()
    {
      if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
    }

    public void OnMove( InputAction.CallbackContext context )
    {
      m_Move = context.ReadValue<Vector2>();
    }

    public void OnLook( InputAction.CallbackContext context )
    {
      m_Look = context.ReadValue<Vector2>();
    }


    public void OnGUI()
    {
      if (m_Charging)
        GUI.Label(new Rect(100, 100, 200, 100), "Charging...");

      ////Press this button to lock the Cursor
      //if (GUI.Button(new Rect(0, 0, 100, 50), "Lock Cursor"))
      //{
      //  Cursor.lockState = CursorLockMode.Locked;
      //}

      ////Press this button to confine the Cursor within the screen
      //if (GUI.Button(new Rect(125, 0, 100, 50), "Confine Cursor"))
      //{
      //  Cursor.lockState = CursorLockMode.Confined;
      //}
    }

    public void Update()
    {
      // Update orientation first, then move. Otherwise move orientation will lag
      // behind by one frame.
      Look(m_Look);
      Move(m_Move);
    }

    private void Look( Vector2 rotate )
    {
      if (rotate.sqrMagnitude < 0.01)
        return;
      rotate *= .1f;
      Debug.Log(rotate.ToString());

      var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
      m_Rotation.y += rotate.x * scaledRotateSpeed;
      m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);

      transform.localEulerAngles = m_Rotation;

      //var quat = Quaternion.FromToRotation(transform.localEulerAngles, m_Rotation);
      //rigidbody.MoveRotation(quat);
    }

    private void Move( Vector2 direction )
    {
      if (direction.sqrMagnitude < 0.01)
        return;
      var scaledMoveSpeed = moveSpeed * Time.deltaTime;
      // For simplicity's sake, we just keep movement in a single plane here. Rotate
      // direction according to world Y rotation of player.
      var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
      transform.position += move * scaledMoveSpeed;
    }


  }

}