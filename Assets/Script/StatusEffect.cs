using System;
using UnityEngine;

[Serializable]
public enum StatusType { Buff, Debuff, Poison, Regen }

[Serializable]
public class StatusEffect 
{
    public int effectID;
    public string effectName;
    public float duration;
    public float effectValue;
    public StatusType type;

    private float _timeLeft;

    public void Start() {
        _timeLeft = duration;
    }

    public void Update(float delta) {
        _timeLeft -= delta;
    }

    public bool IsExpired() {
        return _timeLeft <= 0f;
    }

    // Apply immediate or per-tick effect - example: return value modification
    public void ApplyTo(PlayerStatus target) 
    {
        // Example: if Buff heal over time or poison reduce hp
        if (type == StatusType.Regen) 
        {
            target.Heal(effectValue * Time.deltaTime);
        } 
        else if (type == StatusType.Poison) 
        {
            target.TakeDamage(effectValue * Time.deltaTime);
        }
    }
}
