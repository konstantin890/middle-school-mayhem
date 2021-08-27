using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomDoor : MonoBehaviour
{
    public string sceneNameEnter;
    public bool isFocused;

    public GameObject spawnObj;

    private void Update()
    {
        if (isFocused && InputHandler.instance.IsButtonHeld(0))
        {
            isFocused = false;
            SceneHandler.instance.GoToScene(sceneNameEnter);
            StudentLeader.instance.inventory.SetPopupText("");
            StudentLeader.instance.callout.SetActive(false);
        }
    }
}
