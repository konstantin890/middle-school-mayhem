using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public Canvas endCanvas;

    private void LateUpdate()
    {
        if (TeacherNPC.instance == null)
            TriggerEndScreen();
    }

    private void TriggerEndScreen()
    {
        endCanvas.enabled = true;
    }
}
