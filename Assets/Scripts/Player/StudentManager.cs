using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudentManager : MonoBehaviour
{
    public Transform studentPrefab;

    public List<Student> students = new List<Student>();

    private void Awake()
    {
        Instantiate(studentPrefab, Vector3.zero, Quaternion.identity);
    }

    public int GetStudentCount() => students.Count;
}
