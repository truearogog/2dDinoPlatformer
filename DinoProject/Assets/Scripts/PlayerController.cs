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

    public LayerMask groundMask;
    public Collider2D grounder;
    public bool isGrounded;
    private float prevJumpTime;

    private const float jumpSpeed = 5f;
    private const float defaultWalkSpeed = 5f;
    private float walkSpeed = defaultWalkSpeed;
    private const float runSpeed = defaultWalkSpeed * 2f;
    public bool isRunning = false;

    private float prevKickTime;
    private const float kickTime = 0.5f;
    public LayerMask kickMask;
    public float kickDistance;

    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;
    private MaterialAnimate ma;

    public float defaultStepTime;
    public float runStepTime;
    private float stepTime;
    private float prevStepTime;
    public AudioClip[] stepSounds = new AudioClip[2];

    void Start()
    {
        moveLeftKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), moveLeftKey);
        moveRightKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), moveRightKey);
        kickKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), kickKey);
        runKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), runKey);
        jumpKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), jumpKey);

        stepTime = defaultStepTime;
        prevJumpTime = Time.time;
        prevKickTime = Time.time;
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        ma = GetComponent<MaterialAnimate>();

        ma.ChangeFloat("_Scale", 0, 0.02f);
    }

    void Flip()
    {
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void FixedUpdate()
    {
        //Ground detection
        if (grounder.IsTouchingLayers(groundMask))
            isGrounded = true;
        else
            isGrounded = false;

        //Input managing
        int mLeft = Input.GetKey(moveLeftKeyCode) ? 1 : 0;
        int mRight = Input.GetKey(moveRightKeyCode) ? 1 : 0;
        int mlr = mRight - mLeft;

        if (mlr == 0)
        {
            if (Mathf.Abs(moveInput) < inputDead)
                moveInput = 0;
            else
                moveInput = Mathf.Lerp(moveInput, 0, gravity);
        }
        else
        {
            if ((Time.time - prevStepTime >= stepTime) && isGrounded && (Mathf.Abs(rb.velocity.x) > 0))
            {
                SoundManager.PlaySound(stepSounds[Random.Range(0, stepSounds.Length)]);
                prevStepTime = Time.time;
            }
            moveInput = Mathf.Lerp(moveInput, mlr, speed);
        }

        //Check if running
        if (mlr != 0)
        {
            if (Input.GetKey(runKeyCode) && mlr != 0)
            {
                if (!isRunning)
                {
                    walkSpeed = runSpeed;
                    stepTime = runStepTime;
                    isRunning = true;
                }
            }
            else
            {
                if (isRunning)
                {
                    walkSpeed = defaultWalkSpeed;
                    stepTime = defaultStepTime;
                    isRunning = false;
                }
            }
        }

        //Check for kick
        if (Input.GetKey(kickKeyCode) && (Time.time - prevKickTime >= kickTime))
        {
            animator.SetTrigger("isKicking");
            prevKickTime = Time.time;

            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position - new Vector3(0, col.bounds.size.y / 4, 0), Vector3.right * Mathf.Sign(transform.localScale.x), kickDistance, kickMask);
            if (hits.Length != 0)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    hit.collider.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.right * Mathf.Sign(transform.localScale.x) * 3f + Vector3.up * 5f;
                    hit.collider.gameObject.GetComponent<SpriteFlash>().Flash();
                    hit.collider.gameObject.GetComponent<HittableManager>().GetHit(1);
                }
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
            if (Time.time - prevJumpTime >= 0.2f)
            {
                SoundManager.PlaySound("jump");
                prevJumpTime = Time.time;
            }
        }
    }
}
