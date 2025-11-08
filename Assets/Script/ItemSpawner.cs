using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour 
{
    public float spawnRate = 10f;
    public List<GameObject> itemPrefabs = new List<GameObject>();
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    void Start() 
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnRandomItem();
        }
    }

    void SpawnRandomItem() 
    {
        if (itemPrefabs.Count == 0) return;
        int idx = Random.Range(0, itemPrefabs.Count);
        Vector2 pos = new Vector2(Random.Range(spawnAreaMin.x, spawnAreaMax.x), Random.Range(spawnAreaMin.y, spawnAreaMax.y));
        Instantiate(itemPrefabs[idx], pos, Quaternion.identity);
    }
}