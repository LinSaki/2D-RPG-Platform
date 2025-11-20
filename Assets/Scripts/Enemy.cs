using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Enemy : Entity
{
    private bool playerDetected;

    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;

    [Header("Sound details")]
    public AudioClip enemyAttackSound;

    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }

    protected override void HandleAttack()
    {
        if (playerDetected)
        {
            animator.SetTrigger("attack");
            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(enemyAttackSound);
        }
    }

    protected override void HandleMovement()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(facingDir * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();
        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
    }

    protected override void Die()
    {
        base.Die();
        UI.instance.AddKillCount();
    }
}
