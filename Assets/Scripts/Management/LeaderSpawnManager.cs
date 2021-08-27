using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderSpawnManager : MonoBehaviour
{
    private void Awake()
    {
        foreach (ClassroomDoor spawn in FindObjectsOfType<ClassroomDoor>())
        {
            if (spawn.sceneNameEnter == SceneHandler.instance.lastLoadedLevelName)
            {
                StudentLeader.instance.transform.position = spawn.spawnObj.transform.position;
                Debug.Log("Found pos, tp " + spawn.spawnObj.transform.position);
                return;
            }
        }

        StudentLeader.instance.transform.position = transform.position;
    }
}
