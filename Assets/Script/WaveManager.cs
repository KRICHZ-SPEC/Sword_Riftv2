using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<Waves> waves = new List<Waves>();
    public Player player;          
    public ActiveSkill fireballSkill; 
    private int currentWave = 0;
    private bool waveActive = false;

    // --- ลบ void Start() ออกจากไฟล์นี้ ---
    // void Start()
    // {
    //     StartWave(0); // <--- ลบส่วนนี้ทิ้ง
    // }

    public void StartWave(int index)
    {
        if (index >= waves.Count) return;

        currentWave = index;
        waveActive = true;

        Debug.Log("Start Wave " + (index + 1));

        waves[currentWave].SpawnEnemies();
        
        if (currentWave == 1) 
        {
            if (player != null && fireballSkill != null)
            {
                player.skills.Add(fireballSkill);
                Debug.Log("Fireball Skill Unlocked!");
            }
        }
    }

    public void OnEnemyKilled()
    {
        if (!waveActive) return;

        if (waves[currentWave].IsWaveCleared())
        {
            waveActive = false;
            Debug.Log("Wave " + (currentWave + 1) + " Cleared!");
            currentWave++;

            if (currentWave < waves.Count)
            {
                StartWave(currentWave);
            }
            else
            {
                Debug.Log("All Waves Completed!");
            }
        }
    }
}