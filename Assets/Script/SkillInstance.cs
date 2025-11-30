using UnityEngine;

[System.Serializable]
public class SkillInstance
{
    public ActiveSkill skillAsset; 
    private float lastUsedTime = -999f;

    public SkillInstance(ActiveSkill asset)
    {
        skillAsset = asset;
    }

    public bool CanUse()
    {
        if (skillAsset == null) return false;
        return Time.time >= lastUsedTime + skillAsset.cooldown;
    }

    public void Use(Player player)
    {
        if (!CanUse()) return;

        lastUsedTime = Time.time;
        skillAsset.Activate(player);
    }
    
    public float GetRemainingCooldown()
    {
        if (skillAsset == null) return 0f;
        float endTime = lastUsedTime + skillAsset.cooldown;
        return Mathf.Max(0f, endTime - Time.time);
    }
    
    public float GetCooldownRatio()
    {
        if (skillAsset == null || skillAsset.cooldown <= 0) return 0f;
        return GetRemainingCooldown() / skillAsset.cooldown;
    }
}