using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GiantEnemyData", menuName = "GiantEnemy")]
public class GiantEnemyData : ScriptableObject
{
    public float MaxHealth;
    public float ShieldHealth;
    public float MaxSpeed;
    //public float Speed;
    public float AttackDelay;
    public float ChargeTime;
    public float BeamDamage;
    public float BeamDuration;//  beam that will last for [BeamDuration] seconds

    //Any shared movement data can be put here as well, for example...

    public float SeekWeight;

    public float WanderBehaviorWeight;
    public float WanderRadius;
    public float WanderDistance;
    public float WanderJitter;
    public float RandomHeadingTimer;

}
