using UnityEngine;

public enum StudentClass { Default, Jock, Nerd }

[CreateAssetMenu(fileName = "New Student Data", menuName = "Student/New Student Data")]
public class StudentData : ScriptableObject
{
    public StudentClass studentClass;
    public float speedMultiplier;
    public float rowdinessMultiplier;
    public float braveryMultiplier;
}
