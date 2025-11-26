using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IPooledObject
{
    [Header("Basic Info")]
    public float maxHp = 50f;
    protected float hp;
    public float damage = 10f;
    public float speed = 2f;

    [Header("Combat")]
    public float detectRadius = 6f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1f;
    
    [Header("Loot Settings")]
    [Range(0, 1)] public float dropChance = 0.3f;
    public List<GameObject> lootDrops;

    protected Transform playerTransform;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sr;
    protected float lastAttackTime;
    protected bool isDead = false;

    public static event Action<Enemy> OnEnemyDied;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if(rb != null) rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void OnObjectSpawn()
    {
        isDead = false;
        hp = maxHp;
        
        if(anim != null) { anim.Rebind(); anim.Update(0f); }
        if(rb != null) { rb.velocity = Vector2.zero; rb.bodyType = RigidbodyType2D.Dynamic; }
        GetComponent<Collider2D>().enabled = true;
        if(sr != null) sr.color = Color.white;

        var player = GameObject.FindWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    void Start()
    {
        OnObjectSpawn();
    }

    void Update()
    {
        if (isDead) return;
        if (playerTransform != null) PatrolOrChase();
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
                rb.MovePosition(rb.position + dir * speed * Time.deltaTime);
            }
            else
            {
                anim.SetBool("isWalking", false);
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    anim.SetBool("isAttacking", true);
                    lastAttackTime = Time.time;
                    var p = playerTransform.GetComponent<Player>();
                    if(p != null) Attack(p);
                    Invoke(nameof(EndAttack), 0.5f);
                }
            }
        }
        else anim.SetBool("isWalking", false);
    }
    
    void EndAttack() => anim.SetBool("isAttacking", false);
    public virtual void Attack(Player p) => p.TakeDamage(damage);

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        hp -= amount;
        anim.SetTrigger("isHurt");
        StartCoroutine(FlashRed());
        if (hp <= 0) Die();
    }

    IEnumerator FlashRed()
    {
        if(sr) sr.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        if(sr) sr.color = Color.white;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        
        OnEnemyDied?.Invoke(this);
        
        TryDropLoot();

        anim.SetBool("isDead", true);
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        
        StartCoroutine(DisableAfterDeathAnim());
    }
    
    protected void TryDropLoot()
    {
        if (UnityEngine.Random.value > dropChance) return; // ถ้าดวงไม่ดีก็จบเลย ไม่ดรอป
        if (lootDrops == null || lootDrops.Count == 0) return;
        GameObject itemToDrop = lootDrops[UnityEngine.Random.Range(0, lootDrops.Count)];
        if (itemToDrop != null)
        {
            Instantiate(itemToDrop, transform.position, Quaternion.identity);
        }
    }

    IEnumerator DisableAfterDeathAnim()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}