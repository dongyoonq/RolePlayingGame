using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    [SerializeField] bool debug;

    public float range;
    [Range(0, 360)] public float angle;
    public Equipment sword;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnAttack(InputValue value)
    {
        Attack();
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void AttackTiming()
    {
        int dmg = sword.GetComponent<Equipment>().data.ad;

        // 범위내에 있는지 체크
        Collider[] coller = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Monster"));
        foreach (Collider collider in coller)
        {
            // 앞에 있는지
            Vector3 dirTarget = (collider.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, dirTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
                continue;

            collider.GetComponent<IHitable>().Hit(dmg);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);
        Debug.DrawRay(transform.position, rightDir * range, Color.yellow);
        Debug.DrawRay(transform.position, leftDir * range, Color.yellow);
    }

    private Vector3 AngleToDir(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
    }
}
