using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [Header("Pool Settings")]
    public GameObject objPrefab;
    public int poolSize = 10;

    private Queue<GameObject> pool;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objPrefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // If pool is empty, instantiate a new object
            GameObject obj = Instantiate(objPrefab, transform);
            return obj;
        }
    }

    public void ReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        obj.transform.localScale = Vector3.one;
        pool.Enqueue(obj);
    }

    public void ReleaseObjectWithDelay(GameObject obj, float delay)
    {
        StartCoroutine(ReleaseObjectWithDelay_Co(obj, delay));
    }

    IEnumerator ReleaseObjectWithDelay_Co(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        ReleaseObject(obj);
    }
}
