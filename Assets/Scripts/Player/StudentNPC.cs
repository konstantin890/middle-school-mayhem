using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentNPC : MonoBehaviour
{
    [Header("Debug values")]
    [Range(0, 1)] public float fearLevel;
    private bool isAttracted;

    public AIDestinationSetter aiTargetter;

    private void Start()
    {
        //InitStudent(StudentLeader.instance.transform);
    }

    public void InitStudent(Transform leader)
    {
        isAttracted = true;
        aiTargetter.target = leader;
    }

    public bool IsAttracted() => isAttracted;
}
