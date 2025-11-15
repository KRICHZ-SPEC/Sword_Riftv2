using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float damage = 20f;
    public float speed = 10f;
    public float range = 5f;

    private Vector3 startPos;
    private Vector2 direction;

    void Start()
    {
        startPos = transform.position;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if (Vector3.Distance(startPos, transform.position) >= range)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}