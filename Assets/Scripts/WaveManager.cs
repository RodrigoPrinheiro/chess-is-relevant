using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WaveState
{
    RUNNING,
    STOPPED,
    WAITING,
    CANCEL
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
    [SerializeField] private UpgradeBooth _upgrades;
    [SerializeField] private ActiveEnemies _activeEnemiesSO;
    [Header("Debug")]
    [SerializeField] private bool _debugWaves;
    [SerializeField] private bool _debugEnemies;
    private float _rate;
    private float _time;
    private int _kills;
    private int _wave;
    private float _lastWaveTime;
    private float _waveTimeCounter;
    private float _playerScore;

    private float _gameTime;
    public WaveState CurrentWaveState { get; set; }
    public float GameTime => _gameTime;

    private void Awake()
    {
        Actor.StaticActorDeathEvent += TrackDeaths;

        _lastWaveTime = _roundTime;
        _rate = _initialRate;

        _activeEnemiesSO.ResetList();
        CurrentWaveState = WaveState.STOPPED;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentWaveState == WaveState.CANCEL) return;
        _gameTime += Time.deltaTime;
        if (_waveTimeCounter > 0)
        {
            CurrentWaveState = WaveState.RUNNING;
            _waveTimeCounter -= Time.deltaTime;

            if (_waveTimeCounter <= 0.0f)
            {
                CurrentWaveState = WaveState.STOPPED;

                WaveEnd();

                if (_debugWaves)
                    Debug.Log("Wave end: " + CurrentWaveState + "\n Enemies remaining: " + _activeEnemiesSO.ActiveEnemiesCount + "\n Upgrade available: " + _upgrades.Active);
            }
        }
        // If the waves are stopped
        else if (CurrentWaveState == WaveState.STOPPED)
        {
            // Await for no enemies and the upgrade gathered
            if (_activeEnemiesSO.ActiveEnemiesCount <= 0 && !_upgrades.Active)
            {
                CurrentWaveState = WaveState.WAITING;
                NewWave();
            }
        }

        // Manage Spawns
        if (CurrentWaveState == WaveState.RUNNING)
        {
            ManageSpawns();
        }
    }

    private void NewWave()
    {
        if (_debugWaves)
            Debug.Log("New Wave");
        
        // Increment wave and call new wave event
        _wave++;
        newWaveEvent?.Invoke(_wave);

        _activeEnemiesSO.ResetList();
        
        // Create new wave time and set waves to Running
        StartCoroutine(WaitBetweenWaves());
    }

    private void WaveEnd()
    {
        _upgrades.EnableUpgrade();
        
        waveEndEvent?.Invoke(_wave);
    }
#region EnemySpawnManagement
    private void ManageSpawns()
    {
        if (_time > 0)
        {
            _time -= Time.deltaTime;
        }
        else
        {
            string powerLevel;
            if ((_wave % 5) == 0)
            {
                _spawner.SpawnBoss(UnityEngine.Random.value * 2);
                powerLevel = "Boss";
                _waveTimeCounter = 0.001f;
            }
            else
            {
                float _maxMonsterStrength = _playerScore / 1000f;
                float _monsterPowerLevel = UnityEngine.Random.value;
                _monsterPowerLevel *= _monsterPowerLevel;
                _monsterPowerLevel *= _maxMonsterStrength;

                powerLevel = _monsterPowerLevel.ToString();

                if (UnityEngine.Random.value >= 0.25f)
                {
                    _spawner.Spawn(_monsterPowerLevel);
                }
            }
            // Reset spawn timer
            _time = (UnityEngine.Random.Range(0.7f, 1f) * _roundTime) / _rate;
            // _time *= Mathf.Sqrt(_monsterPowerLevel);
            if (_debugWaves && _debugEnemies)
                Debug.Log("Next Spawn in: " + _time + "seconds. Power Level: " + powerLevel);
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
#endregion
    private IEnumerator WaitBetweenWaves()
    {
        yield return new WaitForSeconds(_timeBetweenWaves);

        // Create new wave time
        _waveTimeCounter = _lastWaveTime + (GROWTH_RATE * _kills * 3f);
        
        if (_debugWaves)
            Debug.Log("Next Wave in: " + (_waveTimeCounter / 60f) + " minutes");
        
        CurrentWaveState = WaveState.RUNNING;
    }

    public event Action<int> newWaveEvent;
    public event Action<int> waveEndEvent;
}
