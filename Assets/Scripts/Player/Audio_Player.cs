using UnityEngine;

/***
 * All this features are moved to the PlayerV1
 */
public class Audio_Player : MonoBehaviour
{
    //public AudioSource Audio;
    //private AudioClip hit;
    //private AudioClip death;
    //private PlayerV1 player;

    private void Awake()
    {
        //Audio = gameObject.AddComponent<AudioSource>();
        //player = gameObject.GetComponent<PlayerV1>();
        //Audio.playOnAwake = false;
        //hit = Resources.Load<AudioClip>("Audio/complete/sfx_playerhit");
        //death = Resources.Load<AudioClip>("Audio/complete/sfx_playerdeath");
        //Audio.volume = 0.1f;
    }
    private void Update()
    {
        //if (player.takeDamge)
        //{
        //    Audio.clip = hit;
        //    Audio.Play();
        //}
        //else if (player.death)
        //{
        //    Audio.clip = death;
        //    Audio.Play();
        //}
    }
}
