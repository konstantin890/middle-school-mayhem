using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager instance;

    public Collider2D confiner;

    private void Awake()
    {
        instance = this;
        RehookForScene();
    }

    public void RehookForScene()
    {
        GameObject.FindGameObjectWithTag("Confiner").GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = confiner;
    }
}
