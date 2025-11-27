using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float damage = 15f;
    public float lifeTime = 3f;
    public GameObject hitEffect;

    void Start()
    {
        Destroy(gameObject, lifeTime);
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