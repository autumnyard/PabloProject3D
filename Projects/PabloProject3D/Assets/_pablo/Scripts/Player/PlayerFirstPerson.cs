using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pablo
{
  public class PlayerFirstPerson : MonoBehaviour
  {
    [Header("References")]
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private CharacterController controller;
    [SerializeField] private new Camera camera;
    [SerializeField] private Feeler feeler;

    private Vector3 moveDirection = Vector3.zero;

    [Header("Parameters")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float gravity = 20.0f;
    private Vector2 m_Rotation;
    private Vector2 m_cameraRotation;

    private void Awake()
    {
      if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
      if (controller == null) controller = GetComponent<CharacterController>();
      if (camera == null) camera = GetComponentInChildren<Camera>();
      if (feeler == null) feeler = GetComponentInChildren<Feeler>();
    }

    // private void Update()
    // {

    //   var gravity -= gravity * Time.deltaTime;
    //   controller.Move(moveDirection * Time.deltaTime);
    // }
    

    internal void Look(Vector2 rotate)
    {
      if (rotate.sqrMagnitude < 0.01)
        return;
      rotate *= .1f;

      var scaledRotateSpeed = rotateSpeed * Time.deltaTime;
      m_Rotation.y += rotate.x * scaledRotateSpeed;
      transform.localEulerAngles = m_Rotation;

      m_cameraRotation.x = Mathf.Clamp(m_cameraRotation.x - rotate.y * scaledRotateSpeed, -89, 89);
      camera.transform.localEulerAngles = m_cameraRotation;
      //var quat = Quaternion.FromToRotation(transform.localEulerAngles, m_Rotation);
      //rigidbody.MoveRotation(quat);
    }

    internal void Move_CharacterController(Vector2 direction)
    {
      // if (direction.sqrMagnitude < 0.01)
      //   return;
      var scaledMoveSpeed = moveSpeed * Time.deltaTime;
      // For simplicity's sake, we just keep movement in a single plane here. Rotate
      // direction according to world Y rotation of player.
      //transform.position += move * scaledMoveSpeed;
      // rigidbody.AddForce(move * scaledMoveSpeed, forceMode);

      // controller.Move(move * scaledMoveSpeed * 40f);
      if (controller.isGrounded)
      {
        // We are grounded, so recalculate
        // move direction directly from axes

        // moveDirection = new Vector3(direction.x, 0.0f, direction.y);
        moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
        moveDirection *= moveSpeed;

        if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        {
          moveDirection.y = jumpForce;
        }
      }

      // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
      // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
      // as an acceleration (ms^-2)
      moveDirection.y -= gravity * Time.deltaTime;

      // Move the controller
      // controller.Move(moveDirection * Time.deltaTime);
      controller.Move(moveDirection * Time.deltaTime);
      //rigidbody.AddForce(move * scaledMoveSpeed, ForceMode.VelocityChange);
    }

    internal void Move_Rigidbody(Vector2 direction)
    {
      if (direction.sqrMagnitude < 0.01)
        return;
      var scaledMoveSpeed = moveSpeed * Time.deltaTime;
      // For simplicity's sake, we just keep movement in a single plane here. Rotate
      // direction according to world Y rotation of player.
      var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, 0, direction.y);
      //transform.position += move * scaledMoveSpeed;
      // rigidbody.AddForce(move * scaledMoveSpeed, forceMode);
      rigidbody.MovePosition(transform.position + move * scaledMoveSpeed);

    }

    internal void Move(Vector2 direction)
    {
      if (rigidbody != null) Move_Rigidbody(direction);
      if (controller != null) Move_CharacterController(direction);
    }

    internal void Jump()
    {
      if (rigidbody != null) rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
      // if (controller != null) controller.Move()
    }

    internal void Interact()
    {
      feeler.TryInteract();
    }
  }
}