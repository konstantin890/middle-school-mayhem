using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour
{
    [Header("Debug values")]
    public bool isLeader;
    [Range(0,1)] public float fearLevel;

    private void Update()
    {
        if (fearLevel == 1)
        {
            // leave group?
        }

        // Handle movement
    }
}
