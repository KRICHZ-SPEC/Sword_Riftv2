using System.Collections;      // <<-- จำเป็นสำหรับ IEnumerator
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

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private bool isGrounded;
    private bool facingRight = true;
    private bool isDead = false;

    [Header("Status")]
    public PlayerStatus status = new PlayerStatus();

    // Coroutine reference
    private Coroutine flashCoroutine;

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
        if (isDead) return;

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

        // ✅ ตรวจศัตรูในระยะโจมตี (เช่น 0.8 หน่วย)
        float attackRadius = 0.8f;
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy e = enemy.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(status.attack); // ใช้ค่าดาเมจของผู้เล่น
                Debug.Log("Hit Enemy: " + e.name);
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

    // ---------------- Damage & Flash ----------------
    private IEnumerator FlashRedMultiple(int times, float interval)
    {
        // ป้องกัน sr == null
        if (sr == null) yield break;

        for (int i = 0; i < times; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(interval);
            sr.color = Color.white;
            yield return new WaitForSeconds(interval);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        status.TakeDamage(amount);

        if (status.hp > 0)
        {
            anim.SetTrigger("Hurt");
            Debug.Log($"Player Hurt! HP: {status.hp}");

            // หยุด coroutine เก่าแล้วเริ่มใหม่
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                flashCoroutine = null;
            }
            flashCoroutine = StartCoroutine(FlashRedMultiple(3, 0.08f));
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetTrigger("Die");
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        Debug.Log("Player Died!");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.8f);
    }
    // ------------------------------------------------
}