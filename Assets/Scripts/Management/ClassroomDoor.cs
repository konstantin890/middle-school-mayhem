// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021 Nikos Konstantinou (nikoskon2003)
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

    [HideInInspector] public bool isBlocked = true;
    public Sprite normalDoor;
    public Sprite barredDoor;

    public bool checkForTeachers = true;

    private void Awake()
    {
        if (isBlocked) 
            transform.parent.GetComponent<SpriteRenderer>().sprite = barredDoor;
    }

    public GameObject spawnObj;

    private void Update()
    {
        if (isFocused && InputHandler.instance.IsButtonHeld(0) && !isBlocked)
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

        if (TeacherNPC.instance == null)
        {
            UnBarDoor();
            checkForTeachers = false;
        }
        else
            isBlocked = true;
    }

    public void UnBarDoor() 
    {
        isBlocked = false;
        transform.parent.GetComponent<SpriteRenderer>().sprite = normalDoor;
    }
}
