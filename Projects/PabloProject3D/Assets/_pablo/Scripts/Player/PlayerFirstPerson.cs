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
    [SerializeField] private new Camera camera;
    [SerializeField] private Feeler feeler;

    [Header("Parameters")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private float jumpForce = 8f;
    private Vector2 m_Rotation;
    private Vector2 m_cameraRotation;

    private void Awake()
    {
      if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
      if (camera == null) camera = GetComponentInChildren<Camera>();
      if (feeler == null) feeler = GetComponentInChildren<Feeler>();
    }


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

    internal void Move(Vector2 direction)
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
      //rigidbody.AddForce(move * scaledMoveSpeed, ForceMode.VelocityChange);
    }

    internal void Jump()
    {
      rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    internal void Interact()
    {
      feeler.TryInteract();
    }
  }
}