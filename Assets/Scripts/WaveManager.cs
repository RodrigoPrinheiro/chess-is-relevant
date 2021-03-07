﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WaveState
{
    RUNNING,
    STOPPED,
}

public class WaveManager : MonoBehaviour
{
    const float GROWTH_RATE = 0.2f;
    const float SCORE_PER_KILL = 7f;
    [Tooltip("Rate equals to enemies per minute")]
    [SerializeField] private float _initialRate = 10f;
    [SerializeField] private float _treshold = 30f;
    [SerializeField] private float _roundTime = 60f;
    [SerializeField] private float _timeBetweenWaves = 3f;
    [SerializeField] private Spawner _spawner;
    private float _rate;
    private float _time;
    private int _kills;
    private int _wave;
    private float _lastWaveTime;
    private float _waveTimeCounter;
    private float _playerScore;
    public WaveState CurrentWaveState { get; set; }

    private void Awake()
    {
        Actor.staticActorDeathEvent += TrackDeaths;

        _lastWaveTime = _roundTime;
        _wave = 1;

        StartCoroutine(WaitBetweenWaves());
    }

    // Update is called once per frame
    void Update()
    {
        if (_waveTimeCounter > 0)
        {
            CurrentWaveState = WaveState.RUNNING;
        }
        else if (CurrentWaveState != WaveState.STOPPED)
        {
            CurrentWaveState = WaveState.STOPPED;

            // Increment wave and call new wave event
            _wave++;
            newWaveEvent?.Invoke(_wave);

            // Create new wave time
            WaitBetweenWaves();
        }

        // Manage Spawns
        if (CurrentWaveState.Equals(WaveState.RUNNING))
        {
            ManageSpawns();
        }
    }

    private void ManageSpawns()
    {
        if (_time > 0)
        {
            _time -= Time.deltaTime;
            
        }
        else
        {
            float _maxMonsterStrength = _playerScore / 1000f;
            float _monsterPowerLevel = UnityEngine.Random.value;
            _monsterPowerLevel *= _monsterPowerLevel;
            _monsterPowerLevel *= _maxMonsterStrength;

            if (UnityEngine.Random.value >= 0.25f)
            {
                _spawner.Spawn(_monsterPowerLevel);
            }

            // Reset spawn timer
            _time = (UnityEngine.Random.Range(0.7f, 1f) * _roundTime) / _rate;
            _time *= Mathf.Sqrt(_monsterPowerLevel);
        }
    }

    public void TrackDeaths(Actor from, Actor target)
    {
        if (target is EnemyActor)
        {
            // Add a kill
            _kills++;
            _playerScore += SCORE_PER_KILL;

            if (_rate < _treshold)
            {
                // Calculate the new spawn rate
                _rate = _initialRate + (GROWTH_RATE * _kills);
            }
            else
            {
                _rate += 1 / Mathf.Sqrt(_rate);
            }
        }
    }

    private IEnumerator WaitBetweenWaves()
    {
        yield return new WaitForSeconds(_timeBetweenWaves);

        // Create new wave time
        _waveTimeCounter = _lastWaveTime + (GROWTH_RATE * _kills * 3f);
        Debug.Log(_waveTimeCounter / 60f);
        CurrentWaveState = WaveState.RUNNING;
    }

    public event Action<int> newWaveEvent;
}
