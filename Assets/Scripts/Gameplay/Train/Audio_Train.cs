//Not Use
using GamePlay;
using UnityEngine;

public class Audio_Train : MonoBehaviour
{
    [SerializeField] private TrainData _trainData = null;
    private Train _train;
    public AudioSource Audio;
    public AudioClip steam;
    public float rhythm = 1.0f;
    private float time = 0f;

    private void Awake()
    {
        _train = gameObject.GetComponent<Train>();
        Audio = gameObject.AddComponent<AudioSource>();
        steam = Resources.Load<AudioClip>("Audio/complete/sfx_steampuff01");
        Audio.playOnAwake = false;
        Audio.volume = 0.1f;
    }
    private void Update()
    {
        if (_trainData.CurrentFuel / _trainData.MaxFuel <= 0.3)
        {
            time += Time.deltaTime;
            if (time >= rhythm * 1.3f)
            {
                Audio.clip = steam;
                Audio.Play();
                time = 0;
            }
        }
        else if (_trainData.CurrentHealth / _trainData.MaxHealth <= 0.3)
        {
            time += Time.deltaTime;
            if (time >= rhythm * Random.Range(0.5f,1.5f))
            {
                Audio.clip = steam;
                Audio.Play();
                time = 0;
            }
        }
        else
        {
            time += Time.deltaTime;
            if (time >= rhythm)
            {
                Audio.clip = steam;
                Audio.Play();
                time = 0;
            }
        }           

    }
}
