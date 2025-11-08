using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour 
{
    public List<Wave> waves = new List<Wave>();
    public int currentWave = 0;
    public Transform enemyParent;
    public float timeBetweenWaves = 3f;

    void Start() 
    {
        if (waves.Count > 0) StartCoroutine(StartWaves());
    }

    IEnumerator StartWaves() 
    {
        while (currentWave < waves.Count) 
        {
            yield return StartCoroutine(RunWave(waves[currentWave]));
            currentWave++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
        OnAllWavesCleared();
    }

    IEnumerator RunWave(Wave wave) 
    {
        yield return StartCoroutine(wave.SpawnEnemies(enemyParent));
        // wait until all enemies dead
        while (enemyParent.childCount > 0) 
        {
            yield return null;
        }
    }

    void OnAllWavesCleared() 
    {
        Debug.Log("All waves cleared!");
        // trigger next scene or boss etc.
    }
}