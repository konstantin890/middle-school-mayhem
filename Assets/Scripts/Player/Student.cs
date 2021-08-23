using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour
{
    public float walkingSpeed = 1f;
    public float sprintSpeed = 2f;
    [Header("Debug values")]
    public bool isLeader;
    [Range(0,1)] public float fearLevel;

    private float speed = 1f;
    private InputHandler inputHandler;

    public void InitiateStudent()
    {
        inputHandler = this.GetComponent<InputHandler>();

        //possibly set "isLeader = true;" if "inputHandler != null;"??
        //aka if the chatachter has an input handler attached to it, then he is the player/leader
        //(im adding it for debug)
        isLeader = (inputHandler != null);
    }

    private void Update()
    {
        if (fearLevel == 1)
        {
            // leave group?
        }

        // Handle movement
        if (isLeader && inputHandler != null)
        {
            Vector2 movementAxis = inputHandler.GetMovement();
            speed = inputHandler.IsSprinting() ? sprintSpeed : walkingSpeed;

            //this is for - should not chnage transform in update
            //also does not check for collisions...
            this.transform.position += new Vector3(movementAxis.x * speed * Time.deltaTime, movementAxis.y * Time.deltaTime * speed, 0);
        }
    }
}
