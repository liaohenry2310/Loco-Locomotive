using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropSpawner : MonoBehaviour
{
    public float Leftwidth = -6f;
    public float Rgihtwidth = 4f;
    public Item_Drop itemdrop;
    public DispenserItem[] ListItem;
    [SerializeField] private float _minSpawnTime = 0.0f;
    [SerializeField] private float _maxSpawnTime = 0.0f;
    private float _time = 0.0f;

    void Start()
    {
        _time = _minSpawnTime;
    }

    void FixedUpdate()
    {
        _time -= Time.deltaTime;
        if (_time < 0)
        {
            _time = Random.Range(_minSpawnTime, _maxSpawnTime);
            float posX = Random.Range(Leftwidth, Rgihtwidth);  
            Vector3 spawnPosition = new Vector3(posX, transform.position.y, 0);
            Instantiate(itemdrop, spawnPosition, Quaternion.identity);
        }
    }
}
