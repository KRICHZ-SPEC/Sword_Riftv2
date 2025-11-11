using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour 
{
    public float spawnRate = 10f;
    public List<GameObject> itemPrefabs = new List<GameObject>();
    public Transform spawnAreaMin;
    public Transform spawnAreaMax;

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
        Vector2 pos = new Vector2(Random.Range(spawnAreaMin.position.x, spawnAreaMax.position.x), Random.Range(spawnAreaMin.position.y, spawnAreaMax.position.y));
        Instantiate(itemPrefabs[idx], pos, Quaternion.identity);
    }
}