using UnityEngine;

public class Player : MonoBehaviour //Need base class MonoBehaviour to attach scripts as a component onto an object
{

    private Rigidbody2D rb;
    private float xInput;
    [SerializeField]private float moveSpeed = 3.5f;

    private string playerName = "Chai";
    private int currentHp = 100;

    private void Awake()
    {
        GetPlayerInfo();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }

    private void GetPlayerInfo()
    {
        Debug.Log("Player name is: " + playerName);
    }
}
