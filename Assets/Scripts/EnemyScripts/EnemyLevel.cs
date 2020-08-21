using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyLevel", menuName = "EnemyLevelInit/Waves")]
public class EnemyLevel : ScriptableObject
{
    [Serializable]
    public class EnemyIniti
    {
        public EnemySpawner.EnemyType enemyType =0;
        public int numOfEnemies =0;
        public float wave_delay = 0;
    }
    [SerializeField]
    public List<EnemyIniti> _waves = new List<EnemyIniti>();
}

//_waves  = 3  