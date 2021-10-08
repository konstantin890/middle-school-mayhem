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
