using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class TimedParticles : MonoBehaviour
{
    private ParticleSystem _particles;
    private float liveTime;
    private void Awake() 
    {
        _particles = GetComponent<ParticleSystem>();
        liveTime = _particles.main.duration;
    }

    private void Start() 
    {
        Destroy(gameObject, liveTime);
    }
}
