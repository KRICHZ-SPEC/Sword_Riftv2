using UnityEngine;

public class EnemyDummy : MonoBehaviour
{
    public float maxHp = 30f;
    public float currentHp;

    public GameObject pickupPrefab;
    public Transform pickupSpawnPoint;

    void Awake()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;

        if (currentHp <= 0)
            Die();
    }

    void Die()
    {
        SpawnPickup();
        Destroy(gameObject);
    }

    void SpawnPickup()
    {
        if (pickupPrefab == null) return;

        Vector3 pos = pickupSpawnPoint ? pickupSpawnPoint.position : transform.position;
        Instantiate(pickupPrefab, pos, Quaternion.identity);
    }
}