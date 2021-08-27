using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        
    }

    public void Explode()
    {
        animator.SetTrigger("Explode");
    }
}
