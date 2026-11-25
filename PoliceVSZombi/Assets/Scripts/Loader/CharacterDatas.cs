using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CharacterDatas
{
    /// <summary>
    /// 키
    /// </summary>
    public int key;

    /// <summary>
    /// 캐릭터 이름
    /// </summary>
    public string name;

    /// <summary>
    /// ExperienceDatas Key
    /// </summary>
    public int minLevel;

    /// <summary>
    /// ExperienceDatas Key
    /// </summary>
    public int maxLevel;

}
public class CharacterDatasLoader
{
    public List<CharacterDatas> ItemsList { get; private set; }
    public Dictionary<int, CharacterDatas> ItemsDict { get; private set; }

    public CharacterDatasLoader(string path = "JSON/CharacterDatas")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, CharacterDatas>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<CharacterDatas> Items;
    }

    public CharacterDatas GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public CharacterDatas GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
