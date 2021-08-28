using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StudentLeader : MonoBehaviour
{
    public static StudentLeader instance;

    public float walkingSpeed = 1f;
    public float sprintSpeed = 2f;

    private float speed = 1f;
    public InputHandler inputHandler;
    public Rigidbody2D rigidBody;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public StudentManager studentManager;

    public string[] craftingStationTags = { "SmallExplosiveCraftingStation", "ItchingPowderCraftingStation", "StinkBombCraftingStation" };

    public Inventory inventory;

    //-1=none, 0=Small Explosive, 1=Itching Powder, 2=Stink Bomb
    private int touchingCraftingStation = 0;

    private TextArea textShown = null;
    private bool canPlayerMove = true;

    public GameObject callout;

    public PlayerSounds playerSounds;

    private void Awake()
    {
        instance = this;

        inventory = GetComponent<Inventory>();
        if (inventory == null)
            Debug.LogError("StudentLeader needs Inventory to work properly.");

        callout = transform.GetChild(0).gameObject; //hopefully this is not null....

        playerSounds = GetComponent<PlayerSounds>(); 
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
        if (inputHandler != null && canPlayerMove)
        {
            Vector2 movementAxis = inputHandler.GetMovement();
            animator.SetFloat("Speed", Mathf.Abs(movementAxis.x) + Mathf.Abs(movementAxis.y));

            speed = inputHandler.IsSprinting() ? sprintSpeed : walkingSpeed;

            if (movementAxis.x >= 0.01f)
                transform.localScale = new Vector3(1f, 1f, 1f);
            else if (movementAxis.x <= -0.01f)
                transform.localScale = new Vector3(-1f, 1f, 1f);

            rigidBody.MovePosition(rigidBody.position + movementAxis * speed * Time.fixedDeltaTime);
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-(transform.position.y - 0.25f));
        }
    }

    public int GetToucingCraftingStation() => touchingCraftingStation;

    public void MaybeShowNextText() 
    {
        if (textShown == null)
            return;

        string text = textShown.GetNextText();

        if (text == "") 
        {
            UnpausePlayer();
            inventory.SetPopupText("");
            textShown = null;
        }

        inventory.SetPopupText(text);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnterArea"))
        {
            // Show text
            ClassroomDoor classroomDoor = collision.gameObject.GetComponent<ClassroomDoor>();
            if (classroomDoor.isBarred)
                inventory.SetPopupText("Door is barred. Cannot enter room");
            else
                inventory.SetPopupText("Enter room?");

            classroomDoor.isFocused = true;
            callout.SetActive(true);
        }

        StudentNPC collStudent = collision.gameObject.GetComponent<StudentNPC>();
        if (collStudent != null) 
        {
            if (!collStudent.IsAttracted())
            {
                studentManager.AddStudent(collStudent);
                playerSounds.FollowerSound();
                //Debug.Log("Added Student to group");
            }

            return;
        }

        TextArea collText = collision.gameObject.GetComponent<TextArea>();
        if (collText != null)
        {
            string text = collText.GetFirstText();
            
            if (text == "")
                return;

            //pause movement before showing text!
            bool pause = collText.ShouldGamePause();
            if (pause)
                StartCoroutine(PausePlayer());

            inventory.SetPopupText(text);
            textShown = collText;

            return;
        }

        if (collision.gameObject.CompareTag("Chemical")) 
        {
            collision.gameObject.tag = "Untagged"; //"Destroy" takes a bit and player could pick up twice a chemical if collided with many at once
            Destroy(collision.gameObject);
            inventory.PickupChemical();
            playerSounds.CollectChemical();
        }

        for (int i = 0; i < craftingStationTags.Length; i++)
        {
            if (collision.gameObject.CompareTag(craftingStationTags[i]))
            {
                touchingCraftingStation = i;
                inventory.UpdateCraftingPopup(i, studentManager.GetStudentCountByClass(StudentClass.Nerd));
                callout.SetActive(true);
                break;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnterArea"))
        {
            // Hide text
            inventory.SetPopupText("");

            collision.gameObject.GetComponent<ClassroomDoor>().isFocused = false;
            callout.SetActive(false);
        }

        TextArea collText = collision.gameObject.GetComponent<TextArea>();
        if (collText != null && collText == textShown) 
        {
            textShown = null;
            inventory.SetPopupText("");
            UnpausePlayer();
        }

        for (int i = 0; i < craftingStationTags.Length; i++)
        {
            if (collision.gameObject.CompareTag(craftingStationTags[i]) && touchingCraftingStation == i)
            {
                callout.SetActive(false);
                touchingCraftingStation = -1;
                inventory.UpdateCraftingPopup(-1, -1);
                break;
            }
        }
    }

    public IEnumerator PausePlayer(bool force = false)
    {
        yield return new WaitForSeconds(0.1f);

        if (textShown != null || force) //player may exit text area...
        {
            canPlayerMove = false;
            animator.SetFloat("Speed", 0f);
            inventory.canUseItems = false;
            inventory.canCraftItems = false;
        }
    }

    public void UnpausePlayer()
    {
        canPlayerMove = true;
        inventory.canUseItems = true; //we needed it later!
        inventory.canCraftItems = true;
    }
}
