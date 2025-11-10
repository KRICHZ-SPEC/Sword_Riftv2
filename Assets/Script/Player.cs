using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    public string playerName;
    public Vector2 velocity;
    public PlayerStatus status;
    public List<ActiveSkill> skills = new List<ActiveSkill>();
    public List<Item> inventory = new List<Item>();
    public List<StatusEffect> activeEffects = new List<StatusEffect>();

    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    public HealthBar healthBar;
    private Animator anim;
    private SpriteRenderer sr;
    private bool isDead = false;

    void Start()
    {
        if (status.maxHp <= 0) status.maxHp = 100f;
        status.hp = status.maxHp;
        
        if (healthBar != null) healthBar.UpdateBar();
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();     
        sr = GetComponent<SpriteRenderer>();
        status = new PlayerStatus(100, 50);
    }

    void Update()
    {
        if (isDead) return;
        HandleMovementInput();
        UpdateStatusEffects();
    }

    void HandleMovementInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Vector2 move = new Vector2(h * moveSpeed, rb.velocity.y);
        rb.velocity = move;
        
        if (anim != null)
            anim.SetFloat("Speed", Mathf.Abs(h));
    }

    public void Move(Vector2 dir)
    {
        rb.velocity = dir * moveSpeed;
    }

    public void Attack()
    {
        Debug.Log("Player attack");
        anim.SetTrigger("Attack");
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        status.TakeDamage(amount);

        if (healthBar != null)
            healthBar.UpdateBar();
        StartCoroutine(FlashRed());
        anim.SetTrigger("Hurt");

        Debug.Log($"Player hurt! HP = {status.hp}");

        if (status.hp <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        sr.color = Color.white;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        GetComponent<Animator>().SetBool("isDead", true);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2f);
    }
    
    public void PickupItem(Item item)
    {
        inventory.Add(item);
        item.OnPickup(this);
    }

    public void ApplyStatus(StatusEffect effect)
    {
        effect.Start();
        activeEffects.Add(effect);
    }

    void UpdateStatusEffects()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].ApplyTo(status);
            activeEffects[i].Update(Time.deltaTime);
            if (activeEffects[i].IsExpired())
            {
                activeEffects.RemoveAt(i);
            }
        }
    }
    public void AddHp(float amount)
    {
        if (isDead) return;

        status.Heal(amount);
        
        if (healthBar != null)
            healthBar.UpdateBar();
    }
}