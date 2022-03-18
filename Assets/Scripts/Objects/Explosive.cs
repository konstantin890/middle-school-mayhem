// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021-2022 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021-2022 Nikos Konstantinou (nikoskon2003)
//
// The following script has been written by either konstantin890 or Nikos (nikoskon2003) or both.
// This file is covered by the GNU GPL v3 license. Read LICENSE.md for more information.

using System.Collections;
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
        //ok, I know I hard-coded this shit and it is BAD!
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("Explode");
        yield return new WaitForSeconds(0.3f);
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
                SceneHandler.instance.UnlockDoor(hitCollider.transform.position);
                if (classroomDoor.isFocused)
                    StudentLeader.instance.inventory.SetPopupText("Enter room?");
            }
        }
    }
}
