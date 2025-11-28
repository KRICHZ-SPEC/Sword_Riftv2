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
    }
    
    public void Setup(Vector2 velocity, float dmg, Collider2D ownerCollider)
    {
        damage = dmg;
        rb.velocity = velocity;
        rb.gravityScale = 0f;
        
        Collider2D[] ownerColliders = ownerCollider.GetComponentsInParent<Collider2D>();
        Collider2D myCollider = GetComponent<Collider2D>();

        foreach (var col in ownerColliders)
        {
            if (col != null && myCollider != null)
                Physics2D.IgnoreCollision(myCollider, col);
        }

        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss")) 
        {
            return; 
        }

        if (collision.CompareTag("Player"))
        {
            Player p = collision.GetComponent<Player>();
            if (p != null)
                p.TakeDamage(damage);

            DestroyProjectile();
        }
        else 
        {
            int groundLayer = LayerMask.NameToLayer("Ground");
            if (groundLayer != -1 && collision.gameObject.layer == groundLayer)
            {
                DestroyProjectile();
            }
        }
    }

    void DestroyProjectile()
    {
        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}