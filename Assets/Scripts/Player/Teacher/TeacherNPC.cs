using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherNPC : MonoBehaviour
{
    public TeacherData type;
    public Animator animator;

    [Header("Hall Teacher")]
    public float secondsBetweenAttackMin;
    public float secondsBetweenAttackMax;

    [Header("Subs Teacher")]
    public Transform chalkPrefab;
    private Transform thrownChalk;
    public float chalkSpeed;

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

    private void Awake()
    {
        // Attack loop
        ExecuteAttackLoop();
    }

    public void ExecuteAttackLoop()
    {
        switch (type.variation)
        {
            case TeacherVariation.HallMonitor:
                AttackLoopHall();
                break;

            case TeacherVariation.Sub:
                //SpecialAttack_Subs_Chalk();
                break;

            case TeacherVariation.Science:
                //MathsAttack_FPaper();
                break;

            default:
                break;
        }
    }


    private IEnumerator AttackLoopHall()
    {
        yield return new WaitUntil(() => studentsInsideArea.Count > 1);
        yield return new WaitForSeconds(Random.Range(secondsBetweenAttackMin, secondsBetweenAttackMax));
        RegularAttack_HallMonitor();
        StartCoroutine(AttackLoopHall());
    }

    private void RegularAttack_HallMonitor()
    {
        /*thrownChalk = Instantiate(chalkPrefab, transform.position, Quaternion.identity);
        Rigidbody2D chalkRb = thrownChalk.GetComponent<Rigidbody2D>();
        Transform target = StudentManager.instance.GetRandomStudent();
        chalkRb.AddForce((target.position - transform.position) * chalkSpeed, ForceMode2D.Impulse);
        animator.SetTrigger("Subs_ThrowChalk");
        Destroy(thrownChalk.gameObject, 3f);*/

        animator.SetBool("Scold", true);

        foreach (StudentNPC student in studentsInsideArea)
        {
            student.ApplyFear(10f);
        }
    }

    private void SpecialAttack_Subs_Chalk()
    {
        thrownChalk = Instantiate(chalkPrefab, transform.position, Quaternion.identity);
        Rigidbody2D chalkRb = thrownChalk.GetComponent<Rigidbody2D>();
        Transform target = StudentManager.instance.GetRandomStudent();
        chalkRb.AddForce((target.position - transform.position) * chalkSpeed, ForceMode2D.Impulse);
        animator.SetTrigger("Subs_ThrowChalk");
        Destroy(thrownChalk.gameObject, 3f);
    }

    private void MathsAttack_FPaper()
    {
        animator.SetTrigger("Maths_ThrowFPaper");
    }

    private void MathsAttack_PrismBeam()
    {
        animator.SetTrigger("Maths_PrismBeam");
    }

    public void LeaveClass()
    {
        // AI to door, corroutine, after that, disapear (animation?)
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
