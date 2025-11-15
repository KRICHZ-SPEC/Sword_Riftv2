using UnityEngine;

public class PickupItem : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Wave1Manager.Instance.OnPickupCollected();
            Destroy(gameObject);
        }
    }
}