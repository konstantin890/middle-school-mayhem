using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomDoor : MonoBehaviour
{
    public string sceneNameEnter;
    public bool isFocused;

    public bool isBarred;
    public Sprite normalDoor;
    public Sprite barredDoor;

    private void Awake()
    {
        if (isBarred) 
            transform.parent.GetComponent<SpriteRenderer>().sprite = barredDoor;
    }

    public GameObject spawnObj;

    private void Update()
    {
        if (isFocused && InputHandler.instance.IsButtonHeld(0) && !isBarred)
        {
            isFocused = false;
            SceneHandler.instance.GoToScene(sceneNameEnter);
            StudentLeader.instance.inventory.SetPopupText("");
            StudentLeader.instance.callout.SetActive(false);
            StudentLeader.instance.playerSounds.DoorSound();
        }
    }

    public void UnBarDoor() 
    {
        isBarred = false;
        transform.parent.GetComponent<SpriteRenderer>().sprite = normalDoor;
    }
}
