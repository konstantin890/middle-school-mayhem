using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public float damageDealt;
    public float detectRadius;

    public bool penetratesStudent;
    public bool doesAreaDamage;
    public float damageAreaRadius;

    private List<int> studentsHit = new List<int>();

    private void Update()
    {
        Collider2D[] collResults = Physics2D.OverlapCircleAll(transform.position, detectRadius);
        foreach (Collider2D hit in collResults)
        {
            if (hit.gameObject.CompareTag("Student"))
            {
                int id = hit.gameObject.GetInstanceID();
                if (studentsHit.IndexOf(id) >= 0)
                    continue;

                if (!doesAreaDamage)
                    hit.gameObject.GetComponent<StudentNPC>().ApplyFear(damageDealt);
                else
                    DoAreaDamage();

                if (!penetratesStudent)
                {
                    Destroy(gameObject);
                    return;
                }

                studentsHit.Add(id);
            }
        }
    }

    private void DoAreaDamage() 
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, damageAreaRadius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Student"))
                hitCollider.gameObject.GetComponent<StudentNPC>().ApplyFear(damageDealt);
        }
    }
}
