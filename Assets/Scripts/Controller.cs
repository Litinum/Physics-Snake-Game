using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour
{
    [SerializeField] float turnSpeed = 70f;

    public float speed = 3f;

    private Rigidbody rigidBody;
    private Vector2 moveInput;

    private Quaternion prevRotationDelta;
    private Vector3 prevMoveDelta;
    

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

        prevMoveDelta = moveForward;
        prevRotationDelta = deltaRotation;
    }

    public void Turn(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input: {moveInput}");
    }

    public void GetLastDeltaMovement(out Vector3 moveDelta, out Quaternion rotationDelta)
    {
        moveDelta = prevMoveDelta;
        rotationDelta = prevRotationDelta;
    }

    void RestartGameOnLoss()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit");

        switch (other.tag)
        {
            case "Food":
                Destroy(other.gameObject);
                SnakeManager.Instance.AddSnakeBody();
                SnakeManager.Instance.IncrementScore();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.transform.tag)
        {
            case "Map Edge":
            case "SnakeBody":
                RestartGameOnLoss();
                break;
        }
    }
}
