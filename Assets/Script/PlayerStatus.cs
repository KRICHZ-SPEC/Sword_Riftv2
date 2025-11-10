using System;
using UnityEngine;

[Serializable]
public class PlayerStatus
{
    public float hp;
    public float mp;
    public float exp;
    public int level;
    public float attack = 10f;
    
    public float maxHp;
    public float maxMp;
    
    private float expToNext = 100f;
    
    public event Action<int> OnLevelUp;
    
    public PlayerStatus(float initialHp = 100f, float initialMp = 50f)
    {
        maxHp = initialHp;
        maxMp = initialMp;
        hp = maxHp;
        mp = maxMp;
        exp = 0f;
        level = 1;
    }
    
    public void TakeDamage(float amount)
    {
        if (amount <= 0f) return;
        hp -= amount;
        if (hp < 0f) hp = 0f;
    }
    
    public void Heal(float amount)
    {
        hp += amount;
        if (hp > maxHp)
            hp = maxHp;
    }
    
    public void GainExperience(float amount)
    {
        if (amount <= 0f) return;
        exp += amount;
        
        while (exp >= expToNext)
        {
            exp -= expToNext;
            UpgradeLevel();
            expToNext = Mathf.Round(expToNext * 1.2f);
        }
    }
    
    public void UpgradeLevel()
    {
        level++;
        maxHp += 10f;
        maxMp += 5f;
        hp = maxHp;
        mp = maxMp;
        
        OnLevelUp?.Invoke(level);
    }
    public void ConsumeMp(float amount)
    {
        mp -= amount;
        if (mp < 0) mp = 0;
    }
}