using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 1.2f;   // ระยะโจมตี
    public float attackDamage = 20f;   // ความเสียหาย
    public float attackCooldown = 0.5f;
    public Transform attackPoint;      // จุดตรวจ hit
    public LayerMask enemyLayer;       // เลเยอร์ของศัตรู

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void Attack()
    {
        // เล่น animation (ถ้ามี)
        Debug.Log("Player โจมตี!");

        // ตรวจจับศัตรูในระยะ
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>()?.TakeDamage(attackDamage);
        }
    }

    // Gizmo แสดงใน Scene view
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
}
