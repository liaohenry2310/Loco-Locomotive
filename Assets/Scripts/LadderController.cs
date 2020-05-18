using UnityEngine;
using System.Collections.Generic;

public class LadderController : MonoBehaviour
{
    private PlayerController player;
    private Vector2 ladderTopPosition;
    private List<BoxCollider2D> passbleFloors = new List<BoxCollider2D>();

    public Vector2 GetLadderTopPosition()
    {
        return ladderTopPosition;
    }

    private void Start()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach(Transform transform in transforms)
        {
            if (transform.gameObject.name == "LadderTop")
            {
                ladderTopPosition = transform.position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PassableFloor"))
        {
            passbleFloors.Add(collision.GetComponent<BoxCollider2D>());
        }

        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerController>();
            player.LadderController = this;
            foreach (BoxCollider2D floor in passbleFloors)
            {
                Physics2D.IgnoreCollision(player.GetComponent<CapsuleCollider2D>(), floor, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PassableFloor"))
        {
            passbleFloors.Remove(collision.GetComponent<BoxCollider2D>());
        }

        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerController>();
            player.LadderController = null;
            foreach (BoxCollider2D floor in passbleFloors)
            {
                Physics2D.IgnoreCollision(player.GetComponent<CapsuleCollider2D>(), floor, false);
            }
        }
    }
}
