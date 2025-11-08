using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private bool isGrounded;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // --- Movement ---
        float move = Input.GetAxisRaw("Horizontal"); // A,D หรือ ลูกศรซ้ายขวา
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        // --- Flip ---
        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        // --- Jump ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }

        // --- Animation ---
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("isGrounded", isGrounded);
        Debug.Log("isGrounded: " + isGrounded);
    }

    void FixedUpdate()
    {
        // --- Ground Check ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        // ยิง Ray ลงจากเท้า 3 จุด (ซ้าย กลาง ขวา)
        float rayLength = 0.3f;
        Vector2 leftFoot = groundCheck.position + new Vector3(-0.15f, 0f);
        Vector2 midFoot = groundCheck.position;
        Vector2 rightFoot = groundCheck.position + new Vector3(0.15f, 0f);

        bool hitLeft = Physics2D.Raycast(leftFoot, Vector2.down, rayLength, groundLayer);
        bool hitMid = Physics2D.Raycast(midFoot, Vector2.down, rayLength, groundLayer);
        bool hitRight = Physics2D.Raycast(rightFoot, Vector2.down, rayLength, groundLayer);

        isGrounded = hitLeft || hitMid || hitRight;

        Debug.DrawRay(leftFoot, Vector2.down * rayLength, Color.red);
        Debug.DrawRay(midFoot, Vector2.down * rayLength, Color.green);
        Debug.DrawRay(rightFoot, Vector2.down * rayLength, Color.blue);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}