using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Waves
{
    public string waveName;
    public List<Enemy> enemyPrefabs;
    public Transform[] spawnPoints;

    private List<Enemy> aliveEnemies = new List<Enemy>();

    public void SpawnEnemies()
    {
        aliveEnemies.Clear();

        if (spawnPoints.Length == 0) 
        {
            Debug.LogError("No spawn points defined!");
            return;
        }
        
        foreach (Enemy prefab in enemyPrefabs)
        {
            if (prefab == null) continue;
            
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            
            Enemy e = GameObject.Instantiate(prefab, randomPoint.position, Quaternion.identity);
            aliveEnemies.Add(e);
        }
    }
    
    public void RemoveEnemyFromList(Enemy enemy)
    {
        if (aliveEnemies.Contains(enemy))
        {
            aliveEnemies.Remove(enemy);
        }
    }

    public bool IsWaveCleared()
    {
        aliveEnemies.RemoveAll(e => e == null);
        return aliveEnemies.Count == 0;
    }
}