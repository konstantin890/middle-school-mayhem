using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentNPC : MonoBehaviour
{
    public StudentData data;

    [Header("Debug values")]
    [Range(0, 1)] public float fearLevel;
    private bool isAttracted;

    public AIPath aiPath;
    public AIDestinationSetter aiTargetter;
    public Animator animator;

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

        animator.SetFloat("Speed", Mathf.Abs(aiPath.desiredVelocity.x) + Mathf.Abs(aiPath.desiredVelocity.y));
    }

    public void InitStudent(Transform leader)
    {
        isAttracted = true;
        aiTargetter.target = leader;
    }

    public bool IsAttracted() => isAttracted;
}
