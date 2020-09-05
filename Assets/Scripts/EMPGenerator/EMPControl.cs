using System;
using UnityEngine;

public class EMPControl : MonoBehaviour
{
    public event Action<bool> OnTriggerEMP;

    private InputReciever _inputReciever;
    public SpriteRenderer SpriteEMPController { get; private set; }


    private void Awake()
    {
        SpriteEMPController = GetComponent<SpriteRenderer>();

        if (!TryGetComponent(out _inputReciever))
        {
            Debug.LogWarning($"[EMPGenerator] -- Failed to get the component: {_inputReciever.name}");
        }
    }

    private void Update()
    {
        OnTriggerEMP?.Invoke(_inputReciever.GetSecondaryInput());
    }
}
