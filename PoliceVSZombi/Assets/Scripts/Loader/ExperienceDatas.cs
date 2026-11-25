using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ExperienceDatas
{
    /// <summary>
    /// 키값
    /// </summary>
    public int key;

    /// <summary>
    /// 필요 경험치
    /// </summary>
    public int experienceRequired;

    /// <summary>
    /// 총 경험치
    /// </summary>
    public int totalExperience;

    /// <summary>
    /// 스텟 id
    /// </summary>
    public int statID;

}
public class ExperienceDatasLoader
{
    public List<ExperienceDatas> ItemsList { get; private set; }
    public Dictionary<int, ExperienceDatas> ItemsDict { get; private set; }

    public ExperienceDatasLoader(string path = "JSON/ExperienceDatas")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, ExperienceDatas>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<ExperienceDatas> Items;
    }

    public ExperienceDatas GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public ExperienceDatas GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
