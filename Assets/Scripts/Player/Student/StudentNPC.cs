using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StudentNPC : MonoBehaviour
{
    public StudentData data;

    public float minTimeBetweenAttack = 0.5f;
    public float maxTimeBetweenAttack = 1f;
    public float basePatianceToLose = 30f;

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

    private AudioSource audioSrc;

    public List<TeacherNPC> teachersInRange = new List<TeacherNPC>();

    public float FearLevel 
    {
        get => maxFear;
        set
        {
            if (value >= maxFear)
                LeaveGroup();

            maxFear = value;
        }
    }

    private void Awake()
    {
        aiPath.maxSpeed *= data.speedMultiplier * (1 + (Random.Range(0, 2) * 2 - 1) * Random.Range(0f, randomSpeedPercentageFactor)/100f);
        aiPath.endReachedDistance *= 1 + (Random.Range(0, 2) * 2 - 1) * Random.Range(0f, randomEndReachPercentageFactor)/100f;

        maxFear /= data.braveryMultiplier;

        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (aiPath.desiredVelocity.x <= -0.01f)
            transform.localScale = new Vector3(-1f, 1f, 1f);

        animator.SetFloat("Speed", Mathf.Abs(aiPath.desiredVelocity.x) + Mathf.Abs(aiPath.desiredVelocity.y));

        spriteRenderer.sortingOrder = Mathf.RoundToInt(-(transform.position.y - 0.25f));
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

                float patianceToLose = basePatianceToLose * data.rowdinessMultiplier;
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
        aiTargetter.target = null;

        animator.SetBool("FearMaxed", true);
        StudentManager.instance.RemoveStudent(this);
        SceneManager.MoveGameObjectToScene(gameObject, SceneHandler.instance.GetCurrentLevelScene());
        Debug.Log("Student left the group!");
    }

    public void ApplyFear(float fearValue)
    {
        float toIncrease = fearValue / data.braveryMultiplier;
        FearLevel += toIncrease;
        Debug.Log("Incrasing student fear by " + toIncrease);

        // play hurt animation
        animator.SetTrigger("Hurt");
        audioSrc.Play();
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
            collision.GetComponent<Animator>().SetTrigger("Break");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Teacher"))
        {
            teachersInRange.Remove(collision.GetComponent<TeacherNPC>());
        }
    }
}
