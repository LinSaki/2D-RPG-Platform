using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class Entity : MonoBehaviour //Need base class MonoBehaviour to attach scripts as a component onto an object
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Collider2D col;
    protected SpriteRenderer sr;

    [Header("Health")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] protected int currentHealth;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration = .1f;
    [SerializeField] protected TextMeshPro healthText;
    private Coroutine damageFeedbackCoroutine;

    [Header("Attack details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;


    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    protected bool isGrounded;
    [SerializeField] private LayerMask whatIsGround; // need visible to assign

    //Facing Directionn Details
    protected int facingDir = 1;
    protected bool canMove = true;
    protected bool isFacingRight = true;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        col = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        HandleCollision();
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

    protected virtual void TakeDamage()
    {
        currentHealth -= 1;
        PlayDamageFeedback();

        if (currentHealth == 0)
            Die();
    }

    protected virtual void Die()
    {
        animator.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12; //make character bounce lighter when dying
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        Destroy(gameObject, 3); //delay of 3 seconds after obj falls
    }

    private void PlayDamageFeedback()
    {
        if (damageFeedbackCoroutine != null)
            StopCoroutine(damageFeedbackCoroutine);

        StartCoroutine(DamageFeedbackCo());
    }

    private IEnumerator DamageFeedbackCo()
    {
        Material originalMat = sr.material;

        sr.material = damageMaterial;

        yield return new WaitForSeconds(damageFeedbackDuration);

        sr.material = originalMat;
    }

    public virtual void EnableMovement(bool enable)
    {
        canMove = enable;
    }

    protected void HandleAnimations()
    {
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded); //requires exact same name of parameter
    }

    protected virtual void HandleAttack()
    {
        if (isGrounded)
            animator.SetTrigger("attack");
    }   

    protected virtual void HandleMovement()
    {
    }

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    protected virtual void HandleFlip()
    {
        if (rb.linearVelocityX > 0 && isFacingRight == false)
            Flip();
        else if (rb.linearVelocityX < 0 && isFacingRight == true)
            Flip();

    }
    //[ContextMenu("Flip")] //can test method in Unity editor
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
        facingDir *= -1; //set x input to opposite after flipping
    }

    private void OnDrawGizmos() //helps us determine the distance from the transform.position
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        
        if(attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

}
