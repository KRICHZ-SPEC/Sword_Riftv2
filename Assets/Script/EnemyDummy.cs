using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyDummy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 100f;
    private float currentHP;

    [Header("Optional Components")]
    public Animator animator;
    public GameObject deathEffect;

    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        
        if (animator == null)
            animator = GetComponent<Animator>();
    }
    
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHP -= damage;
        
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
        
        if (animator != null)
            animator.SetTrigger("Die");
        
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        
        Destroy(gameObject, 0.5f);
    }
}
