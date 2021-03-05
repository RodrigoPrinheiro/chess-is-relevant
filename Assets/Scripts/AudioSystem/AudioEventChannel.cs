using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName="Events/Audio Event")]
public class AudioEventChannel : ScriptableObject
{
    public event Action<AudioClip, AudioConfiguration, Vector3> audioRequest;

    public void RaiseEvent(AudioClip clip, AudioConfiguration configuration, Vector3 audioPosition)
    {
        if (audioRequest != null)
        {
            audioRequest.Invoke(clip, configuration, audioPosition);
        }
        else
        {
            Debug.LogWarning("Audio Event is being raised but no one is listening to it," +
              "make sure you have an audio listener in scene");
        }
    }
}
