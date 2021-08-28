using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeacherNPC : MonoBehaviour
{
    public static TeacherNPC instance; // Looks weird, but we need it for detecting if there are any teachers left in the room.

    public TeacherData type;
    public Animator animator;
    public AIDestinationSetter aiPathSetter;
    public AIPath aiPath;

    [Header("Hall Teacher")]
    public float hallSecondsBetweenYellAttackMin;
    public float hallSecondsBetweenYellAttackMax;

    [Header("Subs Teacher")]
    public Transform chalkPrefab;
    private Transform thrownChalk;
    public float chalkSpeed;
    public float subsSecondsBetweenScoldAttackMin;
    public float subsSecondsBetweenScoldAttackMax;
    public float subsSecondsBetweenChalkAttackMin;
    public float subsSecondsBetweenChalkAttackMax;

    [Header("Science Teacher")]
    public Transform prismPrefab;
    private Transform thrownPrism;
    public float prismSpeed;
    public float scienceSecondsBetweenPrismAttackMin;
    public float scienceSecondsBetweenPrismAttackMax;
    public Transform fPaperPrefab;
    private Transform thrownFPaper;
    public float fPaperSpeed;
    public float scienceSecondsBetweenFPaperAttackMin;
    public float scienceSecondsBetweenFPaperAttackMax;

    public float baseFearValue = 10f;
    public float initialPatianceLevel = 100f;
    private List<StudentNPC> studentsInsideArea = new List<StudentNPC>();

    [HideInInspector]
    public Vector2 initialPoition;

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

    public float PatianceLevel 
    {
        get => initialPatianceLevel;
        set
        {
            if (value <= 0)
            {
                LeaveClass();
            }
            initialPatianceLevel = value;
        }
    }

    private AudioSource soundSrc;

    public AudioClip throwChalkSound;
    public AudioClip giveupSound;
    public AudioClip[] scoldSounds = new AudioClip[2];

    private void Awake()
    {
        instance = this;

        // Attack loop
        ExecuteAttackLoop();

        soundSrc = GetComponent<AudioSource>();

        initialPoition = transform.position;
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
            yield return new WaitForSeconds(Random.Range(hallSecondsBetweenYellAttackMin, hallSecondsBetweenYellAttackMax));

            soundSrc.clip = scoldSounds[Random.Range(0, 2)]; //2 is exclusive, so 0 or 1
            soundSrc.Play();

            float fearToApply = baseFearValue * type.fearMultiplier;
            Debug.Log("Applying raw fear: " + fearToApply);
            ApplyFearToAllStudentsInArea(fearToApply);

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

                thrownChalk.transform.rotation = GetProjectileRotation(transform.position, target.position);
                chalkRb.AddForce((target.position - transform.position) * chalkSpeed, ForceMode2D.Impulse);
                animator.SetTrigger("Subs_ThrowChalk");
                Destroy(thrownChalk.gameObject, 5f);

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

                thrownPrism = Instantiate(prismPrefab, transform.position, Quaternion.identity);
                Rigidbody2D prismRb = thrownPrism.GetComponent<Rigidbody2D>();
                Transform target = StudentManager.instance.GetRandomStudent();
                if(target == null) 
                { 
                    //idk, do something to stop the dude from attacking
                }

                thrownPrism.transform.rotation = GetProjectileRotation(transform.position, target.position);
                prismRb.AddForce((target.position - transform.position) * prismSpeed, ForceMode2D.Impulse);
                Destroy(thrownPrism.gameObject, 3f);

                soundSrc.clip = throwChalkSound;
                soundSrc.Play();


                //ApplyFearToAllStudentsInArea(baseFearValue * type.fearMultiplier * prismFearMultiplier);
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(scienceSecondsBetweenFPaperAttackMin, scienceSecondsBetweenFPaperAttackMax));

                animator.SetTrigger("Maths_ThrowFPaper");

                thrownFPaper = Instantiate(fPaperPrefab, transform.position, Quaternion.identity);
                Rigidbody2D paperRb = thrownFPaper.GetComponent<Rigidbody2D>();
                Transform target = StudentManager.instance.GetRandomStudent();
                if (target == null)
                {
                    yield return null;
                }

                thrownFPaper.transform.rotation = GetProjectileRotation(transform.position, target.position);
                paperRb.AddForce((target.position - transform.position) * fPaperSpeed, ForceMode2D.Impulse);
                Destroy(thrownFPaper.gameObject, 3f);

                soundSrc.clip = throwChalkSound;
                soundSrc.Play();

                //ApplyFearToAllStudentsInArea(baseFearValue * type.fearMultiplier * fPaperFearMultiplier);
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

    public void LosePatiance(float patianceToLose)
    {
        if (PatianceLevel <= 0)
            return;

        animator.SetBool("IsAngry", true);
        float toSubtract = PatianceLevel - patianceToLose * type.patianceMultiplier;
        Debug.Log("Teacher, reduced patiance to " + toSubtract);
        
        PatianceLevel = toSubtract;

        animator.SetBool("IsAngry", false);
    }

    public void LeaveClass()
    {
        // AI to door, corroutine, after that, disapear (animation?)

        //HIS IS USED FOR LOADING/UNLOADING SCENES
        SceneHandler.instance.RemoveTeacher(initialPoition);

        Debug.Log("Teacher, leaving classroom");

        soundSrc.clip = giveupSound;
        soundSrc.Play();

        animator.SetBool("IsItchy", false);
        animator.StopPlayback();
        animator.SetBool("LostPatiance", true);
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Student"))
        {
            studentsInsideArea.Add(collision.gameObject.GetComponent<StudentNPC>());
            return;
        }

        /*StinkBomb collBomb = collision.GetComponent<StinkBomb>();
        if (collBomb != null) 
            collBomb.MaybeExplode();*/
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Student"))
        {
            StudentNPC npc = collision.gameObject.GetComponent<StudentNPC>();
            if (studentsInsideArea.Contains(npc))
                studentsInsideArea.Remove(npc);
        }
    }

    private Quaternion GetProjectileRotation(Vector2 teacherPos, Vector2 studentPos) 
    {
        Vector3 angleOutput = Vector3.zero;

        if(teacherPos.x == studentPos.x) 
            angleOutput.z = (teacherPos.y < studentPos.y) ? 90f : 270f;
        else 
            angleOutput.z = Mathf.Atan2(teacherPos.y - studentPos.y, teacherPos.x - studentPos.x) * Mathf.Rad2Deg;

        angleOutput.z += 90f; //Because of the model lmao
        return Quaternion.Euler(angleOutput);
    }
}
