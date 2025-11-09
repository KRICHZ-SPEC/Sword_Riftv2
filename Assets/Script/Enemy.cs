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

    [Header("Combat Settings")]
    public float detectRadius = 6f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1f;

    [Header("Drop Settings")]
    public List<Item> dropTable = new List<Item>();

    protected Transform playerTransform;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected float lastAttackTime;
    protected bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        var player = GameObject.FindWithTag("Player");
        if (player != null)
            playerTransform = player.transform;
    }

    void Update()
    {
        if (isDead) return; // หยุดทำงานถ้าตายแล้ว

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
                Vector2 dir = (playerTransform.position - transform.position).normalized;
                Vector2 newPos = rb.position + dir * speed * Time.deltaTime;
                rb.MovePosition(newPos);
            }
            else
            {
                TryAttack();
            }
        }
    }

    protected virtual void TryAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            var player = playerTransform.GetComponent<Player>();
            if (player != null)
                Attack(player);

            lastAttackTime = Time.time;
        }
    }

    public virtual void Attack(Player player)
    {
        player.TakeDamage(damage);
    }

    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        hp -= amount;
        anim.SetTrigger("isHurt"); // เล่นอนิเมชัน hurt

        if (hp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        anim.SetBool("isDead", true); // เล่นอนิเมชันตาย

        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        DropItem();

        // รอให้อนิเมชันตายเล่นจบก่อน Destroy
        Destroy(gameObject, 1.5f); // ปรับเวลาให้พอดีกับความยาวอนิเมชัน
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
}