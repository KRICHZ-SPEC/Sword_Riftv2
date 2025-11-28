using UnityEngine;

[System.Serializable]
public abstract class ActiveSkill : ScriptableObject
{
    public float cooldown = 1f;
    public abstract void Activate(Player player);
}