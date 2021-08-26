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
    public TMP_Text nerdNumberText;

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

    public void AddStudent(StudentNPC student)
    {
        student.InitStudent(studentLeader); //initialize the student
        attractedStudents.Add(student);
        UpdateAttractedStudentsUI();
    }

    public void RemoveStudent(StudentNPC student)
    {
        attractedStudents.Remove(student);
        UpdateAttractedStudentsUI();
    }

    public void UpdateAttractedStudentsUI()
    {
        studentNumberText.text = $"x{GetStudentCount()}";
        nerdNumberText.text = $"x{GetStudentCountByClass(StudentClass.Nerd)}";
    }

    public int GetStudentCount() => attractedStudents.Count;

    public int GetStudentCountByClass(StudentClass sClass)
    {
        int count = 0;

        foreach (StudentNPC npc in attractedStudents)
        {
            if (npc.data.studentClass == sClass)
                count++;
        }

        return count;
    }
}
