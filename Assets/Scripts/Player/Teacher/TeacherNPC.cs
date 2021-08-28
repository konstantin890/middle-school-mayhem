using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherNPC : MonoBehaviour
{
    public TeacherData type;
    public Animator animator;

    [Header("Hall Teacher")]
    public float hallSecondsBetweenYellAttackMin;
    public float hallSecondsBetweenYellAttackMax;

    [Header("Subs Teacher")]
    public Transform chalkPrefab;
    private Transform thrownChalk;
    public float chalkSpeed;
    public float chalkFearMultiplier;
    public float subsSecondsBetweenScoldAttackMin;
    public float subsSecondsBetweenScoldAttackMax;
    public float subsSecondsBetweenChalkAttackMin;
    public float subsSecondsBetweenChalkAttackMax;

    [Header("Science Teacher")]
    public float prismFearMultiplier;
    public float scienceSecondsBetweenPrismAttackMin;
    public float scienceSecondsBetweenPrismAttackMax;
    public float fPaperFearMultiplier;
    public float scienceSecondsBetweenFPaperAttackMin;
    public float scienceSecondsBetweenFPaperAttackMax;

    public float baseFearValue = 10f;
    [Range(0, 1)] public float patianceLevel;
    private List<StudentNPC> studentsInsideArea = new List<StudentNPC>();

    private bool isAngry;
    public bool IsAngry
    {
        get { return isAngry; }
        set 
        {
            animator.SetBool("IsAngry", value); // used for angry walk
            isAngry = value;
        }
    }

    private bool isItchy;
    public bool IsItchy
    {
        get { return isItchy; }
        set
        {
            animator.SetBool("IsItchy", value); // used for angry walk
            isItchy = value;
        }
    }

    private AudioSource soundSrc;

    public AudioClip throwChalkSound;
    public AudioClip giveupSound;
    public AudioClip[] scoldSounds = new AudioClip[2];

    private void Awake()
    {
        // Attack loop
        ExecuteAttackLoop();

        soundSrc = GetComponent<AudioSource>();
    }

    public void ExecuteAttackLoop()
    {
        switch (type.variation)
        {
            case TeacherVariation.HallMonitor:
                StartCoroutine(AttackLoopHall());
                break;

            case TeacherVariation.Sub:
                StartCoroutine(AttackLoopSub());
                break;

            case TeacherVariation.Science:
                StartCoroutine(AttackLoopScience());
                break;

            default:
                Debug.LogError("Teacher not implemented yet!");
                break;
        }
    }


    private IEnumerator AttackLoopHall()
    {
        while (true)
        {
            yield return new WaitUntil(() => studentsInsideArea.Count > 1);
            Debug.Log("Enoughs students inside area!");
            yield return new WaitForSeconds(Random.Range(hallSecondsBetweenYellAttackMin, hallSecondsBetweenYellAttackMax));
            Debug.Log("Time to strike!");

            soundSrc.clip = scoldSounds[Random.Range(0, 2)]; //2 is exclusive, so 0 or 1
            soundSrc.Play();

            ApplyFearToAllStudentsInArea(baseFearValue * type.fearMultiplier);

            animator.SetBool("Scold", true);
            yield return new WaitForSeconds(1f);
            animator.SetBool("Scold", false);
        }
    }

    private IEnumerator AttackLoopSub()
    {
        while (true)
        {
            yield return new WaitUntil(() => studentsInsideArea.Count > 1);
            int attackID = Random.Range(0, 2);
            if (attackID == 0)
            {
                yield return new WaitForSeconds(Random.Range(subsSecondsBetweenScoldAttackMin, subsSecondsBetweenScoldAttackMax));

                ApplyFearToAllStudentsInArea(baseFearValue * type.fearMultiplier);

                animator.SetBool("Scold", true);
                yield return new WaitForSeconds(1f);
                animator.SetBool("Scold", false);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(subsSecondsBetweenChalkAttackMin, subsSecondsBetweenChalkAttackMax));

                thrownChalk = Instantiate(chalkPrefab, transform.position, Quaternion.identity);
                Rigidbody2D chalkRb = thrownChalk.GetComponent<Rigidbody2D>();
                Transform target = StudentManager.instance.GetRandomStudent();
                chalkRb.AddForce((target.position - transform.position) * chalkSpeed, ForceMode2D.Impulse);
                animator.SetTrigger("Subs_ThrowChalk");
                Destroy(thrownChalk.gameObject, 3f);

                soundSrc.clip = throwChalkSound;
                soundSrc.Play();
            }
        }
    }

    private IEnumerator AttackLoopScience()
    {
        while (true)
        {
            yield return new WaitUntil(() => studentsInsideArea.Count > 1);
            int attackID = Random.Range(0, 2);
            if (attackID == 0)
            {
                yield return new WaitForSeconds(Random.Range(scienceSecondsBetweenPrismAttackMin, scienceSecondsBetweenPrismAttackMax));

                animator.SetTrigger("Maths_PrismBeam");

                ApplyFearToAllStudentsInArea(baseFearValue * type.fearMultiplier * prismFearMultiplier);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(scienceSecondsBetweenFPaperAttackMin, scienceSecondsBetweenFPaperAttackMax));

                animator.SetTrigger("Maths_ThrowFPaper");

                ApplyFearToAllStudentsInArea(baseFearValue * type.fearMultiplier * fPaperFearMultiplier);
            }
        }
    }

    private void ApplyFearToAllStudentsInArea(float fearToApply)
    {
        foreach (StudentNPC student in studentsInsideArea)
        {
            student.ApplyFear(fearToApply);
        }
    }

    public void LeaveClass()
    {
        // AI to door, corroutine, after that, disapear (animation?)

        soundSrc.clip = giveupSound;
        soundSrc.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Student"))
        {
            Debug.Log("A Student entered AoE");
            studentsInsideArea.Add(collision.gameObject.GetComponent<StudentNPC>());
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Student"))
        {
            StudentNPC npc = collision.gameObject.GetComponent<StudentNPC>();
            Debug.Log("A Student entered AoE");
            if (studentsInsideArea.Contains(npc))
                studentsInsideArea.Remove(npc);
        }
    }
}
