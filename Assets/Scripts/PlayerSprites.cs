using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprites : MonoBehaviour
{
    public GameObject player;
    public Sprite[] sprites;
    public Sprite[] spriteSide;
    public SpriteRenderer _collectedItemSprite;

    private InputReciever mInputReceiver;
    private PlayerInput playerInput;
    private void Start()
    {
        mInputReceiver = player.GetComponentInParent<InputReciever>();
        
    }
    private void Update()
    {
        for(int i =0;i<PlayerInputManager.instance.playerCount;++i)
        {
            if (player.GetComponentInChildren<SpriteRenderer>().sprite == sprites[i]
                || player.GetComponentInChildren<SpriteRenderer>().sprite == spriteSide[i])
            {

                if (mInputReceiver.GetHorizontalInput() == -1)
                {
                    player.GetComponentInChildren<SpriteRenderer>().sprite = spriteSide[i];
                    player.GetComponentInChildren<SpriteRenderer>().flipX = false;
                }
                else if (mInputReceiver.GetHorizontalInput() == 1)
                {
                    player.GetComponentInChildren<SpriteRenderer>().sprite = spriteSide[i];
                    player.GetComponentInChildren<SpriteRenderer>().flipX = true;
                }
                else if (mInputReceiver.GetHorizontalInput() == 0)
                {
                    player.GetComponentInChildren<SpriteRenderer>().sprite = sprites[i];
                }
            }
        }
        if(player.GetComponentInChildren<SpriteRenderer>().flipX == true)
        {
            _collectedItemSprite.flipX = false;
        }
        else if (player.GetComponentInChildren<SpriteRenderer>().flipX == false)
        {
            _collectedItemSprite.flipX = true;
        }

    }

}


