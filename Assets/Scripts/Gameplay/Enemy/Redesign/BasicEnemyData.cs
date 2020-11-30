using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicEnemyData", menuName = "BasicEnemy")]
public class BasicEnemyData : ScriptableObject
{
    public float MaxHealth;
    public float ShieldHealth;
    public float MaxSpeed;
    public float AttackDelay;
    public GameObject projectile;
    public float Basic_AttackDamage;
    public float Basic_AttackSpeed;

    //Any shared movement data can be put here as well, for example...
    public float WallAvoidWeight;

    public float WanderBehaviorWeight;
    public float WanderRadius;
    public float WanderDistance;
    public float WanderJitter;
}
