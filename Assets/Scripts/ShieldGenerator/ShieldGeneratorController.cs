using System;
using UnityEngine;

public class ShieldGeneratorController : MonoBehaviour
{
    public event Action OnControllShield;

    [SerializeField] private ShieldControl _shieldControl;

}
