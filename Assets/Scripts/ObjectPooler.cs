using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //public static ObjectPooler SharedInstance;

    [Header("Properties")]
    public GameObject objectToPool;
    
    public int amountToPool;
    public int AmountToPool => amountToPool;

    private List<GameObject> pooledObjects;

    private void Awake()
    {
        //SharedInstance = this;
        pooledObjects = new List<GameObject>(AmountToPool);
        for (int i = 0; i < AmountToPool; ++i)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    private void Start()
    {
        //pooledObjects = new List<GameObject>(AmountToPool);
        //for (int i = 0; i < AmountToPool; ++i)
        //{
        //    GameObject obj = Instantiate(objectToPool);
        //    obj.SetActive(false);
        //    pooledObjects.Add(obj);
        //}
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < AmountToPool; ++i)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
