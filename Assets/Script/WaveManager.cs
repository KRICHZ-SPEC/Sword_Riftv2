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
    
    void OnEnable()
    {
        Enemy.OnEnemyDied += HandleEnemyDeath;
    }
    
    void OnDisable()
    {
        Enemy.OnEnemyDied -= HandleEnemyDeath;
    }

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
    
    void HandleEnemyDeath(Enemy enemy)
    {
        if (!waveActive) return;
        
        if (currentWave < waves.Count)
        {
             waves[currentWave].RemoveEnemyFromList(enemy);
        }
        
        CheckWaveStatus();
    }
    
    public void CheckWaveStatus()
    {
        if (waves[currentWave].IsWaveCleared())
        {
            waveActive = false;
            Debug.Log("Wave " + (currentWave + 1) + " Cleared!");
            currentWave++;

            if (currentWave < waves.Count)
            {
                Invoke(nameof(StartNextWave), 2f);
            }
            else
            {
                Debug.Log("All Waves Completed!");
            }
        }
    }

    void StartNextWave()
    {
        StartWave(currentWave);
    }
}