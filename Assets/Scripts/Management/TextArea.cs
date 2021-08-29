using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextArea : MonoBehaviour
{
    public string[] textToDisplay;
    public bool pauseOnEnter;
    public bool canPauseMultipleTimes;

    private bool hasBeenShown = false;
    private int textIdx = 0;

    public bool ShouldGamePause() => pauseOnEnter && (canPauseMultipleTimes || (!canPauseMultipleTimes && !hasBeenShown && !SceneHandler.instance.CurrentLevelHasBeenLoadedBefore()));

    //Note: If any of these functions returns an empty string, it means the 'textToDisplay' array has ended! 

    public string GetFirstText()
    {
        if (!ShouldShowText() || textToDisplay.Length <= 0)
            return "";

        textIdx = 0;
        return textToDisplay[0];
    }

    public string GetNextText()
    {
        if (TitleManager.instance)
            TitleManager.instance.RemoveTitle();

        textIdx++;

        if (textToDisplay.Length <= textIdx)
        {
            hasBeenShown = true;
            return "";
        }

        return textToDisplay[textIdx];
    }
    
    private bool ShouldShowText()
    {
        if (hasBeenShown || SceneHandler.instance.CurrentLevelHasBeenLoadedBefore())
            return false;
        else
            return true;
    }
}
