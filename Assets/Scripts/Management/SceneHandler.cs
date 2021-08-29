using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;

    [HideInInspector] public string lastLoadedLevelName;
    private string currentlyLoadedLevelName;
    private string pendingLoadLevelName;

    public string initialSceneName;

    public Animator fadeAnimator;
    public Transform studentLeader;

    private List<string> unloadedLevels = new List<string>();

    private Dictionary<string, List<Vector2>> pickedUpChemicals = new Dictionary<string, List<Vector2>>();
    private Dictionary<string, List<Vector2>> pickedUpStudents = new Dictionary<string, List<Vector2>>();
    private Dictionary<string, List<Vector2>> unlockedDoors = new Dictionary<string, List<Vector2>>();
    private Dictionary<string, List<Vector2>> removedTeachers = new Dictionary<string, List<Vector2>>();

    public GameObject GameOverGO;
    public TMP_Text gameOverText;

    private void Awake()
    {
        instance = this;

        SceneManager.LoadScene(initialSceneName, LoadSceneMode.Additive);
        currentlyLoadedLevelName = initialSceneName;
        //PostLevelLoad();
    }

    public void GoToScene(string sceneName)
    {
        studentLeader.GetComponent<StudentLeader>().ForcePausePlayer();
        fadeAnimator.SetTrigger("FadeOut");
        pendingLoadLevelName = sceneName;
    }

    public void OnFadeOutComplete()
    {
        if (currentlyLoadedLevelName != null)
            StartCoroutine(UnloadPastScene());
        else
            LoadNextScene();
    }

    private IEnumerator UnloadPastScene()
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(currentlyLoadedLevelName);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(pendingLoadLevelName, LoadSceneMode.Additive);

        lastLoadedLevelName = currentlyLoadedLevelName;
        currentlyLoadedLevelName = pendingLoadLevelName;

        if (unloadedLevels.IndexOf(lastLoadedLevelName) < 0)
            unloadedLevels.Add(lastLoadedLevelName);

        PostLevelLoad();
    }

    private void PostLevelLoad()
    {
        // Post-load set-up

        //studentLeader.transform.position = GameObject.FindGameObjectWithTag("StudentLeaderSpawnPoint").transform.position;

        // TODO: Reinstantiate the students around the Leader, could be random pos in a sphere

        StinkBomb[] bombs = FindObjectsOfType<StinkBomb>();
        foreach(StinkBomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }

        if (!pickedUpChemicals.ContainsKey(currentlyLoadedLevelName))
            pickedUpChemicals.Add(currentlyLoadedLevelName, new List<Vector2>());

        if (!pickedUpStudents.ContainsKey(currentlyLoadedLevelName))
            pickedUpStudents.Add(currentlyLoadedLevelName, new List<Vector2>());

        if (!unlockedDoors.ContainsKey(currentlyLoadedLevelName))
            unlockedDoors.Add(currentlyLoadedLevelName, new List<Vector2>());

        if (!removedTeachers.ContainsKey(currentlyLoadedLevelName))
            removedTeachers.Add(currentlyLoadedLevelName, new List<Vector2>());

        if (CurrentLevelHasBeenLoadedBefore()) 
            StartCoroutine(DestroyClass());

        studentLeader.GetComponent<StudentLeader>().UnpausePlayer();

        foreach (StudentNPC npc in StudentManager.instance.attractedStudents)
        {
            npc.teachersInRange = new List<TeacherNPC>(); // clear the list when switching scenes because the teacher no longer exist
        }
    }

    public Scene GetCurrentLevelScene()
    {
        return SceneManager.GetSceneByName(currentlyLoadedLevelName);
    }

    public bool CurrentLevelHasBeenLoadedBefore() => unloadedLevels.IndexOf(currentlyLoadedLevelName) >= 0;

    private IEnumerator DestroyClass() 
    {
        yield return new WaitForSeconds(0.05f);
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Destructable");

        foreach (GameObject go in objects)
        {
            go.GetComponent<Animator>().SetTrigger("Break");
        }

        objects = GameObject.FindGameObjectsWithTag("Destructible");
        foreach (GameObject go in objects)
        {
            go.GetComponent<Animator>().SetTrigger("Break");
        }

        objects = GameObject.FindGameObjectsWithTag("Teacher");
        foreach (GameObject go in objects)
        {
            foreach (Vector2 pos in removedTeachers[currentlyLoadedLevelName])
            {
                if (Vector2.Distance(go.transform.position, pos) < 0.05f)
                {
                    Destroy(go);
                    break;
                }
            }
        }

        objects = GameObject.FindGameObjectsWithTag("Student");
        foreach (GameObject go in objects)
        {
            if (go.scene.buildIndex != -1)
            {
                foreach (Vector2 pos in pickedUpStudents[currentlyLoadedLevelName])
                {
                    if (Vector2.Distance(go.transform.position, pos) < 0.05f)
                    {
                        Destroy(go);
                        break;
                    }
                }
            }
        }

        objects = GameObject.FindGameObjectsWithTag("Chemical");
        foreach (GameObject go in objects)
        {
            foreach (Vector2 pos in pickedUpChemicals[currentlyLoadedLevelName]) 
            {
                if(Vector2.Distance(go.transform.position, pos) < 0.05f) 
                {
                    Destroy(go);
                    break;
                }
            }
        }

        objects = GameObject.FindGameObjectsWithTag("EnterArea");
        foreach (GameObject go in objects)
        {
            foreach (Vector2 pos in unlockedDoors[currentlyLoadedLevelName])
            {
                if (Vector2.Distance(go.transform.position, pos) < 0.05f)
                {
                    go.GetComponent<ClassroomDoor>().UnBarDoor();
                    break;
                }
            }
        }

    }

    public void PickUpChemicalOnScene(Vector2 pos) 
    {
        if (!pickedUpChemicals.ContainsKey(currentlyLoadedLevelName))
            pickedUpChemicals.Add(currentlyLoadedLevelName, new List<Vector2>());

        pickedUpChemicals[currentlyLoadedLevelName].Add(pos); 
    }

    public void PickUpStudent(Vector2 pos) 
    {
        if (!pickedUpStudents.ContainsKey(currentlyLoadedLevelName))
            pickedUpStudents.Add(currentlyLoadedLevelName, new List<Vector2>());

        pickedUpStudents[currentlyLoadedLevelName].Add(pos); 
    }

    public void UnlockDoor(Vector2 pos) //TODO: Probably not needed anymore since the door design has been changed
    {
        if (!unlockedDoors.ContainsKey(currentlyLoadedLevelName))
            unlockedDoors.Add(currentlyLoadedLevelName, new List<Vector2>());

        unlockedDoors[currentlyLoadedLevelName].Add(pos);
    }

    public void RemoveTeacher(Vector2 pos)
    {
        if (!removedTeachers.ContainsKey(currentlyLoadedLevelName))
            removedTeachers.Add(currentlyLoadedLevelName, new List<Vector2>());

        removedTeachers[currentlyLoadedLevelName].Add(pos);
    }

    public void GameOver()
    {
        GameOverGO.SetActive(true);
        StartCoroutine(GameOverCountdown());
    }

    private IEnumerator GameOverCountdown()
    {
        gameOverText.text = "Restarting in 3..";
        yield return new WaitForSeconds(1f);
        gameOverText.text = "Restarting in 2..";
        yield return new WaitForSeconds(1f);
        gameOverText.text = "Restarting in 1..";
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
