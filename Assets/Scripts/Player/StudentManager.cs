using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StudentManager : MonoBehaviour
{
    public static StudentManager instance;

    public Transform studentPrefab;
    public Transform studentLeader;

    public List<StudentNPC> attractedStudents = new List<StudentNPC>();

    public TMP_Text studentNumberText;

    private void Awake()
    {
        instance = this;

        //SpawnLeader();
    }

    private void Update()
    {

    }

    /*private void SpawnLeader()
    {
        Transform studentToAdd = Instantiate(studentPrefab, Vector3.zero, Quaternion.identity);
        studentToAdd.gameObject.AddComponent(typeof(InputHandler)); //add input handler to the leader
        AddStudent(studentToAdd.GetComponent<StudentNPC>());
    }*/

    private void AddStudent(StudentNPC student)
    {
        student.InitStudent(studentLeader); //initialize the student
        attractedStudents.Add(student);
        studentNumberText.text = $"Students: {GetStudentCount()}";
    }

    public int GetStudentCount() => attractedStudents.Count;
}
