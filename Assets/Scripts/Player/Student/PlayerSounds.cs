// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021-2022 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021-2022 Nikos Konstantinou (nikoskon2003)
//
// The following script has been written by either konstantin890 or Nikos (nikoskon2003) or both.
// This file is covered by the GNU GPL v3 license. Read LICENSE.md for more information.

using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public AudioSource soundSrc;

    public AudioClip craft;
    public AudioClip door;
    public AudioClip followerGained;
    public AudioClip collectChemical;

    public void CraftAction()
    {
        soundSrc.clip = craft;
        soundSrc.Play();
    }

    public void DoorAction()
    {
        soundSrc.clip = door;
        soundSrc.Play();
    }

    public void GainedFollower()
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
