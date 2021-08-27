using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomDoor : MonoBehaviour
{
    public string sceneNameEnter;
    public bool isFocused;

    private void Update()
    {
        if (isFocused && InputHandler.instance.IsButtonHeld(0))
        {
            isFocused = false;
            SceneHandler.instance.GoToScene(sceneNameEnter);
        }
    }
}
