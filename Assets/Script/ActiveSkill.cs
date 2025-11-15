using UnityEngine;

using UnityEngine;

[System.Serializable]
public class ActiveSkill
{
    public int skillID;
    public string skillName;
    public float cooldown = 1f;
    public float lastUsedTime = -999f;

    public bool CanUse()
    {
        return Time.time >= lastUsedTime + cooldown;
    }

    public virtual void Activate(Player player)
    {
        Debug.Log("Use Skill: " + skillName);
    }
}