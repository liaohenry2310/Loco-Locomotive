using UnityEngine;

public class EMPTurret : MonoBehaviour
{
    public IMachineriesActions IMachineriesAction { get; set; }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player player = collider.GetComponent<Player>();
            player.machineriesActions = IMachineriesAction;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Player player = collider.GetComponent<Player>();
            player.machineriesActions = null;
        }
    }
}
