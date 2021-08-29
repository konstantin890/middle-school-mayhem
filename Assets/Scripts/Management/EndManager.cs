using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public Canvas endCanvas;

    private void LateUpdate()
    {
        if (FindObjectsOfType<TeacherNPC>().Length == 0)
                TriggerEndScreen();
    }

    private void TriggerEndScreen()
    {
        endCanvas.enabled = true;
    }
}
