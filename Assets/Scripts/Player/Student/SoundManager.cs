// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021-2022 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021-2022 Nikos Konstantinou (nikoskon2003)
//
// The following script has been written by either konstantin890 or Nikos (nikoskon2003) or both.
// This file is covered by the GNU GPL v3 license. Read LICENSE.md for more information.

using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Student Audio Sources")]
    public AudioSource hurtSoundSource;
    public AudioSource chaosSoundSource;

    [Header("Teacher Audio Sources")]
    public AudioSource scoldSoundSource;

    private void Awake()
    {
        instance = this;
    }

    #region Student Audio

    public void PlayHurtSound()
    {
        if (!hurtSoundSource.isPlaying)
            hurtSoundSource.Play();
    }

    public void PlayShoutingSound()
    {
        if (!chaosSoundSource.isPlaying)
        {
            chaosSoundSource.loop = false;
            chaosSoundSource.Play();
        }
    }

    public void StopShoutingSound()
    {
        StartCoroutine(StopShoutingAction());
    }

    private IEnumerator StopShoutingAction()
    {
        yield return new WaitWhile(() => chaosSoundSource.isPlaying);
        chaosSoundSource.loop = false;
    }

    #endregion

    #region Teacher Audio

    public void PlayScoldSound(AudioClip clip)
    {
        if (!scoldSoundSource.isPlaying)
            scoldSoundSource.PlayOneShot(clip);
    }

    #endregion
}
