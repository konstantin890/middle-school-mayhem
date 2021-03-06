// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021-2022 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021-2022 Nikos Konstantinou (nikoskon2003)
//
// The following script has been written by either konstantin890 or Nikos (nikoskon2003) or both.
// This file is covered by the GNU GPL v3 license. Read LICENSE.md for more information.

using System.Collections;
using UnityEngine;

public class StinkBomb : MonoBehaviour
{
    public Animator animator;
    public float effectRadius = 1.5f;
    private AudioSource soundSrc;

    public float timeToExplode = 0.5f;
    public float tickTime = 1f;
    public float loopTick = 3f;
    public float patianceDamage = 30f;

    private void Awake()
    {
        soundSrc = GetComponent<AudioSource>();

        StartCoroutine(StartExploding());
    }

    IEnumerator StartExploding()
    {
        yield return new WaitForSeconds(timeToExplode);
        animator.SetTrigger("Explode");
        yield return new WaitForSeconds(0.3f);
        soundSrc.Play();
        yield return new WaitForSeconds(0.15f);
        StartCoroutine(GasEffect());
        yield return new WaitForSeconds(0.25f);

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 1f); //we need to wait a bit for the sound to finish!
    }

    public void ActivateEffect()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, effectRadius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Teacher"))
            {
                TeacherNPC npc = hitCollider.GetComponent<TeacherNPC>();
                npc.LosePatiance(patianceDamage);
            }
        }
    }

    private IEnumerator GasEffect()
    {
        for (int i = 0; i < loopTick; i++)
        {
            yield return new WaitForSeconds(tickTime);
            ActivateEffect();
        }
    }
}
