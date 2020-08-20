using System;
using UnityEngine;

public class ShieldControl : MonoBehaviour
{
    public event Action OnControllShield;

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

    private void CheckIsTriggered()
    {

    }

}
