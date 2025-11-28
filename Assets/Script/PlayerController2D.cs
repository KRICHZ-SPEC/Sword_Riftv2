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
    public bool canMove = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;
    private float lastAttackTime = -999f;
    public float attackDamage = 20f;
    public Transform attackPoint;
    public float attackRange = 0.8f;
    public LayerMask enemyLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Player playerStats;

    private bool isGrounded;
    private bool facingRight = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<Player>();

        if (sr == null) Debug.LogError("SpriteRenderer not found on Player");
        if (playerStats == null) Debug.LogError("Player script not found on Player Object");
    }

    void Update()
    {
        if ((playerStats != null && playerStats.status.hp <= 0) || !canMove)
        {
            if (playerStats.status.hp <= 0) rb.velocity = Vector2.zero; 
            return; 
        }

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
    }
    
    void Attack()
    {
        lastAttackTime = Time.time;
        anim.SetTrigger("Attack");
        
        Vector3 origin = attackPoint != null ? attackPoint.position : transform.position;
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(origin, attackRange, enemyLayer);

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
    
    public IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
        
        if (groundCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}