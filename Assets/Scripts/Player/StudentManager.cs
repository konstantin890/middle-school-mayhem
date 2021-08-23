using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StudentManager : MonoBehaviour
{
    public Transform studentPrefab;

    public List<Student> students = new List<Student>();

    public TMP_Text studentNumberText;

    private void Awake()
    {
        SpawnLeader();
    }

    private void Update()
    {

    }

    private void SpawnLeader()
    {
        Transform studentToAdd = Instantiate(studentPrefab, Vector3.zero, Quaternion.identity);
        studentToAdd.gameObject.AddComponent(typeof(InputHandler)); //add input handler to the leader
        AddStudent(studentToAdd.GetComponent<Student>());
    }

    private void AddStudent(Student student)
    {
        student.InitiateStudent(); //initialize the student
        students.Add(student);
        studentNumberText.text = $"Students: {GetStudentCount()}";
    }

    public int GetStudentCount() => students.Count;
}
