// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021-2022 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021-2022 Nikos Konstantinou (nikoskon2003)
//
// The following script has been written by either konstantin890 or Nikos (nikoskon2003) or both.
// This file is covered by the GNU GPL v3 license. Read LICENSE.md for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomDoor : MonoBehaviour
{
    public string sceneNameEnter;
    public bool isFocused;

    [HideInInspector] public bool isBlockedByTeacher; // true when a teacher is present in the room
    [HideInInspector] public bool isBlocked; // true when the room to which the door goes to is "too far"
    [HideInInspector] public bool visuallyOpen; // true when the room has been explored
    public Sprite normalDoor;
    public Sprite openedDoor;
    public Sprite barredDoor;

    public bool checkForTeachers = true;

    private void Awake()
    {
        if (isBlockedByTeacher) 
            transform.parent.GetComponent<SpriteRenderer>().sprite = barredDoor;
    }

    public GameObject spawnObj;

    private void Update()
    {
        if (isFocused && InputHandler.instance.IsButtonHeld(0) && !isBlockedByTeacher && !isBlocked)
        {
            isFocused = false;
            SceneHandler.instance.GoToScene(sceneNameEnter);
            StudentLeader.instance.inventory.SetPopupText("");
            StudentLeader.instance.callout.SetActive(false);
            StudentLeader.instance.playerSounds.DoorAction();
        }
    }

    private void LateUpdate()
    {
        if (!checkForTeachers)
            return;

        if (!visuallyOpen && TeacherNPC.instance == null && !isBlocked)
        {
            UnBarDoor();
            checkForTeachers = false;
        }
        else
        {
            isBlockedByTeacher = true;
            transform.parent.GetComponent<SpriteRenderer>().sprite = barredDoor;
        }
    }

    public void LockDoor()
    {
        isBlocked = true;
        isBlockedByTeacher = false;
        transform.parent.GetComponent<SpriteRenderer>().sprite = barredDoor;
    }

    public void UnBarDoor() 
    {
        isBlockedByTeacher = false;
        isBlocked = false;
        transform.parent.GetComponent<SpriteRenderer>().sprite = normalDoor;
    }

    public void MarkDoorAsOpen()
    {
        visuallyOpen = true;
        transform.parent.GetComponent<SpriteRenderer>().sprite = openedDoor;
    }
}
