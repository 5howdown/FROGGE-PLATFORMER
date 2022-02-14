using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("TU TOUCHE CHTE GOUME")]
    public KeyCode keyDash;
    
    public float moveSpeed;
    public float jumpForce;

    private bool isJumping;
    public bool isGrounded;
    public int JumpCount;
    public int dashForce;
    public int capSpeed;
    public int dashCooldown;
    public bool canDash;
    [HideInInspector]

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask collisionLayers;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D playerCollider;

    private Vector3 velocity = Vector3.zero;
    private float horizontalMovement;
    private float verticalMovement;

    public static PlayerMovement instance;

    private void Start()
    {
        JumpCount = 2;
        canDash = true;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène");
            return;
        }

        instance = this;
    }

    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;

        if (Input.GetButtonDown("Jump") && JumpCount > 0)
        {
            isJumping = true;
            StartCoroutine(WaitingForLiftOff());
        }

        Flip(rb.velocity.x);
        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
        
        if (Input.GetKeyDown(keyDash) && canDash)
        {
            if (spriteRenderer.flipX == false)
            {
                rb.velocity = new Vector2(rb.velocity.x + dashForce , 0);
            }
            
            if (spriteRenderer.flipX == true)
            {
                rb.velocity = new Vector2(rb.velocity.x - dashForce , 0);

            }

            if (rb.velocity.x >= 0)
            {
                rb.velocity = new Vector2(capSpeed, rb.velocity.y);

            }
            
            else
            {
                rb.velocity = new Vector2(-capSpeed, rb.velocity.y);
            }

            canDash = false;
            StartCoroutine(DashCooldown());



        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
        MovePlayer(horizontalMovement, verticalMovement);

        if (isGrounded)
        {
            JumpCount = 2;
        }
    }
    
        void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
        if (true)
        {
            Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

            if (isJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0f, jumpForce));
                isJumping = false;
                animator.SetBool("GroundReception", false);
                animator.SetBool("IsJumping", true);
            }
            else
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("GroundReception", true);
            }
        }
    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }else if(_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    IEnumerator WaitingForLiftOff()
    {
        yield return new WaitForSeconds(0.1f);
        if (JumpCount > 0)
        {
            JumpCount -= 1;
        }
        
    }

    IEnumerator DashCooldown()
    {
        if (!canDash)
        {
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }



}