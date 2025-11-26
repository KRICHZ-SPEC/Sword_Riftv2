using UnityEngine;

public class Boss : Enemy 
{
    public string bossName;
    public int phase = 1;
    public ActiveSkill[] specialSkills;
    public float enragedThreshold = 0.3f; // % hp

    //protected override void Update() 
    //{
    //base.Update();
    //CheckPhase();
    //UseSkills();
    //}

    void CheckPhase() {
        float hpPercent = hp / 100f; // assume max 100 for simplicity
        if (hpPercent <= enragedThreshold) 
        {
            phase = 2;
            // maybe buff damage or change pattern
        }
    }

    void UseSkills() 
    {
        if (specialSkills == null) return;
        foreach (var s in specialSkills) 
        {
            if (s != null && s.CanUse()) 
            {
                //(FindObjectOfType<Player>());
            }
        }
    }

    // override attack patterns if needed
}