using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    //we may need this later... ok?
    public bool canUseItems = true;

    //we may need this later... ok?
    public bool canCraftItems = true;

    //0: "Chemicals", 1: Small Explosive, 2: Itching Powder, 3: Stink Bomb
    public int[] items = { 0, 0, 0, 0 };

    public Transform smallExplosivePrefab;
    public Transform itchingPowderPrefab;
    public Transform stinkBombPrefab;

    public TMP_Text chemicalsCountDisplay;
    public TMP_Text smallExplosiveCountDisplay;
    public TMP_Text itchingPowderCountDisplay;
    public TMP_Text stinkBombCountDisplay;

    private StudentLeader studentLeader;

    private void Awake()
    {
        studentLeader = GetComponent<StudentLeader>();
        if (studentLeader == null)
            Debug.LogError("Inventory needs StudentLeader to work properly.");
    }

    public void MaybeCraftItems() 
    {
        if (!canCraftItems)
            return;

        //should not change in an instant...
        int nerds = studentLeader.studentManager.GetStudentCountByClass(StudentClass.Nerd);

        if (studentLeader.GetToucingCraftingStations()[0] && items[0] >= 3 && nerds >= 3) 
        {
            items[0] -= 3; //remove needed chemicals
            items[1]++; //add item
            //update texts
            smallExplosiveCountDisplay.text = $"{items[1]}";
            chemicalsCountDisplay.text = $"{items[0]}";
        }
        if (studentLeader.GetToucingCraftingStations()[1] && items[0] >= 1 && nerds >= 2)
        {
            items[0] -= 1;
            items[2]++;
            itchingPowderCountDisplay.text = $"{items[2]}";
            chemicalsCountDisplay.text = $"{items[0]}";
        }
        if (studentLeader.GetToucingCraftingStations()[2] && items[0] >= 1 && nerds >= 1)
        {
            items[0] -= 1;
            items[3]++;
            stinkBombCountDisplay.text = $"{items[3]}";
            chemicalsCountDisplay.text = $"{items[0]}";
        }
    }

    public void PickupChemical()
    {
        items[0] = Mathf.Max(items[0] + 1, 1); //We dont want "anti-chemicals" :)
        chemicalsCountDisplay.text = $"{items[0]}";
    }

    public void UseSmallExplosive()
    {
        if (!canUseItems)
            return;

        if (items[1] > 0)
        {
            items[1]--;
            Instantiate(smallExplosivePrefab, transform.position, transform.rotation); //I have no clue if this is the correct rotation...
            smallExplosiveCountDisplay.text = $"{items[1]}";
        }
        else
        {
            //lets be nice and give the player the option to reset the item to 0 if they get it to negative somehow!
            items[1] = 0;
        }
    }

    //read above comments
    public void UseItchingPowder()
    {
        if (!canUseItems)
            return;

        if (items[2] > 0)
        {
            items[2]--;
            Instantiate(itchingPowderPrefab, transform.position, transform.rotation);
            itchingPowderCountDisplay.text = $"{items[2]}";
        }
        else
        {
            items[2] = 0;
        }
    }

    public void UseStinkBomb()
    {
        if (!canUseItems)
            return;

        if (items[3] > 0)
        {
            items[3]--;
            Instantiate(stinkBombPrefab, transform.position, transform.rotation);
            stinkBombCountDisplay.text = $"{items[3]}";
        }
        else
        {
            items[3] = 0;
        }
    }
}
