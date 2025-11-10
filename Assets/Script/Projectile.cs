using UnityEngine;

public class Projectile : MonoBehaviour 
{
    private float damage;
    private float speed;
    private string ownerTag;

    public float lifeTime = 5f;

    public void Initialize(float damage, float speed, string ownerTag) 
    {
        this.damage = damage;
        this.speed = speed;
        this.ownerTag = ownerTag;
        Destroy(gameObject, lifeTime);
    }

    void Update() 
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == ownerTag) return;
        var enemy = other.GetComponent<Enemy>();
        if (enemy != null) 
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}