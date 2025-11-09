using UnityEngine;

public enum ItemType { HP, MP, Buff, Key }

public class Item : MonoBehaviour 
{
    public int itemID;
    public string itemName;
    public ItemType type;
    public float value;
    public int rarity; // 0 common, 1 rare etc.

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                OnPickup(player);
            }
        }
        Debug.Log($"Trigger enter: {other.name}");
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log("Picked up item!");
                OnPickup(player);
            }
        }
    }

    public void Use(Player player) 
    {
        switch (type) 
        {
            case ItemType.HP:
                player.status.Heal(value);
                break;
            case ItemType.MP:
                player.status.mp += value;
                if (player.status.mp > player.status.maxMp) player.status.mp = player.status.maxMp;
                break;
            case ItemType.Buff:
                StatusEffect se = new StatusEffect 
                {
                    effectID = 1,
                    effectName = "Buff",
                    duration = 5f,
                    effectValue = value,
                    type = StatusType.Buff
                };
                player.ApplyStatus(se);
                break;
            case ItemType.Key:
                // unlock something
                break;
        }
        Destroy(gameObject);
    }

    public void OnPickup(Player player) 
    {
        Debug.Log("Using item: " + itemName);
        if (type == ItemType.HP || type == ItemType.MP) 
        {
            Use(player);
        } 
        else 
        {
            // add to inventory later
            Debug.Log($"{itemName} added to inventory.");
        }
        Destroy(gameObject);
    }
}