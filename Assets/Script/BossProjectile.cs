using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BossProjectile : MonoBehaviour
{
    private float damage = 10f;
    private Rigidbody2D rb;
    public GameObject hitEffect;
    public float lifeTime = 5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
        int projLayer = LayerMask.NameToLayer("Projectile");
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        if (projLayer >= 0 && enemyLayer >= 0)
            Physics2D.IgnoreLayerCollision(projLayer, enemyLayer, true);
    }

    public void Setup(Vector2 velocity, float dmg)
    {
        damage = dmg;
        rb.velocity = velocity;
        rb.gravityScale = 0f;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player p = collision.GetComponent<Player>();
            if (p != null)
                p.TakeDamage(damage);

            DestroyProjectile();
        }
        
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}