// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021-2022 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021-2022 Nikos Konstantinou (nikoskon2003)
//
// The following script has been written by either konstantin890 or Nikos (nikoskon2003) or both.
// This file is covered by the GNU GPL v3 license. Read LICENSE.md for more information.

using Cinemachine;
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
