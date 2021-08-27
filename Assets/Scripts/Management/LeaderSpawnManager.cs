using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderSpawnManager : MonoBehaviour
{
    private void Awake()
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
            StudentLeader.instance.transform.position = transform.position;

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
