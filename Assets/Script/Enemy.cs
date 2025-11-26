using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [Header("Basic Info")]
    public float hp = 50f;
    public float damage = 10f;
    public float speed = 2f;
    public float maxHp = 50f;

    [Header("Combat Settings")]
    public float detectRadius = 6f;
    public float attackRange = 1.2f;
    public float attackCooldown = 1f;

    protected Transform playerTransform;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer sr;
    protected float lastAttackTime;
    protected bool isDead = false;

    [Header("Hit Flash")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.15f;
    
    public static event Action<Enemy> OnEnemyDied;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

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

        if (hp <= 0)
            Die();
    }

    IEnumerator HitFlash()
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = Color.white;
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        anim.SetBool("isDead", true);
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        
        OnEnemyDied?.Invoke(this);

        Destroy(gameObject, 2f);
    }
}