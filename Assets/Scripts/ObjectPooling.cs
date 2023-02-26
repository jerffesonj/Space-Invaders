using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int numPool;
    [SerializeField] private List<GameObject> pools = new List<GameObject>();

    public List<GameObject> Pools { get => pools; }

    void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < numPool; i++)
        {
            GameObject prefabClone = Instantiate(prefab, this.transform);
            pools.Add(prefabClone);
            prefabClone.SetActive(false);
        }
    }

    public GameObject GetPooledObj()
    {
        GameObject obj = null;
        foreach(GameObject gameObject in pools)
        {
            if (!gameObject.activeSelf)
            {
                obj = gameObject;
                return obj;
            }
        }

        GameObject prefabClone = Instantiate(prefab, this.transform);
        pools.Add(prefabClone);
        prefabClone.SetActive(false);

        return obj;
    }
}
