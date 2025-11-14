using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Wave1Manager wave;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            wave.OnItemPicked();
            Destroy(gameObject);
        }
    }
}