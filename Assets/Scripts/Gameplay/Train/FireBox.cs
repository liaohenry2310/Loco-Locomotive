using Interfaces;
using System;
using UnityEngine;

public class FireBox : MonoBehaviour, IInteractable
{
    public event Action OnReloadFuel;

    public void Interact(PlayerV1 player)
    {
        if (player.GetItem.ItemType == DispenserData.Type.Fuel)
        {
            OnReloadFuel?.Invoke();
            player.GetItem.DestroyAfterUse();
        }
    }
   
}
