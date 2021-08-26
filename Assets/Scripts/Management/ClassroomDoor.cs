using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomDoor : MonoBehaviour
{
    public string sceneNameEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("StudentLeader"))
        {
            Debug.Log("Student Enter!");
            SceneHandler.instance.GoToScene(sceneNameEnter);
        }
    }
}
