using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BomberEnemyData", menuName = "BomberEnemy")]
public class BomberEnemyData : ScriptableObject
{
    public float MaxHealth;
    public float ShieldHealth;
    public float MaxSpeed;
    public float AttackDelay;
    public GameObject projectile;
    public float Bomber_AttackDamage;
    public float Bomber_AttackSpeed;



    public float WallAvoidWeight;

    public float WanderBehaviorWeight;
    public float WanderRadius;
    public float WanderDistance;
    public float WanderJitter;
}
