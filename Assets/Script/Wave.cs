using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave 
{
    public int waveNumber;
    public List<GameObject> enemyPrefabs; 
    public int spawnCount = 5;
    public float spawnInterval = 1f;
    public Transform spawnPoint;

    public IEnumerator SpawnEnemies(Transform parent) 
    {
        for (int i = 0; i < spawnCount; i++) 
        {
            if (enemyPrefabs.Count == 0) yield break;
            int idx = Random.Range(0, enemyPrefabs.Count);
            var prefab = enemyPrefabs[idx];
            GameObject.Instantiate(prefab, spawnPoint.position, Quaternion.identity, parent);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}