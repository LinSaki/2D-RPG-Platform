using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8.0f; //player related
    private float xInput;
    private bool canJump = true;

    [Header("Sound details")]
    public AudioClip playerAttackSound;

    protected override void Awake()
    {
        base.Awake();
        healthText.text = currentHealth.ToString();
    }

    protected override void Update()
    {
        base.Update();
        HandleKeyboardInput();
    }

    private void HandleKeyboardInput()
    {
#if UNITY_ANDROID || UNITY_IOS
        float keyboardX = Input.GetAxisRaw("Horizontal");
        // Only override mobile input if keyboard is pressed
        if (keyboardX != 0)
            xInput = keyboardX;
#else
        xInput = Input.GetAxisRaw("Horizontal");
#endif


        // Jump (PC)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            TryToJump();

        // Attack (PC)
        if (Input.GetKeyDown(KeyCode.L))
        {
            HandleAttack();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(playerAttackSound);
        }
    }

    // For Mobile UI
    public void PressLeft() { xInput = -1; }
    public void PressRight() { xInput = 1; }
    public void StopMove() { xInput = 0; }

    public void PressJump()
    {
        TryToJump();
    }

    public void PressAttack()
    {
        HandleAttack();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(playerAttackSound);
    }
    protected override void HandleMovement()
    {
        if (canMove)
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); //stops player from moving when moving
    }

    private void TryToJump()
    {
        if (isGrounded && canJump)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public override void EnableMovement(bool enable)
    {
        base.EnableMovement(enable);
        canJump = enable;
    }

    protected override void Die()
    {
        base.Die();
        UI.instance.EnableGameOverUI();
    }

    protected override void TakeDamage()
    {
        base.TakeDamage();
        healthText.text = currentHealth.ToString();
    }
}
