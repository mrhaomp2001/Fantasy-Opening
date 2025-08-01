using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject
{
    void OnObjectSpawnAfter();
}

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    public static ObjectPooler Instance;

    public Dictionary<string, Queue<GameObject>> PoolDictionary { get => poolDictionary; set => poolDictionary = value; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            if (poolDictionary.ContainsKey(pool.tag))
            {
                Debug.Log("Pool with tag '" + pool.tag + "' is duplicated");

                continue;
            }
            Queue<GameObject> objectPool = new Queue<GameObject>();

            GameObject objContainer = new GameObject(pool.tag);

            objContainer.transform.SetParent(transform);

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, objContainer.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag '" + tag + "' doesn't exist");
            return null;
        }

        GameObject objectToSpawn;

        objectToSpawn = poolDictionary[tag].Dequeue();
        poolDictionary[tag].Enqueue(objectToSpawn);

        if (objectToSpawn.activeSelf)
        {
            for (int i = 0; i < pools.Count; i++)
            {
                if (pools[i].tag.Equals(tag))
                {
                    objectToSpawn = Instantiate(pools[i].prefab, transform);
                    poolDictionary[tag].Enqueue(objectToSpawn);
                    objectToSpawn.SetActive(false);
                    break;
                }
            }
        }

        IPoolObject pooledObject = objectToSpawn.GetComponent<IPoolObject>();

        objectToSpawn.transform.SetPositionAndRotation(position, rotation);

        objectToSpawn.SetActive(true);

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawnAfter();
        }

        return objectToSpawn;
    }
}
