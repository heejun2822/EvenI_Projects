using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public struct Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in pools)
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, queue);
        }
    }

    public GameObject SpawnFromPool(string poolTag)
    {
        if(!poolDictionary.ContainsKey(poolTag))
        {
            Debug.Log($"{poolTag} is not in the pool dictionary");
            return null;
        }

        GameObject obj = poolDictionary[poolTag].Dequeue();
        poolDictionary[poolTag].Enqueue(obj);

        return obj;
    }

    public void ResetPoolDictionary()
    {
        List<string> keys = new List<string>(poolDictionary.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            for (int j = 0; j < poolDictionary[keys[i]].Count; j++)
            {
                GameObject obj = poolDictionary[keys[i]].Dequeue();
                obj.SetActive(false);
                poolDictionary[keys[i]].Enqueue(obj);
            }
        }
    }
}
