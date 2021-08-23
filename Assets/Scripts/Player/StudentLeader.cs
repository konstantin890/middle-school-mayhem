using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentLeader : MonoBehaviour
{
    public static StudentLeader instance;

    public float walkingSpeed = 1f;
    public float sprintSpeed = 2f;

    private float speed = 1f;
    public InputHandler inputHandler;

    private void Awake()
    {
        instance = this;
    }

    public void InitiateStudent()
    {
        //possibly set "isLeader = true;" if "inputHandler != null;"??
        //aka if the chatachter has an input handler attached to it, then he is the player/leader
        //(im adding it for debug)
        //isLeader = (inputHandler != null);
    }

    private void Update()
    {
        // Handle movement
        if (inputHandler != null)
        {
            Vector2 movementAxis = inputHandler.GetMovement();
            speed = inputHandler.IsSprinting() ? sprintSpeed : walkingSpeed;

            //this is for - should not chnage transform in update
            //also does not check for collisions...
            transform.position += new Vector3(movementAxis.x * speed * Time.deltaTime, movementAxis.y * Time.deltaTime * speed, 0);
        }
    }
}
