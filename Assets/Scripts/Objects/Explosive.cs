using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public Animator animator;

    private AudioSource soundSrc;

    private void Awake()
    {
        soundSrc = GetComponent<AudioSource>();
    }

    public void Explode()
    {
        animator.SetTrigger("Explode");
        soundSrc.Play();
    }
}
