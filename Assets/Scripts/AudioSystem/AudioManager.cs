using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private int initialAudioPoolSize;
    [SerializeField] private AudioSource _sourcePrefab;
    [Header("Configuration")]
    [SerializeField] AudioEventChannel _sfxChannel;
    [SerializeField] AudioEventChannel _musicChannel;

    private Pool<AudioSource> _pool;

    public override void Awake() 
    {
        base.Awake();
        _sfxChannel.audioRequest += PlayAudioCue;
        _musicChannel.audioRequest += PlayAudioCue;
        
        _pool = new Pool<AudioSource>(_sourcePrefab, transform);
        _pool.Initialize(initialAudioPoolSize);
        _pool.SetRequestCondition((s) => !s.isPlaying);
    }

    private void PlayAudioCue(AudioClip clip, AudioConfiguration configuration, Vector3 position)
    {
        AudioSource playSource = _pool.Request();

        if (playSource != null)
        {
            // Setup AudioSource
            playSource.clip = clip;
            playSource.volume = configuration.Volume;
            playSource.pitch = configuration.Pitch;
            playSource.transform.position = position;
            playSource.outputAudioMixerGroup = configuration.Mixer;

            // Play
            playSource.Play();
        }
    }
}