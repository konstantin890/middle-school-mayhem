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
    public Animator animator;

    public StudentManager studentManager;

    private Inventory inventory;

    //0: Small Explosive, 1: Itching Powder, 2: Stink Bomb
    public bool[] touchingCraftingStations = { false, false, false };

    private void Awake()
    {
        instance = this;

        inventory = GetComponent<Inventory>();
        if (inventory == null)
            Debug.LogError("StudentLeader needs Inventory to work properly.");
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
            animator.SetFloat("Speed", Mathf.Abs(movementAxis.x) + Mathf.Abs(movementAxis.y));

            speed = inputHandler.IsSprinting() ? sprintSpeed : walkingSpeed;

            if (movementAxis.x >= 0.01f)
                transform.localScale = new Vector3(1f, 1f, 1f);
            else if (movementAxis.x <= -0.01f)
                transform.localScale = new Vector3(-1f, 1f, 1f);

            rigidBody.MovePosition(rigidBody.position + movementAxis * speed * Time.deltaTime);
        }
    }

    public bool[] GetToucingCraftingStations() => touchingCraftingStations;

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

        if (collision.gameObject.CompareTag("Chemical")) 
        {
            inventory.PickupChemical();
            //idk if this is optimal...
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("SmallExplosiveCraftingStation"))
            touchingCraftingStations[0] = true;
        if (collision.gameObject.CompareTag("ItchingPowderCraftingStation"))
            touchingCraftingStations[1] = true;
        if (collision.gameObject.CompareTag("StinkBombCraftingStation"))
            touchingCraftingStations[2] = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SmallExplosiveCraftingStation"))
            touchingCraftingStations[0] = false;
        if (collision.gameObject.CompareTag("ItchingPowderCraftingStation"))
            touchingCraftingStations[1] = false;
        if (collision.gameObject.CompareTag("StinkBombCraftingStation"))
            touchingCraftingStations[2] = false;
    }
}
