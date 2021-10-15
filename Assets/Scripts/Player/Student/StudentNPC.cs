// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021 Nikos Konstantinou (nikoskon2003)
//
// The following script has been written by either konstantin890 or Nikos (nikoskon2003) or both.
// This file is covered by the GNU GPL v3 license. Read LICENSE.md for more information.

using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StudentClass { Normal, Jock, Nerd }

public class StudentNPC : MonoBehaviour
{
    public StudentClass studentClass;

    public float minTimeBetweenAttack = 0.5f;
    public float maxTimeBetweenAttack = 1f;
    public float patianceToLose = 30f;

    [Header("Debug values")]
    public float maxFear = 100;
    private bool isAttracted;

    public AIPath aiPath;
    public AIDestinationSetter aiTargetter;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Range(0, 100)]
    public float randomSpeedPercentageFactor = 5f;
    [Range(0, 100)]
    public float randomEndReachPercentageFactor = 10f;

    public AudioSource audioSrcHurt;
    public AudioSource audioSrcChaos;

    public List<TeacherNPC> teachersInRange = new List<TeacherNPC>();

    private float fearLevel;
    public float FearLevel 
    {
        get => fearLevel;
        set
        {
            if (value >= maxFear)
                LeaveGroup();

            fearLevel = value;
        }
    }

    public bool IsShouting 
    {
        get => isShouting;
        set
        {
            if (value != isShouting)
            {
                if (value)
                {
                    animator.SetBool("IsShouting", true);
                    Debug.Log("Play!");
                    SoundManager.instance.PlayShoutingSound();
                    //audioSrcChaos.Play();
                }
                else
                {
                    animator.SetBool("IsShouting", false);
                    Debug.Log("Stop!");
                    SoundManager.instance.StopShoutingSound();
                    //audioSrcChaos.Stop();
                }
            }

            isShouting = value;
        }
    }

    private bool isShouting;

    private void Awake()
    {
        aiPath.maxSpeed *= 1 + (Random.Range(0, 2) * 2 - 1) * Random.Range(0f, randomSpeedPercentageFactor) / 100f;
        aiPath.endReachedDistance *= 1 + (Random.Range(0, 2) * 2 - 1) * Random.Range(0f, randomEndReachPercentageFactor) / 100f;
    }

    private void Update()
    {
        RefreshTeachersList();

        if (aiPath.desiredVelocity.x >= 0.01f)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (aiPath.desiredVelocity.x <= -0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        animator.SetFloat("Speed", Mathf.Abs(aiPath.desiredVelocity.x) + Mathf.Abs(aiPath.desiredVelocity.y));

        spriteRenderer.sortingOrder = Mathf.RoundToInt(-(transform.position.y - 0.25f));

        if (teachersInRange.Count > 0)
        {
            IsShouting = true;
        }
        else
        {
            IsShouting = false;
        }
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTimeBetweenAttack, maxTimeBetweenAttack));

            foreach (TeacherNPC npc in teachersInRange)
            {
                if (npc == null)
                    continue;

                Debug.Log("Reducing Teacher patiance with " + patianceToLose);
                npc.LosePatiance(patianceToLose);
            }
        }
    }

    public void InitStudent(Transform leader)
    {
        isAttracted = true;
        aiTargetter.target = leader;
        DontDestroyOnLoad(this);
        StartCoroutine(AttackLoop());
    }

    public void LeaveGroup()
    {
        isAttracted = false;

        Destroy(aiTargetter);
        Destroy(aiPath);

        animator.SetBool("FearMaxed", true);
        StudentManager.instance.RemoveStudent(this);
        Debug.Log("Destructable scene " + SceneHandler.instance.GetCurrentLevelScene());
        SceneManager.MoveGameObjectToScene(gameObject, SceneHandler.instance.GetCurrentLevelScene());
        Debug.Log("Student left the group!");

        gameObject.tag = "Untagged";

        Destroy(this);
    }

    public void ApplyFear(float fearValue)
    {
        FearLevel += fearValue;
        Debug.Log("Incrasing student fear by " + fearValue);

        // play hurt animation
        animator.SetTrigger("Hurt");
        SoundManager.instance.PlayHurtSound();
        //audioSrcHurt.Play();
    }

    public bool IsAttracted() => isAttracted;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Teacher"))
        {
            teachersInRange.Add(collision.GetComponent<TeacherNPC>());
        }

        if (collision.gameObject.CompareTag("Destructible"))
        {
            Animator destructableAnimator = collision.GetComponent<Animator>();
            destructableAnimator.SetTrigger("Break");
            collision.gameObject.tag = "Untagged";
            animator.SetTrigger("IsInteracting");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Teacher"))
        {
            teachersInRange.Remove(collision.GetComponent<TeacherNPC>());
        }
    }

    private void RefreshTeachersList()
    {
        for (int i = 0; i < teachersInRange.Count; i++)
        {
            if (teachersInRange[i] == null)
                teachersInRange.RemoveAt(i);
        }
    }
}
