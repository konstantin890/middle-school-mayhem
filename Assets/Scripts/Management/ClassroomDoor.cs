using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomDoor : MonoBehaviour
{
    public bool toUpperFloor;
    public int currentFloor;
    public int roomNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("StudentLeader"))
        {
            Debug.Log("Student Enter!");
        }
    }
}
