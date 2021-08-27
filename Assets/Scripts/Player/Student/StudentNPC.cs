using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StudentNPC : MonoBehaviour
{
    public StudentData data;

    [Header("Debug values")]
    [Range(0, 1)] private float fearLevel = 0f;
    public float maxFear = 100;
    private bool isAttracted;

    public AIPath aiPath;
    public AIDestinationSetter aiTargetter;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private AudioSource audioSrc;

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

    private void Awake()
    {
        aiPath.maxSpeed *= data.speedMultiplier;
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

        spriteRenderer.sortingOrder = Mathf.FloorToInt(-transform.position.y);
    }

    public void InitStudent(Transform leader)
    {
        isAttracted = true;
        aiTargetter.target = leader;
        DontDestroyOnLoad(this);
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
        FearLevel += fearValue / data.braveryMultiplier;
        // play hurt animation
        animator.SetTrigger("Hurt");
        audioSrc.Play();
    }

    public bool IsAttracted() => isAttracted;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Teacher"))
        {
            Debug.Log("A Teacher entered AoE");
        }

        if (collision.gameObject.CompareTag("Destructible"))
        {
            collision.GetComponent<Animator>().SetTrigger("Break");
        }
    }
}
