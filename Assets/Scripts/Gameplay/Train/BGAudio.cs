using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGAudio : MonoBehaviour
{
    public AudioSource BgAudio;
    public AudioClip LowHealthClip;
    [SerializeField] private TrainData _trainData = null;
    private bool _audioPlayed = false;
    private void Update()
    {
        if (_trainData.HealthPercentage < 0.2f && !_audioPlayed)
        {
            BgAudio.clip = LowHealthClip;
            BgAudio.Play();
            _audioPlayed = true;
        }
    }
}
