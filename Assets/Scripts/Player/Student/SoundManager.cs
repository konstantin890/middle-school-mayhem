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
