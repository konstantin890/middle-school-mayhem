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

public class TitleManager : MonoBehaviour
{
    public static TitleManager instance;
    public Animator animator;

    private void Awake()
    {
        instance = this;
    }

    public void RemoveTitle()
    {
        /*if (SceneHandler.instance.CurrentLevelHasBeenLoadedBefore())
        {
            transform.GetChild(0).gameObject.SetActive(false);
            return;
        }*/
            

        Debug.Log("Remove title");
        animator.SetTrigger("MoveAway");
    }
}
