using UnityEngine;

public enum ItemType { HP, MP, Buff, Key }

public class Item : MonoBehaviour 
{
    public int itemID;
    public string itemName;
    public ItemType type;
    public float value; 
    public int rarity;

    public void Use(Player player) 
    {
        switch (type) 
        {
            case ItemType.HP:
                player.AddHp(value);
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
            Debug.Log($"{itemName} added to inventory.");
            Destroy(gameObject); 
        }
    }
}