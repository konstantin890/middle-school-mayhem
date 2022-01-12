using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [Header("Preferences")]
    public Vector3 offset;

    [Header("Components")]
    public Slider slider;
    public Animator animator;

    public float maxHealth;

    private void Update()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    /// <summary>
    /// Updates the healthbar - plays animation and updates slider value
    /// </summary>
    /// <param name="newHealthValue">New health value. Will be divided by 10 for the result.</param>
    public void OnEntityHealthUpdate(float newHealthValue)
    {
        StartCoroutine(LerpFunction(newHealthValue / maxHealth, 0.5f));

        animator.SetTrigger("HealthUpdated");
    }

    private IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        float startValue = slider.value;

        while (time < duration)
        {
            slider.value = Mathf.Lerp(startValue, endValue, time / duration) ;
            time += Time.deltaTime;
            yield return null;
        }

    }
}
