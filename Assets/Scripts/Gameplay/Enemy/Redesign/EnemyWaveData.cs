using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyWaveData", menuName = "EnemyWave")]
public class EnemyWaveData : ScriptableObject
{
    public enum EnemyType
    {
        Basic,
        Bomber,
        Swarm,
        Giant
    }

    //This is to match enemy type with the actual enemy prefab.
    [Serializable]
    public struct EnemyPrefab
    {
        EnemyType type;
        GameObject prefab;
    }

    public EnemyPrefab[] enemyPrefabs;

    //Data for enemy waves.
    [Serializable]
    public class EnemyWave
    {
        public EnemyType EnemyType=0;
        public int NumToSpawn=0;
        public float NextWaveTimeDelay=0.0f;
        public float WormholeSize=0.0f;
    }

    public List<EnemyWave> waveData  = new List<EnemyWave>();
}
