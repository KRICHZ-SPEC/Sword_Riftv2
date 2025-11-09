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

    // 🩸 เพิ่มส่วนใหม่
    private Animator anim;
    private SpriteRenderer sr;
    private bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();     // ✅
        sr = GetComponent<SpriteRenderer>(); // ✅
        status = new PlayerStatus(100, 50);
    }

    void Update()
    {
        if (isDead) return; // ❗ถ้าตายแล้วไม่ต้องขยับ
        HandleMovementInput();
        UpdateStatusEffects();
    }

    void HandleMovementInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Vector2 move = new Vector2(h * moveSpeed, rb.velocity.y);
        rb.velocity = move;

        // ✅ อัปเดต Animator
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
        anim.SetTrigger("Attack"); // ✅ trigger โจมตี
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        status.TakeDamage(amount);

        // ✅ flash ตัวแดง + animation
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

        anim.SetBool("isDead", true); // เรียกเล่นแอนิเมชัน
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;

        Debug.Log("Player died");
        Destroy(gameObject, 2f); // ลบตัวละครหลัง animation เล่น
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
}