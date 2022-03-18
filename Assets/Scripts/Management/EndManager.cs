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
