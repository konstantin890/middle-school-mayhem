using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderSpawnPoint : MonoBehaviour
{
    private void Awake()
    {
        GameObject.FindGameObjectWithTag("StudentLeader").transform.position = transform.position;
    }
}
