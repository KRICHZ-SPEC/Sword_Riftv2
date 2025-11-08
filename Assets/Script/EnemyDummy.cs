using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyDummy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 100f;
    private float currentHP;

    [Header("Optional Components")]
    public Animator animator; // ถ้ามี animation
    public GameObject deathEffect; // เอฟเฟกต์เมื่อศัตรูตาย

    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;

        // ตรวจสอบ Animator
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    /// <summary>
    /// ฟังก์ชันให้ศัตรูโดนโจมตี
    /// </summary>
    /// <param name="damage">จำนวนความเสียหาย</param>
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHP -= damage;

        // เล่น Animation โดนโจมตี
        if (animator != null)
            animator.SetTrigger("Hit");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // เล่น Animation ตาย
        if (animator != null)
            animator.SetTrigger("Die");

        // สร้าง Death Effect
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        // ลบ Object หลัง Animation ตาย (ถ้าไม่มี Animator ให้ลบทันที)
        Destroy(gameObject, 0.5f);
    }
}
