using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Range(0f, 1f)]
    public float inputSensitivity = 0.1f;
    private int verticalMovement = 0;
    private int horizontalMovement = 0;
    private bool isSprinting = false;

    private bool[] buttonsHeld = { false, false, false, false };

    private Inventory inventory;

    void Awake()
    {
        inventory = GetComponent<Inventory>();
        if (inventory == null)
            Debug.LogError("InputHandler needs Inventory to work properly.");
    }

    private void Update()
    {
        //result = right - left   or   result = up - down
        horizontalMovement = ((Input.GetAxis("Horizontal") >= inputSensitivity) ? 1 : 0) - ((Input.GetAxis("Horizontal") < -inputSensitivity) ? 1 : 0);
        verticalMovement = ((Input.GetAxis("Vertical") >= inputSensitivity) ? 1 : 0) - ((Input.GetAxis("Vertical") < -inputSensitivity) ? 1 : 0);
        isSprinting = Input.GetAxis("Sprint") >= inputSensitivity;

        //Action = Interact with something OR craft
        if (CheckIfButtonPressed("Action", 0))
        {
            //Debug.Log("Action Pressed!");
            inventory.MaybeCraftItems();
            inventory.studentLeader.MaybeShowNextText();
            //add other possible actions here
        }

        //Item1 = Small Explosive
        if (CheckIfButtonPressed("Item1", 1))
        {
            //Debug.Log("Item1 Used!");
            inventory.UseSmallExplosive();
        }

        //Item2 = Itching Powder
        if (CheckIfButtonPressed("Item2", 2))
        {
            inventory.UseItchingPowder();
        }

        //Item3 = Stink Bomb
        if (CheckIfButtonPressed("Item3", 3))
        {
            inventory.UseStinkBomb();
        }


        /*if (CheckIfButtonPressed("Pause", CREATE THE INDEX))
        {
            Debug.Log("Game paused!");
        }*/
    }

    public Vector2 GetMovement() => new Vector2(horizontalMovement, verticalMovement);
    public bool IsSprinting() => isSprinting;

    //Function returns if the requested button is JUST pressed (aka, returns 'true' only once)
    //If we need to check if the button is being held, just check the array 'buttonsHeld', at the wanted position
    private bool CheckIfButtonPressed(string axisName, int arrayIdx)
    {
        if (Input.GetAxis(axisName) >= inputSensitivity && !buttonsHeld[arrayIdx])
        {
            buttonsHeld[arrayIdx] = true;
            return true;
        }
        else if (Input.GetAxis(axisName) < inputSensitivity && buttonsHeld[arrayIdx])
        {
            //Here the button is released... if we need to detect this, we could chnage it so the function returns
            //an integer (0: not pressed, 1: just pressed, 2: just released)
            buttonsHeld[arrayIdx] = false;
        }

        return false;
    }
}
