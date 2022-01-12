// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021 Nikos Konstantinou (nikoskon2003)
//
// The following script has been written by either konstantin890 or Nikos (nikoskon2003) or both.
// This file is covered by the GNU GPL v3 license. Read LICENSE.md for more information.

using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public static TitleManager instance;
    public Animator titleAnimator;
    public Animator creditsAnimator;
    public GameObject creditsPanel;

    private bool titleOpen = true;
    private bool creditsOpen;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (titleOpen && !creditsOpen && InputHandler.instance.CheckIfButtonPressed("Open Credits", 4))
        {
            // Show credits
            Debug.Log("Show credits");
            creditsOpen = true;
            creditsAnimator.SetTrigger("FadeOut");
        }
        else if (creditsOpen && InputHandler.instance.CheckIfButtonPressed("Open Credits", 4))
        {
            // Close credits
            Debug.Log("Hide credits");
            creditsOpen = false;
            creditsAnimator.SetTrigger("FadeIn");
        }
    }

    public void RemoveTitle()
    {
        titleOpen = false;
        titleAnimator.SetTrigger("MoveAway");
    }
}
