using System;
using UnityEngine;

public class ShieldGeneratorController : MonoBehaviour
{
    public event Action OnControllShield;

    [SerializeField] private ShieldControl _shieldControl;
    
    private InputReciever _inputReciever;


    private void Awake()
    {
        if (!TryGetComponent(out _inputReciever))
        {
            Debug.LogWarning($"[ShiledGenerator] -- Failed to get the component: {_inputReciever.name}");
        }
    }


}
