using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwarmEnemyData", menuName = "SwarmEnemy")]
public class SwarmEnemyData : ScriptableObject
{
    public float MaxHealth;
    public float ShieldHealth;
    public float MaxSpeed;
    public float Speed;
    public float AttackDelay;

    public float Swarm_AttackDamage;
    public float Swarm_AttackSpeed;

    public float Swarm_tiltingAngle;


    public float WanderBehaviorWeight;
    public float WanderRadius;
    public float WanderDistance;
    public float WanderJitter;

    public float SeekBehaviorWeight;
    public float CohesionBehaviorWeight;
    //public float SeparationBehaviorWeight;
    //public float SeparationBehaviorRadius;
    //public float AlignmentBehaviorWeight;

}
