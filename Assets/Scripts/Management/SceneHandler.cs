using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;

    private string currentlyLoadedLevelName;
    private string pendingLoadLevelName;

    public string initialSceneName;

    public Animator fadeAnimator;
    public Transform studentLeader;

    private void Awake()
    {
        instance = this;

        SceneManager.LoadScene(initialSceneName, LoadSceneMode.Additive);
        currentlyLoadedLevelName = initialSceneName;
    }

    public void GoToScene(string sceneName)
    {
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

        currentlyLoadedLevelName = pendingLoadLevelName;

        PostLevelLoad();
    }

    private void PostLevelLoad()
    {
        // Post-load set-up

        //studentLeader.transform.position = GameObject.FindGameObjectWithTag("StudentLeaderSpawnPoint").transform.position;

    }
}
