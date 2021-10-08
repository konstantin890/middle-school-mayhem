// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021 Nikos Konstantinou (nikoskon2003)
//
// The following script has been written by either konstantin890 or Nikos (nikoskon2003) or both.
// This file is covered by the GNU GPL v3 license. Read LICENSE.md for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    private void Awake()
    {
        HandleStudentSpawn();
    }

    private void HandleStudentSpawn()
    {
        bool foundNonStandardSpawnPos = false;
        foreach (ClassroomDoor spawn in FindObjectsOfType<ClassroomDoor>())
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
            Vector2 posToSpawn = new Vector2(Random.Range(leaderPos.position.x - 2, leaderPos.position.x + 2), Random.Range(leaderPos.position.y - 2, leaderPos.position.y));
            npc.transform.position = posToSpawn;
            Debug.Log("Decided pos: " + posToSpawn);
        }
    }
}
