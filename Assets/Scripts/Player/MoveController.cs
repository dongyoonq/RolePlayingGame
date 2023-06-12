using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour
{
    Vector3 moveDir;
    CharacterController controller;
    Animator animator;

    public float moveSpeed;
    public float runSpeed;
    public float jumpForce;

    private float ySpeed;
    private float lastSpeed;

    private bool OnRunKey;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Awake()
    {
        OnRunKey = false;
        lastSpeed = moveSpeed;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fall();
    }

    void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
        moveDir.z = value.Get<Vector2>().y;
    }

    void Move()
    {
        if (moveDir.magnitude == 0)
        {
            animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
            return;
        }

        Vector3 forwardVec = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
        Vector3 rightVec = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z).normalized;
        Vector3 moveVec = (forwardVec * moveDir.z + rightVec * moveDir.x) * lastSpeed * Time.deltaTime;
        controller.Move(moveVec);

        Quaternion lookRotation = Quaternion.LookRotation(forwardVec * moveDir.z + rightVec * moveDir.x);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.2f);

        float percent = ((OnRunKey) ? 1 : 0.5f) * moveDir.magnitude;
        animator.SetFloat("Speed", percent, 0.1f, Time.deltaTime);
    }
        
    void OnJump(InputValue value)
    {
        Jump();
    }

    void Jump()
    {
        ySpeed = jumpForce;
    }

    void Fall()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (controller.isGrounded && ySpeed < 0)
            ySpeed = 0;

        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void OnRun(InputValue value)
    {
        if (value.isPressed)
        {
            lastSpeed = runSpeed;
            OnRunKey = true;
        }
        else
        {
            lastSpeed = moveSpeed;
            OnRunKey = false;
        }
    }
}
