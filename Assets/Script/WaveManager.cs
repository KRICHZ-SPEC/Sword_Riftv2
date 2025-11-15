using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<Waves> waves = new List<Waves>();

    private int currentWave = 0;
    private bool waveActive = false;

    void Start()
    {
        StartWave(0);
    }

    public void StartWave(int index)
    {
        if (index >= waves.Count) return;

        currentWave = index;
        waveActive = true;

        Debug.Log("Start Wave " + (index + 1));

        waves[currentWave].SpawnEnemies();
        
        if (currentWave == 0)
        {
            FindObjectOfType<TutorialUI>().SendMessage("Attack the training dummy to test the combat system!");
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