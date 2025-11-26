using UnityEngine;

public class EnemyDummy : Enemy
{
    [Header("Dummy Drop Settings")]
    public GameObject pickupPrefab;
    public Transform pickupSpawnPoint;
    
    protected override void PatrolOrChase()
    {
        if(anim != null) anim.SetBool("isWalking", false);
        if(rb != null) rb.velocity = Vector2.zero;
    }
    
    protected override void TryAttack()
    {
        
    }
    
    void OnDisable()
    {
        if (isDead && pickupPrefab != null)
        {
            SpawnPickup();
        }
    }

    void SpawnPickup()
    {
        Vector3 pos = (pickupSpawnPoint != null) ? pickupSpawnPoint.position : transform.position;
        Instantiate(pickupPrefab, pos, Quaternion.identity);
    }
}