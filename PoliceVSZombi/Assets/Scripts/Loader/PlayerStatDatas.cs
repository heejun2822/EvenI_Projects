using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerStatDatas
{
    /// <summary>
    /// 키
    /// </summary>
    public int key;

    /// <summary>
    /// 기본 체력
    /// </summary>
    public int baseHealth;

    /// <summary>
    /// 기본 공격력
    /// </summary>
    public int baseAttack;

    /// <summary>
    /// 기본 속도
    /// </summary>
    public float baseSpeed;

}
public class PlayerStatDatasLoader
{
    public List<PlayerStatDatas> ItemsList { get; private set; }
    public Dictionary<int, PlayerStatDatas> ItemsDict { get; private set; }

    public PlayerStatDatasLoader(string path = "JSON/PlayerStatDatas")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, PlayerStatDatas>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<PlayerStatDatas> Items;
    }

    public PlayerStatDatas GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public PlayerStatDatas GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
