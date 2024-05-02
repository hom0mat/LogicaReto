using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
//using UnityEngine.InputSystem;

public class TractorMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;
    Vector2 movement;
    public Animator animator;
    public VectorValue startingPosition;

    public UIController controller;
    private void Start()
    {
        transform.position = startingPosition.initialValue;
        moveSpeed = 3.5f;
    }
    void Update()
    {

        if (controller.moveRight == 1)
        {
            movement.x = 1f; // Mover hacia la derecha automáticamente
            movement.y = 0f;
        }
        else if (controller.moveLeft == 1)
        {
            movement.x = -1f;
            movement.y = 0;
        }
        else if (controller.moveUp == 1)
        {
            movement.x = 0f;
            movement.y = 1f;
        }
        else if (controller.moveDown == 1)
        {
            movement.x = 0f;
            movement.y = -1f;
        }
        else
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Cambios de velocidad
        if (controller.LowSpeed == 1)
        {
            moveSpeed = 2f;
        }
        else
        {
            moveSpeed = 3.5f;
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

}


