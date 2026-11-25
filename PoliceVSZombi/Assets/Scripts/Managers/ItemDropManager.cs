using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemDropManager : Singleton<ItemDropManager>
{
    [SerializeField] List<GameObject> dropItemPrefab = new List<GameObject>();

    public void DropRandomWeapon(float dropRate, Vector3 position)
    {
        if (dropItemPrefab.Count == 0 || dropRate == 0)
            return;

        int percent = Random.Range(0, 101);

        if (percent >= dropRate)
            return;

        int randomIndex = Random.Range(0, dropItemPrefab.Count);
        CreateItem(randomIndex, position);
    }

    void CreateItem(int index, Vector3 position)
    {
        GameObject prefab = dropItemPrefab[index];
        GameObject go = Instantiate(prefab, position, Quaternion.identity);

        Destroy(go, 5);
    }
}
