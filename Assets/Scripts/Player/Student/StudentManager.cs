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
    public TMP_Text jockNumberText;

    private void Awake()
    {
        instance = this;

        //SpawnLeader();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && attractedStudents.Count > 0) 
        {
            attractedStudents[0].ApplyFear(20);
        }
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
        jockNumberText.text = $"x{GetStudentCountByClass(StudentClass.Jock)}";
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

    public Transform GetRandomStudent()
    {
        if (attractedStudents.Count == 0)
            return null;

        int listPos = Random.Range(0, attractedStudents.Count);

        return attractedStudents[listPos].transform;
    }
}
