using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    enum State
    {
        Idle = 0,
        Move = 1,
        IdleAndHolding = 2,
        MovingAndHolding = 3,
        Climbing = 4,
        ClimbingAndHolding =5,
        UsingTurretBegin =6,
        UsingTurret =7
    }
    private GameObject player;
    public Animator animator;


}
