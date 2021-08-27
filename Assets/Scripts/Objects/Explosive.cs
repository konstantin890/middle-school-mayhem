using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public Animator animator;
    public float explosionRadius = 1f;
    private AudioSource soundSrc;

    private void Awake()
    {
        soundSrc = GetComponent<AudioSource>();

        StartCoroutine(StartExploding());
    }

    IEnumerator StartExploding() 
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("Explode");
        yield return new WaitForSeconds(0.3f); //idk change this
        soundSrc.Play();
        yield return new WaitForSeconds(0.15f);
        ExplodeBarredDoors();
        yield return new WaitForSeconds(0.25f);

        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 1f); //we need to wait a bit for the sound to finish!
    }


    public void ExplodeBarredDoors()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("EnterArea")) 
            {
                ClassroomDoor classroomDoor = hitCollider.gameObject.GetComponent<ClassroomDoor>();
                classroomDoor.UnBarDoor();
                if (classroomDoor.isFocused) 
                    StudentLeader.instance.inventory.SetPopupText("Enter room?");
                
            }
        }
    }
}
