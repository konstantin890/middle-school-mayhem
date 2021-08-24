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
    public Rigidbody2D rigidBody;

    public StudentManager studentManager;

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

    private void FixedUpdate()
    {
        // Handle movement
        if (inputHandler != null)
        {
            Vector2 movementAxis = inputHandler.GetMovement();
            speed = inputHandler.IsSprinting() ? sprintSpeed : walkingSpeed;

            if (movementAxis.x >= 0.01f)
                transform.localScale = new Vector3(1f, 1f, 1f);
            else if (movementAxis.x <= -0.01f)
                transform.localScale = new Vector3(-1f, 1f, 1f);

            rigidBody.MovePosition(rigidBody.position + movementAxis * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StudentNPC collStudent = collision.gameObject.GetComponent<StudentNPC>();
        if (collStudent != null) 
        {
            if (!collStudent.IsAttracted())
            {
                studentManager.AddStudent(collStudent);
                //Debug.Log("Added Student to group");
            }
        }
    }
}
