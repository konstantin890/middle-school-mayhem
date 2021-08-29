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
        if (SceneHandler.instance.CurrentLevelHasBeenLoadedBefore())
            return;

        Debug.Log("Remove title");
        animator.SetTrigger("MoveAway");
    }
}
