using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherNPC : MonoBehaviour
{
    public TeacherData type;

    [Range(0, 1)] public float patianceLevel;

    public void ExecuteAttack()
    {

    }

    public void LeaveClass()
    {
        // AI to door, corroutine, after that, disapear (animation?)
    }
}
