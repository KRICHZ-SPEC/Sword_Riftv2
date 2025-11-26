using System.Collections.Generic;
using UnityEngine;

public interface IPooledObject
{
    void OnObjectSpawn();
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Prefab missing for pool spawn.");
            return null;
        }

        string key = prefab.name;

        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary.Add(key, new Queue<GameObject>());
        }

        GameObject objectToSpawn = null;
        
        int count = poolDictionary[key].Count;
        for (int i = 0; i < count; i++)
        {
            GameObject temp = poolDictionary[key].Dequeue();
            poolDictionary[key].Enqueue(temp);

            if (temp != null && !temp.activeInHierarchy)
            {
                objectToSpawn = temp;
                break;
            }
        }
        
        if (objectToSpawn == null)
        {
            objectToSpawn = Instantiate(prefab);
            objectToSpawn.name = prefab.name;
            poolDictionary[key].Enqueue(objectToSpawn);
        }
        
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);
        
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        return objectToSpawn;
    }
}
