using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    public Vector2 LadderTopPosition { get; private set; } = Vector2.zero;

    private readonly List<BoxCollider2D> _passbleFloors = new List<BoxCollider2D>();

    private void Start()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform transform in transforms)
        {
            if (transform.gameObject.name == "LadderTop")
            {
                LadderTopPosition = transform.position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PassableFloor"))
        {
            _passbleFloors.Add(collision.GetComponent<BoxCollider2D>());
        }

        if (collision.CompareTag("Player"))
        {
            PlayerV1 _player = collision.GetComponent<PlayerV1>();
            _player.LadderController = this;
            _player.IsOnfloor = true;
            foreach (BoxCollider2D floor in _passbleFloors)
            {
                Physics2D.IgnoreCollision(_player.GetComponent<CapsuleCollider2D>(), floor, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PassableFloor"))
        {
            _passbleFloors.Remove(collision.GetComponent<BoxCollider2D>());
        }

        if (collision.CompareTag("Player"))
        {
            PlayerV1 _player = collision.GetComponent<PlayerV1>();
            _player.LadderController = null;
            _player.IsOnfloor = false;
            foreach (BoxCollider2D floor in _passbleFloors)
            {
                Physics2D.IgnoreCollision(_player.GetComponent<CapsuleCollider2D>(), floor, false);
            }
        }
    }
}
