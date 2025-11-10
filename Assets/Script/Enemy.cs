using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [Header("Basic Info")]
    public int enemyID;
    public string type;
    public float hp = 50f;
    public float damage = 10f;
    public float speed = 2f;
    public float maxHp = 50f;

    [Header("Combat Settings")]
    public float detectRadius = 6f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1f;

    [Header("Drop Settings")]
    public List<Item> dropTable = new List<Item>();

    protected Transform playerTransform;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sr;
    protected float lastAttackTime;
    //[Header("Health Bar")]
    //public EnemyHealthBar healthBar;
    protected bool isDead = false;

    [Header("Hit Flash")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.15f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        var player = GameObject.FindWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
    }

    void Update()
    {
        if (isDead) return;

        if (playerTransform != null)
            PatrolOrChase();
    }

    protected virtual void PatrolOrChase()
    {
        float dist = Vector2.Distance(transform.position, playerTransform.position);

        if (dist <= detectRadius)
        {
            if (dist > attackRange)
            {
                anim.SetBool("isWalking", true);
                Vector2 dir = (playerTransform.position - transform.position).normalized;
                Vector2 newPos = rb.position + dir * speed * Time.deltaTime;
                rb.MovePosition(newPos);
            }
            else
            {
                anim.SetBool("isWalking", false);
                TryAttack();
            }
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    protected virtual void TryAttack()
    {
        
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            anim.SetBool("isAttacking", true);
            var player = playerTransform.GetComponent<Player>();
            if (player != null)
                Attack(player);

            lastAttackTime = Time.time;
            
            Invoke(nameof(EndAttack), 0.5f);
        }
    }
    void EndAttack()
    {
        anim.SetBool("isAttacking", false);
    }

    public virtual void Attack(Player player)
    {
        player.TakeDamage(damage);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        hp -= amount;
        if (hp < 0) hp = 0;

        anim.SetTrigger("isHurt");
        StartCoroutine(HitFlash());

        // ✅ อัปเดต HealthBar
        //if (healthBar != null)
           // healthBar.UpdateBar();
        //else
            //Debug.LogWarning("HealthBar not assigned!");

        if (hp <= 0)
            Die();
    }

    IEnumerator HitFlash()
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = Color.white;
    }

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        anim.SetBool("isDead", true);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        DropItem();
        Destroy(gameObject, 1.5f);
    }

    protected virtual void DropItem()
    {
        if (dropTable.Count > 0)
        {
            var itemPrefab = dropTable[Random.Range(0, dropTable.Count)];
            if (itemPrefab != null)
                Instantiate(itemPrefab.gameObject, transform.position, Quaternion.identity);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController2D player = other.GetComponent<PlayerController2D>();
            if (player != null)
            {
                player.TakeDamage(30f);
            }
        }
    }
}