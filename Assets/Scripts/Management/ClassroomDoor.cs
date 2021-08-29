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
            StudentLeader.instance.playerSounds.DoorSound();
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
