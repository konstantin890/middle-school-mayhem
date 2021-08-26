using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;

    private string currentlyLoadedLevelName;

    public string initialSceneName;

    public int floorOneHallSceneId;
    public int[] floorOneSceneIds;
    public int floorTwoHallSceneId;
    public int[] floorTwoSceneIds;

    public float fadeMultiplier;

    public Animator fadeAnimator;

    private void Awake()
    {
        instance = this;

        currentlyLoadedLevelName = initialSceneName;
        SceneManager.LoadScene(initialSceneName);
    }

    public void GoToScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        fadeAnimator.SetTrigger("FadeOut");
        currentlyLoadedLevelName = sceneName;
    }

    public void OnFadeOutComplete()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentlyLoadedLevelName); // in this case currentlyLoadedLevelName is the scene to be loaded next

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
