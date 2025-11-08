using UnityEngine;

public enum ItemType { HP, MP, Buff, Key }

public class Item : MonoBehaviour 
{
    public int itemID;
    public string itemName;
    public ItemType type;
    public float value;
    public int rarity; // 0 common, 1 rare etc.

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
                // create status effect and apply
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
        // auto-use or add to inventory
        // Example: auto-use HP pot
        if (type == ItemType.HP || type == ItemType.MP) 
        {
            Use(player);
        } 
        else 
        {
            // added to inventory, handled elsewhere
        }
    }

    public void ApplyEffect(Player player) 
    {
        Use(player);
    }
}