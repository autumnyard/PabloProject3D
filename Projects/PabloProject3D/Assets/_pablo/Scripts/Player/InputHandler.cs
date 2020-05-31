using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Pablo
{
  public class InputHandler : MonoBehaviour
  {

    [SerializeField] private PlayerFirstPerson player;

    private Vector2 m_Look;
    private Vector2 m_Move;


    private void Awake()
    {
      if (player == null) player = FindObjectOfType<PlayerFirstPerson>();
    }

    public void OnMove( InputAction.CallbackContext context )
    {
      m_Move = context.ReadValue<Vector2>();
    }

    public void OnLook( InputAction.CallbackContext context )
    {
      m_Look = context.ReadValue<Vector2>();
    }


    private void Update()
    {
      if (Cursor.lockState == CursorLockMode.Locked)
        player.Look(m_Look);

      player.Move(m_Move);

      if (Keyboard.current.spaceKey.wasPressedThisFrame) player.Jump();
      if (Keyboard.current.eKey.wasPressedThisFrame) player.Interact();

      if (Keyboard.current.escapeKey.wasPressedThisFrame) Application.Quit();
    }
  }
}
