// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021-2022 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021-2022 Nikos Konstantinou (nikoskon2003)
//
// The following script has been written by either konstantin890 or Nikos (nikoskon2003) or both.
// This file is covered by the GNU GPL v3 license. Read LICENSE.md for more information.

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StudentManager : MonoBehaviour
{
    public static StudentManager instance;

    public Transform studentPrefab;
    public Transform studentLeader;

    public List<StudentNPC> attractedStudents = new List<StudentNPC>();

    public TMP_Text studentNumberText;
    public TMP_Text nerdNumberText;
    public TMP_Text jockNumberText;

    public List<Transform> teachersPrincipalCanSpawn = new List<Transform>();

    private void Awake()
    {
        instance = this;

        //SpawnLeader();
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

        if (attractedStudents.Count == 0)
        {
            Debug.Log("Game Over!");
            SceneHandler.instance.GameOver();
        }
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
            if (npc.studentClass == sClass)
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
