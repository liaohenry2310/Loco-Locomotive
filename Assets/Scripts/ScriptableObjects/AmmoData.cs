﻿using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDataObject", menuName = "Weapon/Ammo")]
public class AmmoData : ScriptableObject
{
    [SerializeField] private float _damage = 0.0f;
    [SerializeField] private float _moveSpeed = 0.0f;
    [SerializeField] private DispenserData.Type _type = DispenserData.Type.Normal;

    public float Damage => _damage;
    public float MoveSpeed => _moveSpeed;
    public DispenserData.Type Type => _type;

}
