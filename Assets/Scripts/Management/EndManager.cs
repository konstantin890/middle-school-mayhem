using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public Canvas endCanvas;
    public bool stop;

    private void LateUpdate()
    {
        if (!stop && FindObjectsOfType<TeacherNPC>().Length == 0)
                TriggerEndScreen();
    }

    private void TriggerEndScreen()
    {
        stop = true;
        endCanvas.enabled = true;
    }
}
