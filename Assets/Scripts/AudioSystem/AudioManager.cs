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
    private List<AudioSource> _sources;

    public override void Awake()
    {
        base.Awake();
        _sfxChannel.audioRequest += PlayAudioCue;
        _musicChannel.audioRequest += PlayAudioCue;

        if (_sources == null)
        {
            _sources = new List<AudioSource>();
        }
    }

    private void PlayAudioCue(AudioClip clip, AudioConfiguration configuration, Vector3 position)
    {
        AudioSource playSource = GetSource();

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

    private AudioSource GetSource()
    {
        AudioSource source = null;
        for (int i = 0; i < _sources.Count; i++)
        {
            if (!_sources[i].isPlaying)
            {
                source = _sources[i];
            }
        }

        if (source == null)
        {
            GameObject obj = new GameObject("Manager Source");
            source = obj.AddComponent<AudioSource>();
        }
        
        return source;
    }
}