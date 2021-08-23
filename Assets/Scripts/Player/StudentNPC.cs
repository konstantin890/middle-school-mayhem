using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentNPC : MonoBehaviour
{
    [Header("Debug values")]
    [Range(0, 1)] public float fearLevel;
    private bool isAttracted;

    private void Update()
    {
        
    }

    public void InitStudent()
    {
        isAttracted = true;
    }
}
