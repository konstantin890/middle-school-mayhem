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
