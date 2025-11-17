using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;
    private float lastAttackTime = -999f;
    public float attackDamage = 20f;

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

        if (sr == null)
            Debug.LogError("SpriteRenderer not found on Player — add one or require it in the inspector.");
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);

        if (move > 0 && !facingRight) Flip();
        else if (move < 0 && facingRight) Flip();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        }

        if (Input.GetKeyDown(KeyCode.J) && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
        }

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

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
    
    void Attack()
    {
        lastAttackTime = Time.time;
        anim.SetTrigger("Attack");
        Debug.Log("Player Attacked!");

        float attackRadius = 0.8f;
        LayerMask enemyLayer = LayerMask.GetMask("Enemy"); 
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        foreach (Collider2D hit in hitColliders)
        {
            Enemy e = hit.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(attackDamage); 
                Debug.Log("Hit Enemy: " + e.name);
                continue;
            }
            
            EnemyDummy dummy = hit.GetComponent<EnemyDummy>();
            if (dummy != null)
            {
                dummy.TakeDamage(attackDamage); 
                Debug.Log("Hit Dummy: " + dummy.name);
            }
        }
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.8f);
    }
}