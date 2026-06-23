using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour
{
    [SerializeField] float turnSpeed = 70f;

    public float speed = 3f;

    private Rigidbody rigidBody;
    private Vector2 moveInput;
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0,0,0));

        if (moveInput.x != 0)
        {
            Vector3 rotationEuler = new Vector3(0, 0, turnSpeed * -moveInput.x * Time.fixedDeltaTime);
            deltaRotation = Quaternion.Euler(rotationEuler);

            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
        }

        Vector3 moveForward = transform.up * speed * Time.fixedDeltaTime;
        rigidBody.MovePosition(rigidBody.position + moveForward);
    }

    public void Turn(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input: {moveInput}");
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit");

        switch (other.tag)
        {
            case "Food":
            case "MouseFood":
                Destroy(other.gameObject);
                SnakeManager.Instance.AddSnakeBody();
                SnakeManager.Instance.IncrementScore();
                speed = SnakeManager.Instance.IncreaseSnakeSpeed(speed);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.transform.tag)
        {
            case "Map Edge":
            case "SnakeBody":
                SnakeManager.Instance.TriggerGameLoss();
                break;
        }
    }
}
