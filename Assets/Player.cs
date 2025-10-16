using UnityEngine;

public class Player : MonoBehaviour //Need base class MonoBehaviour to attach scripts as a component onto an object
{

    private Rigidbody2D rb;
    private float xInput;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8.0f;

    private string playerName = "Chai";
    private int currentHp = 100;

    private void Awake()
    {
        GetPlayerInfo();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
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
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

}
