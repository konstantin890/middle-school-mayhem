using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StinkBomb : MonoBehaviour
{
    public Animator animator;
    private CircleCollider2D circleCollider = null;
    private AudioSource soundSrc;

    public float timeToDetonate = 0f;
    public float damageDealt;

    private bool easterEggYeyy = true;

    private void Awake()
    {
        //soundSrc = GetComponent<AudioSource>();

        easterEggYeyy = true;

        circleCollider = GetComponent<CircleCollider2D>();
        if(circleCollider == null) 
            Debug.LogError("Broooo!!! Add a circle collider to the stink bomb!!!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Teacher") && easterEggYeyy)
        {
            easterEggYeyy = false; //we detonate the bomb only once
            StartCoroutine(StartExploding());
        }
    }

    IEnumerator StartExploding()
    {
        yield return new WaitForSeconds(timeToDetonate);
        animator.SetTrigger("Explode");
        yield return new WaitForSeconds(0.3f);
        //soundSrc.Play();
        yield return new WaitForSeconds(0.15f);
        ActivateEffect();
        yield return new WaitForSeconds(0.25f);

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 1f); //we need to wait a bit for the sound to finish!
    }

    public void ActivateEffect()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Teacher"))
            {
                //idk do damage or something
            }
        }
    }
}
