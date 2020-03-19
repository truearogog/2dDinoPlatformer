using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public string kickKey;
    private KeyCode kickKeyCode;

    public string moveLeftKey;
    private KeyCode moveLeftKeyCode;

    public string moveRightKey;
    private KeyCode moveRightKeyCode;

    public string runKey;
    private KeyCode runKeyCode;

    public string jumpKey;
    private KeyCode jumpKeyCode;

    public float moveInput = 0;
    public float gravity;
    public float speed;
    public float inputDead;

    public bool isGrounded;
    public Transform grounder;
    private float checkRadius;
    public LayerMask groundMask;

    private const float jumpSpeed = 5f;
    private const float defaultWalkSpeed = 5f;
    private float walkSpeed = defaultWalkSpeed;
    private const float runSpeed = defaultWalkSpeed * 2f;

    private Rigidbody2D rb;
    private Animator animator;

    public bool isRunning = false;

    void Start()
    {
        moveLeftKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), moveLeftKey);
        moveRightKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), moveRightKey);
        kickKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), kickKey);
        runKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), runKey);
        jumpKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), jumpKey);

        checkRadius = GetComponent<BoxCollider2D>().size.x / 2;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Flip()
    {
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void FixedUpdate()
    {
        //Check if dino is grounded
        isGrounded = Physics2D.OverlapCircle(grounder.position, checkRadius, groundMask);

        //Input managing
        int mLeft = Input.GetKey(moveLeftKeyCode) ? 1 : 0;
        int mRight = Input.GetKey(moveRightKeyCode) ? 1 : 0;
        int mlr = mRight - mLeft;

        if (mlr == 0)
        {
            {
                if (Mathf.Abs(moveInput) < inputDead)
                    moveInput = 0;
                else
                    moveInput = Mathf.Lerp(moveInput, 0, gravity);
            }
        }
        else
        {
            moveInput = Mathf.Lerp(moveInput, mlr, speed);
        }

        //Check if running
        if (mlr != 0)
        {
            if (Input.GetKey(runKeyCode) && mlr != 0)
            {
                walkSpeed = runSpeed;
                if (!isRunning)
                    isRunning = true;
            }
            else
            {
                walkSpeed = defaultWalkSpeed;
                if (isRunning)
                    isRunning = false;
            }
        }

        animator.SetBool("isRunning", isRunning && (Mathf.Abs(rb.velocity.x) > defaultWalkSpeed));

        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        rb.velocity = new Vector2(moveInput * walkSpeed, rb.velocity.y);

        //Flip dino if it changes walking side
        if ((transform.localScale.x < 0 && moveInput > 0) || (transform.localScale.x > 0 && moveInput < 0))
        {
            Flip();
        }

        //Check for jump
        if (isGrounded && Input.GetKey(jumpKeyCode))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
}
