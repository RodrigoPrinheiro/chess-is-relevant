using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCue : MonoBehaviour
{
    [Header("Sound Definition")]
    [SerializeField] private AudioClip _soundClip;
    [SerializeField] private AudioConfiguration _soundConfig;
    [SerializeField] private bool _playOnStart;

    [Header("Configuration")]
    [SerializeField] private AudioEventChannel _channel;

    private void Start()
    {
        if (_playOnStart) Play();
    }

    // Start is called before the first frame update
    public void Play()
    {
        _channel.RaiseEvent(_soundClip, _soundConfig, transform.position);
    }
}
