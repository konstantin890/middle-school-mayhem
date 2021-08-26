using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chalk : MonoBehaviour
{
    public float damage = 10f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Student"))
        {
            Debug.Log("Hurt!");
            StudentNPC npc = collision.gameObject.GetComponent<StudentNPC>();

            npc.ApplyFear(damage);
        }
    }
}
