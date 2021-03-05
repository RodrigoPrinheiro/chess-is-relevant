using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Actor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(Actor from, float damage)
    {
        damageEvent?.Invoke(from, this, damage);
    }

    /// <summary>
    /// Takes in from whom the damage came from, who took the damage and the amount
    /// </summary>
    public static Action<Actor, Actor, float> damageEvent;
    public static Action<Actor> actorDeathEvent;
    public Action actorHit;
    public Action actorDeath;
}
