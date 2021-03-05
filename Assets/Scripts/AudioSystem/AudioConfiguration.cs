using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[CreateAssetMenu(menuName = "Audio/Audio Configuration")]
public class AudioConfiguration : ScriptableObject
{
    [SerializeField, Range(0,1)] private float volume = 1f;
    [Header("Volume Rnd settings")]
    [SerializeField] private bool randomizeVolume;
    [SerializeField, Range(0,1)] private float minVolume;
    [SerializeField, Range(0,1)] private float maxVolume = 1f;
    [Space(30)]
    [SerializeField, Range(0,1)] private float pitch = 1f;
    [Header("Pitch Rnd settings")]
    [SerializeField] private bool randomizePitch;
    [SerializeField, Range(0,1)] private float minPitch = 1f;
    [SerializeField, Range(0,1)] private float maxPitch = 1f;

    [Header("Mixer Settings")]
    [SerializeField] private AudioMixerGroup audioCueMixer;
    public float Volume 
    {
        get 
        {
            if (randomizeVolume)
                return Random.Range(minVolume, maxVolume);
            else
                return volume;
        }
    }

    public float Pitch 
    {
        get 
        {
            if (randomizePitch)
                return Random.Range(minPitch, maxPitch);
            else
                return pitch;
        }
    }

    public AudioMixerGroup Mixer => audioCueMixer;
}
