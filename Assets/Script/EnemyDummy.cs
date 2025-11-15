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
        maxHp -= dmg;
        if (maxHp <= 0)
        {
            Destroy(gameObject);
        }
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