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

        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            foreach (Transform spawn in spawnPoints)
            {
                Enemy e = GameObject.Instantiate(enemyPrefabs[i], spawn.position, Quaternion.identity);
                aliveEnemies.Add(e);
            }
        }
    }

    public bool IsWaveCleared()
    {
        aliveEnemies.RemoveAll(e => e == null);
        return aliveEnemies.Count == 0;
    }
}