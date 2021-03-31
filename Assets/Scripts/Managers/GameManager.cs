using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public string PlayerName {get; set;}
    private void OnEnable() {
        Actor.StaticActorDeathEvent += PlayerDeath;
    }

    private void OnDisable() {
        Actor.StaticActorDeathEvent -= PlayerDeath;
    }

    private void PlayerDeath(Actor src, Actor target)
    {
        if (target is PlayerActor)
        {
            OnGameEnd();
        }
    }

    private void OnGameEnd()
    {
        WaveManager waves = FindObjectOfType<WaveManager>();
        waves.CurrentWaveState = WaveState.CANCEL;
        
        Debug.Log("End Game");
        gameEndEvent?.Invoke();
    }

    public static event System.Action gameEndEvent;
    
}
