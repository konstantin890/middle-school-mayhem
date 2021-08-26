using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;

    public int floorOneHallSceneId;
    public int[] floorOneSceneIds;
    public int floorTwoHallSceneId;
    public int[] floorTwoSceneIds;

    private void Awake()
    {
        instance = this;
    }
}
