using UnityEngine;

public abstract class ActiveSkill : ScriptableObject 
{
    public int skillID;
    public string skillName;
    public float cooldown;
    public float lastUsedTime = -999f;
    public float mpCost = 0f;

    public bool CanUse() 
    {
        return Time.time - lastUsedTime >= cooldown;
    }

    public virtual bool Use(Player player) 
    {
        if (!CanUse()) return false;
        if (player.status.mp < mpCost) return false;
        player.status.ConsumeMp(mpCost);
        lastUsedTime = Time.time;
        Activate(player);
        return true;
    }

    protected abstract void Activate(Player player);
}