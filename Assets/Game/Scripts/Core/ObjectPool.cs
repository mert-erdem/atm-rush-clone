using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Component
{
    private static ObjectPool<T> instance;
    public static ObjectPool<T> Instance => instance ??= FindObjectOfType<ObjectPool<T>>();

    [SerializeField] private T poolObject;
    [SerializeField] private new BoxCollider collider;
    [SerializeField] private int poolSize;
    private List<T> pool;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        pool = new List<T>();

        for (int i = 0; i < poolSize; i++)
        {
            var objectForPool = Instantiate(poolObject, transform);
            objectForPool.gameObject.SetActive(false);
            pool.Add(objectForPool);
        }
    }

    public T GetObject()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pool[i].gameObject.activeInHierarchy)
                return pool[i];
        }

        return null;
    }

    public float GetObjectHeight()
    {
        return collider.size.y / 2;
    }
}
