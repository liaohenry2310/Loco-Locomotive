using UnityEngine;
using System;
using System.Collections.Generic;

//[CreateAssetMenu(fileName = "EnemyLevel", menuName = "EnemyLevelInit/Waves")]
public class EnemyLevel : ScriptableObject
{
    [Serializable]
    public class EnemyIniti
    {
        public EnemySpawner_old.EnemyType enemyType =0;
        public int numOfEnemies =0;
        public float wave_delay = 0;

        public float wormholeRotationSpeed = 0.0f;
        public float wormholeGrowthRate = 0.0f;
        public float wormholeSpawnTime = 0.0f;

        //[HideInInspector]
        private bool startedSpawn = false;
        public bool StartedSpawn { get {return startedSpawn; } private set { } }
    }
    [SerializeField]
    public List<EnemyIniti> _waves = new List<EnemyIniti>();
}

//_waves  = 3  