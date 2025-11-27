using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float damage = 15f;
    public float lifeTime = 3f;
    public GameObject hitEffect;
    
    private Rigidbody2D rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        Destroy(gameObject, lifeTime);
    }
    
    public void Setup(Vector2 velocity)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity; 
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player"))
        {
            Player player = hitInfo.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            DestroyProjectile();
        }
        else if (hitInfo.CompareTag("Ground"))
        {
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}