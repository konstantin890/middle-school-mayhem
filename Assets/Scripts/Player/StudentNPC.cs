using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StudentClass { Jock, Nerd, Prankster }

public class StudentNPC : MonoBehaviour
{
    public StudentClass studentClass;

    [Header("Debug values")]
    [Range(0, 1)] public float fearLevel;
    private bool isAttracted;

    public AIPath aiPath;
    public AIDestinationSetter aiTargetter;

    private void Start()
    {
        //InitStudent(StudentLeader.instance.transform);
    }

    private void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (aiPath.desiredVelocity.x <= -0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    public void InitStudent(Transform leader)
    {
        isAttracted = true;
        aiTargetter.target = leader;
    }

    public bool IsAttracted() => isAttracted;
}
