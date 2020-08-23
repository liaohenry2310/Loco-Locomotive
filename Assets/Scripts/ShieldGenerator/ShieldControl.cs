using System;
using UnityEngine;

public class ShieldControl : MonoBehaviour
{
    public event Action OnControllShield;

    [NonSerialized] private InputReciever _inputReciever;

    private void Awake()
    {
        if (!TryGetComponent(out _inputReciever))
        {
            Debug.LogWarning($"[ShiledGenerator] -- Failed to get the component: {_inputReciever.name}");
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player player = collider.GetComponent<Player>();
            //player.turretLoader = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player player = collider.GetComponent<Player>();
            //player.turretLoader = this;
        }
    }

}