using UnityEngine;

public class FireBallProjectile : MonoBehaviour
{
    public float damage = 20f;
    public float lifeTime = 3f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir, float speed)
    {
        rb.velocity = dir.normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
        
        if (col.CompareTag("Ground") || col.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}