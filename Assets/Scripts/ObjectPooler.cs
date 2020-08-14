using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private GameObject _objectToPool = default;
    [SerializeField] private int _amountToPool = 0;

    private List<GameObject> pooledObjects;

    public int AmountToPool => _amountToPool;


    private void Awake()
    {
        pooledObjects = new List<GameObject>(AmountToPool);
        for (int i = 0; i < AmountToPool; ++i)
        {
            GameObject obj = Instantiate(_objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
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
