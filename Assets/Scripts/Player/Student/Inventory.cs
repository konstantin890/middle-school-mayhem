// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021-2022 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021-2022 Nikos Konstantinou (nikoskon2003)
//
// The following script has been written by either konstantin890 or Nikos (nikoskon2003) or both.
// This file is covered by the GNU GPL v3 license. Read LICENSE.md for more information.

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Debug values")]
    //we may need this later... ok?
    public bool canUseItems = true;

    //we may need this later... ok?
    public bool canCraftItems = true;

    public int chemicalCount = 0;

    //0: Small Explosive, 1: Itching Powder, 2: Stink Bomb
    public int[] items = { 0, 0, 0 };

    [Header("Item Prefabs")]
    public Transform smallExplosivePrefab;
    public Transform itchingPowderPrefab;
    public Transform stinkBombPrefab;

    [Header("Display Texts")]
    public TMP_Text chemicalsCountDisplay;
    public TMP_Text smallExplosiveCountDisplay;
    public TMP_Text itchingPowderCountDisplay;
    public TMP_Text stinkBombCountDisplay;
    public TMP_Text actionPopupText;
    public Image popupUnderlay;

    [Header("Crafting")]
    public string[] itemNames = { "Small Explosive", "Itching Powder", "Stink Bomb" };
    //0: Small Explosive, 1: Itching Powder, 2: Stink Bomb
    //Vector2: Chemicals, Nerds
    public Vector2Int[] craftingRecepies = { new Vector2Int(3, 3), new Vector2Int(1, 2), new Vector2Int(1, 1) };

    [HideInInspector]
    public StudentLeader studentLeader;

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
        int craftingStation = studentLeader.GetToucingCraftingStation();

        if (craftingStation < 0)
            return;

        if (chemicalCount >= craftingRecepies[craftingStation].x && nerds >= craftingRecepies[craftingStation].y)
        {
            chemicalCount -= craftingRecepies[craftingStation].x; //remove used chemicals
            items[craftingStation]++; //add item
            //update texts
            chemicalsCountDisplay.text = $"{chemicalCount}";
            smallExplosiveCountDisplay.text = $"{items[0]}";
            itchingPowderCountDisplay.text = $"{items[1]}";
            stinkBombCountDisplay.text = $"{items[2]}";

            UpdateCraftingPopup(craftingStation, nerds);

            studentLeader.playerSounds.CraftAction();
        }
    }

    public void UpdateCraftingPopup(int item, int nerds)
    {
        if (item < 0 || item >= itemNames.Length)
        {
            SetPopupText("");
            return;
        }

        string output = $"{itemNames[item]}";

        if (chemicalCount < craftingRecepies[item].x)
            output += $"\n<color=#ff0000>Chemicals {chemicalCount}/{craftingRecepies[item].x}</color>";
        else
            output += $"\n<color=#00ff00>Chemicals {chemicalCount}/{craftingRecepies[item].x}</color>";

        if (nerds < craftingRecepies[item].y)
            output += $"\n<color=#ff0000>Nerds {nerds}/{craftingRecepies[item].y}</color>";
        else
            output += $"\n<color=#00ff00>Nerds {nerds}/{craftingRecepies[item].y}</color>";

        SetPopupText(output);
    }

    public void SetPopupText(string text)
    {
        actionPopupText.text = $"{text}";
        popupUnderlay.enabled = (text.Trim() != "");
    }

    public void PickupChemical()
    {
        chemicalCount = Mathf.Max(chemicalCount + 1, 1); //We dont want "anti-chemicals" :)
        chemicalsCountDisplay.text = $"{chemicalCount}";
    }

    public void UseSmallExplosive()
    {
        if (!canUseItems)
            return;

        if (items[0] > 0)
        {
            items[0]--;
            Instantiate(smallExplosivePrefab, transform.position, transform.rotation); //I have no clue if this is the correct rotation...
            smallExplosiveCountDisplay.text = $"{items[0]}";
        }
        else
        {
            //lets be nice and give the player the option to reset the item to 0 if they get it to negative somehow!
            items[0] = 0;
        }
    }

    //read above comments
    public void UseItchingPowder()
    {
        if (!canUseItems)
            return;

        if (items[1] > 0)
        {
            items[1]--;
            Instantiate(itchingPowderPrefab, transform.position, transform.rotation);
            itchingPowderCountDisplay.text = $"{items[1]}";
        }
        else
        {
            items[1] = 0;
        }
    }

    public void UseStinkBomb()
    {
        if (!canUseItems)
            return;

        if (items[2] > 0)
        {
            items[2]--;
            Instantiate(stinkBombPrefab, transform.position, transform.rotation);
            stinkBombCountDisplay.text = $"{items[2]}";
        }
        else
        {
            items[2] = 0;
        }
    }
}
