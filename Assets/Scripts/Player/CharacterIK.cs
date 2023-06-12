using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIK : MonoBehaviour
{
    Animator animator;

    public Vector3 footIk_offset;

    private bool isGround;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (isSlope() && isGround)
        {
            Vector3 L_foot = animator.GetBoneTransform(HumanBodyBones.LeftFoot).position;
            Vector3 R_foot = animator.GetBoneTransform(HumanBodyBones.RightFoot).position;

            L_foot = GetHintPoint(L_foot + Vector3.up, L_foot - Vector3.up * 5) + footIk_offset;
            R_foot = GetHintPoint(R_foot + Vector3.up, R_foot - Vector3.up * 5) + footIk_offset;

            //transform.localPosition = new Vector3(0, -Mathf.Abs(L_foot.y - R_foot.y) / 2, 0);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, L_foot);

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, R_foot);
        }

    }

    private Vector3 GetHintPoint(Vector3 start, Vector3 end)
    {
        RaycastHit hit;
        if (Physics.Linecast(start, end, out hit))
        {
            return hit.point;
        }

        return end;
    }

    private bool isSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * 1f, Vector3.down, out hit, 2f, LayerMask.GetMask("Enviroment")))
        {
            float angles = Vector3.Angle(hit.normal, Vector3.up);

            return angles != 0;
        }

        return false;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position + Vector3.up * 1f, 0.5f, Vector3.down, out hit, 0.6f, LayerMask.GetMask("Enviroment")))
            isGround = true;
        else
            isGround = false;
    }
}
