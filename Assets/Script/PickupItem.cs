using UnityEngine;

public class PickupItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Pickup touched Player");
            
            if (WaveManager.Instance != null)
            {
                WaveManager.Instance.OnPickupCollected();
            }
            
            Item item = GetComponent<Item>();
            if (item != null)
            {
                item.OnPickup(col.GetComponent<Player>());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}