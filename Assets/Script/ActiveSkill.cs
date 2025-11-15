using UnityEngine;

[System.Serializable]
public abstract class ActiveSkill : ScriptableObject
{
    public float cooldown = 1f;
    protected float lastUsedTime = -999f;

    public bool CanUse()
    {
        return Time.time >= lastUsedTime + cooldown;
    }

    public void UseSkill(Player player)
    {
        if (!CanUse()) return;

        lastUsedTime = Time.time;
        Activate(player);
    }

    public abstract void Activate(Player player);
}