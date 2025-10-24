using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour //Need base class MonoBehaviour to attach scripts as a component onto an object
{

    protected Rigidbody2D rb;
    protected Animator animator;

    [Header("Attack details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;

    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8.0f; //player related
    protected int facingDir = 1;
    protected bool canMove = true;
    private float xInput;
    private bool isFacingRight = true;
    private bool canJump = true;


    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    private bool isGrounded;
    [SerializeField] private LayerMask whatIsGround; // need visible to assign

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);

        foreach (Collider2D enemy in enemyColliders)
        {
            Entity entityTarget = enemy.GetComponent<Entity>();
            entityTarget.TakeDamage();
        }
    }

    private void TakeDamage()
    {
        //throw new NotImplementedException();
    }

    public void EnableMovementAndJump(bool enable)
    {
        canMove = enable;
        canJump = enable;
    }

    protected void HandleAnimations()
    {
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded); //requires exact same name of parameter
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.UpArrow)))
            TryToJump();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleAttack();
        }
    }

    protected virtual void HandleAttack()
    {
        if (isGrounded)
            animator.SetTrigger("attack");
    }   

    private void TryToJump()
    {
        if (isGrounded && canJump)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    protected virtual void HandleMovement()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); //stops player from moving when moving
    }


    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    protected void HandleFlip()
    {
        if (rb.linearVelocityX > 0 && isFacingRight == false)
            Flip();
        else if (rb.linearVelocityX < 0 && isFacingRight == true)
            Flip();

    }
    //[ContextMenu("Flip")] //can test method in Unity editor
    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
        facingDir *= -1; //set x input to opposite after flipping
    }

    private void OnDrawGizmos() //helps us determine the distance from the transform.position
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

}
