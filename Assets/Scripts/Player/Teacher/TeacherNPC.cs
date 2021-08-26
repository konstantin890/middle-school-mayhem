using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherNPC : MonoBehaviour
{
    public TeacherData type;
    public Animator animator;

    [Header("Subs Teacher")]
    public Transform chalkPrefab;
    private Transform thrownChalk;
    public float chalkSpeed;

    [Range(0, 1)] public float patianceLevel;

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

    public void ExecuteAttack()
    {
        switch (type.variation)
        {
            case TeacherVariation.Sub:
                SubAttack_Chalk();
                break;

            case TeacherVariation.Math:
                MathsAttack_FPaper();
                break;

            default:
                break;
        }
    }

    private void SubAttack_Chalk()
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
            ExecuteAttack();
        }
    }
}
