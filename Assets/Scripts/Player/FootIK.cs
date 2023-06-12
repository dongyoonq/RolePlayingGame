using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FootIK : MonoBehaviour
{
    Animator animator;

    [Range(0, 1f)]
    public float distanceGround;

    private bool isGround;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator && isSlope() && isGround)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);

            RaycastHit hit;
            Ray ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, distanceGround + 1f, LayerMask.GetMask("Enviroment")))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += distanceGround;

                animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPosition);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
            }

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);

            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);

            if (Physics.Raycast(ray, out hit, distanceGround + 1f, LayerMask.GetMask("Enviroment")))
            {
                Vector3 footPosition = hit.point;
                footPosition.y += distanceGround;

                animator.SetIKPosition(AvatarIKGoal.RightFoot, footPosition);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
            }
        }
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
