using UnityEngine;

public class Player : MonoBehaviour //Need base class MonoBehaviour to attach scripts as a component onto an object
{

    private Rigidbody2D rb;
    private Animator animator;

    [Header("Movement details")]
    private float xInput;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8.0f;
    private bool isFacingRight = true;

    private string playerName = "Chai";
    private int currentHp = 100;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    private bool isGrounded;
    [SerializeField] private LayerMask whatIsGround; // need visible to assign

    private void Awake()
    {
        GetPlayerInfo();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    private void HandleAnimations()
    {
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetBool("isGrounded", isGrounded); //requires exact same name of parameter
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.UpArrow)))
            Jump();
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }

    private void GetPlayerInfo()
    {
        Debug.Log("Player name is: " + playerName);
    }

    private void Jump()
    {
        if (isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void HandleFlip()
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
    }

    private void OnDrawGizmos() //helps us determine the distance from the transform.position
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
    }

}
