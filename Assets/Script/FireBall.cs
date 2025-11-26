using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;
    public float lifeTime = 2f;
    private Vector2 direction;

    public void Setup(Vector2 dir)
    {
        direction = dir.normalized;
        
        Destroy(gameObject, lifeTime);
        
        if (dir.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}