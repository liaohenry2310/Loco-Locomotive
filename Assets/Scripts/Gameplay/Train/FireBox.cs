using Interfaces;
using Items;
using System;
using UnityEngine;

public class FireBox : MonoBehaviour, IInteractable
{
    public event Action OnReloadFuel;
    public Animator animator;

    public void Interact(PlayerV1 player)
    {
        Item item = player.GetItem;
        animator.SetBool("Addfuel", false);
        if (item != null && item.ItemType == DispenserData.Type.Fuel)
        {
            animator.SetBool("Addfuel",true);
            OnReloadFuel?.Invoke();
            item.DestroyAfterUse();
        }
    }

}
