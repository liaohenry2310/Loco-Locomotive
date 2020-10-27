using Interfaces;
using Items;
using System;
using UnityEngine;

public class FireBox : MonoBehaviour, IInteractable
{
    public event Action OnReloadFuel;

    public void Interact(PlayerV1 player)
    {
        Item item = player.GetItem;
        if (item != null && item.ItemType == DispenserData.Type.Fuel)
        {
            OnReloadFuel?.Invoke();
            item.DestroyAfterUse();
        }
    }

}
