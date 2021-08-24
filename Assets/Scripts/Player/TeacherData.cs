using UnityEngine;

public enum TeacherVariation 
{
    HallMonitor,
    Sub,
    Coach,
    Science,
    Math,
    Geography,
    Principal
}

[CreateAssetMenu(fileName = "New Teacher Data", menuName = "Teacher/New Teacher Data")]
public class TeacherData : ScriptableObject
{
    [Header("Teacher Properties for variation")]
    public TeacherVariation variation;

    public float speedMultiplier;
    public float patianceMultiplier;
    public float fearMultiplier;
}
