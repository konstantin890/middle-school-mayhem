// Developed by TerraStudios (https://github.com/TerraStudios)
//
// Copyright(c) 2021-2022 Konstantin Milev (konstantin890 | milev109@gmail.com)
// Copyright(c) 2021-2022 Nikos Konstantinou (nikoskon2003)
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

    public System.Guid uniqueness;

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
                    SoundManager.instance.PlayShoutingSound();
                }
                else
                {
                    animator.SetBool("IsShouting", false);
                    SoundManager.instance.StopShoutingSound();
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

        spriteRenderer.sortingOrder = Mathf.RoundToInt(-(transform.position.y - (spriteRenderer.bounds.size.y / 2)) * 100);

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
            float randomDelay = Random.Range(minTimeBetweenAttack, maxTimeBetweenAttack);

            yield return new WaitForSeconds(randomDelay);

            foreach (TeacherNPC npc in teachersInRange)
            {
                if (npc == null)
                    continue;

                Debug.Log("Reducing Teacher patience by " + patianceToLose);
                npc.LosePatiance(patianceToLose);
            }
        }
    }

    public void InitStudent(Transform leader)
    {
        isAttracted = true;
        aiTargetter.target = leader;
        uniqueness = System.Guid.NewGuid();
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
        Debug.Log("Increasing student fear by " + fearValue);

        // play hurt animation
        animator.SetTrigger("Hurt");
        SoundManager.instance.PlayHurtSound();
        //audioSrcHurt.Play();
    }

    public bool IsAttracted() => isAttracted;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttracted && collision.gameObject.CompareTag("Teacher"))
        {
            TeacherNPC detectedTeacher = collision.GetComponent<TeacherNPC>();

            if (!teachersInRange.Contains(detectedTeacher))
            {
                teachersInRange.Add(detectedTeacher);
            }
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
        if (isAttracted && collision.gameObject.CompareTag("Teacher"))
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
