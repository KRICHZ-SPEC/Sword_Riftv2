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
}