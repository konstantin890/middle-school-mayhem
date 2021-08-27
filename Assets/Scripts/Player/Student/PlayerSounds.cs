using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioSource soundSrc;

    public AudioClip craft;
    public AudioClip door;
    public AudioClip followerGained;
    public AudioClip collectChemical;

    public void CaftSound() 
    {
        soundSrc.clip = craft;
        soundSrc.Play();
    }

    public void DoorSound()
    {
        soundSrc.clip = door;
        soundSrc.Play();
    }

    public void FollowerSound() 
    {
        soundSrc.clip = followerGained;
        soundSrc.Play();
    }

    public void CollectChemical() 
    {
        soundSrc.clip = collectChemical;
        soundSrc.Play();
    }

}
