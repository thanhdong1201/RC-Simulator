using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    //private void Start()
    //{
    //    for (int i = 0; i < poolSize; i++)
    //    {
    //        GameObject obj = Instantiate(prefab);
    //        obj.SetActive(false);
    //        pool.Enqueue(obj);
    //    }
    //}

    //public GameObject GetObject()
    //{
    //    if (pool.Count == 0)
    //    {
    //        GameObject obj = Instantiate(prefab);
    //        obj.SetActive(false);
    //        return obj;
    //    }

    //    GameObject pooledObj = pool.Dequeue();
    //    return pooledObj;
    //}

    //public void ReturnObject(GameObject obj)
    //{
    //    obj.SetActive(false);
    //    pool.Enqueue(obj);
    //}
}
