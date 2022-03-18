// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021-2022 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021-2022 Nikos Konstantinou (nikoskon2003)
//
// The following script has been written by either konstantin890 or Nikos (nikoskon2003) or both.
// This file is covered by the GNU GPL v3 license. Read LICENSE.md for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public List<ClassroomDoor> doors = new List<ClassroomDoor>();

    public bool radialStudentSpawn = true;

    private void Awake()
    {
        HandleStudentSpawn();
        ChangeDoorStates();
    }

    private void HandleStudentSpawn()
    {
        bool foundNonStandardSpawnPos = false;
        foreach (ClassroomDoor spawn in doors)
        {
            if (spawn.sceneNameEnter == SceneHandler.instance.lastLoadedLevelName)
            {
                StudentLeader.instance.transform.position = spawn.spawnObj.transform.position;
                Debug.Log("Found pos, tp " + spawn.spawnObj.transform.position);
                foundNonStandardSpawnPos = true;
            }
        }

        if (!foundNonStandardSpawnPos)
        {
            Transform spawnPoint = GameObject.FindGameObjectWithTag("StudentLeaderSpawnPoint").transform;
            StudentLeader.instance.transform.position = spawnPoint.position;
        }

        Transform leaderPos = StudentLeader.instance.transform;
        Debug.Log("Leader pos: " + leaderPos.position);

        foreach (StudentNPC npc in StudentManager.instance.attractedStudents)
        {
            Vector2 posToSpawn;
            if (radialStudentSpawn)
                posToSpawn = new Vector2(Random.Range(leaderPos.position.x - 1, leaderPos.position.x + 1), Random.Range(leaderPos.position.y - 1, leaderPos.position.y));
            else
                posToSpawn = leaderPos.position;

            npc.transform.position = posToSpawn;
            Debug.Log("Decided pos: " + posToSpawn);
        }
    }

    private void ChangeDoorStates()
    {
        int lastOpenedDoor = GetLastOpenedDoor();
        Debug.Log("Last opened door ID " + lastOpenedDoor);

        for (int i = 0; i < doors.Count; i++)
        {
            ClassroomDoor door = doors[i];

            if (doors.Count > 1) // check if we're in a hall
            {
                if (SceneHandler.instance.previousUnloadedLevels.Contains(door.sceneNameEnter)) // check if the level has been loaded before
                {
                    door.UnBarDoor(); // this door has been opened before
                    door.MarkDoorAsOpen(); 
                    continue;
                }

                //doors[lastOpenedDoor + 1].UnBarDoor();

                int doorExpectedToEnter = lastOpenedDoor + 1;

                if (i == doorExpectedToEnter) // this is one door ahead of the last opened door so: 0 0 X <--
                {
                    // this door should be unlocked as expected, though the ones after it should be locked

                    door.UnBarDoor();
                    Debug.Log("Unlock!, because last open is " + lastOpenedDoor + " and I am " + i);
                }
                else if (i > doorExpectedToEnter)
                    door.LockDoor();
            }
        }
    }

    private int GetLastOpenedDoor()
    {
        return doors.FindLastIndex(d => HasDoorBeenOpened(d) == true);

        //Debug.LogError("There are no open doors in this level?!");
        //return 0;
    }

    private bool HasDoorBeenOpened(ClassroomDoor door)
    {
        if (SceneHandler.instance.previousUnloadedLevels.Contains(door.sceneNameEnter)) // check if the level has been loaded before
        {
            return true; // this door has been opened before
        }

        return false;
    }
}
