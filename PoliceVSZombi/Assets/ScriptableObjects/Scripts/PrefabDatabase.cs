using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PrefabDatabase", menuName = "ScritableObjects/PrefabDatabase", order = 1)]
public class PrefabDatabase : ScriptableObject
{
    [System.Serializable]
    public struct PrefabData
    {
        public int key;
        public GameObject prefab;
    }

    public List<PrefabData> prefabList;
    private Dictionary<int , GameObject> prefabDictionary;

    public void Initialize()
    {
        prefabDictionary = new Dictionary<int, GameObject>();

        foreach (var data in prefabList)
        {
            if (!prefabDictionary.ContainsKey(data.key))
            {
                prefabDictionary.Add(data.key, data.prefab);
            }
        }
    }

    public GameObject GetPrefabByKey(int key)
    {
        if(prefabDictionary.ContainsKey(key))
        {
            return prefabDictionary[key];
        }
        else
        {
            Debug.LogWarning($"Prefab with key {key} not found.");
            return null;
        }
    }

}
