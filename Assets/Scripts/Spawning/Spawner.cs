using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _sameTypeRerollChance;
    [SerializeField] private ActorSpawnSettings[] _spawns;
    [SerializeField] private ActorSpawnSettings[] _bosses;
    [SerializeField] private Vector3 _arenaDimensions;
    private Transform _playerTransform;
    private ActorSpawnSettings _lastSpawned;
    private int _nextBossIndex;

    private void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerActor>().transform;
    }

    public void SpawnBoss(float power)
    {
        if (_bosses == null || _bosses.Length == 0)
            return;

        EnemyActor spawned = Create(_bosses[_nextBossIndex]);
        spawned.SetPower(power);

        if (_nextBossIndex + 1 < _bosses.Length)
            _nextBossIndex++;
    }
    public void Spawn(float power)
    {
        if (_spawns.Length <= 0) return;

        // 1 + power = increased stats
        
        List<ActorSpawnSettings> possible = new List<ActorSpawnSettings>();

        bool shouldRollIfSameType = Random.value <= _sameTypeRerollChance;
        for (int i = 0; i < _spawns.Length; i++)
        {
            if (_spawns[i].PowerLevelGate <= power + 1)
            {
                possible.Add(_spawns[i]);
            }
        }

        ActorSpawnSettings newSpawn = possible[Random.Range(0, possible.Count)];
        // if (_lastSpawned != null &&
        //     newSpawn.Type == _lastSpawned.Type && newSpawn.Attack == _lastSpawned.Attack)
        // {
        //     if (Random.value < _sameTypeRerollChance)
        //     {
        //         ActorSpawnSettings first = newSpawn;

        //         while(newSpawn == first)
        //             newSpawn = _spawns[Random.Range(0, _spawns.Length)];
        //     }
        // }

        EnemyActor spawned = Create(newSpawn);
        spawned.SetPower(power);

        _lastSpawned = newSpawn;
    }

    private EnemyActor Create(ActorSpawnSettings spawn)
    {
        // get random direction,
        float angle = Random.Range(0.0f, Mathf.PI * 2);
        Vector3 dir = new Vector3(Mathf.Sin(angle), 0.0f, Mathf.Cos(angle));

        // If its a flying enemy get random y direction aswell,
        if (spawn.aiValues.MovementType == MovementType.Air)
        {
            dir.y = Random.Range(0.5f, 1);
            dir.Normalize();
        }
        // multiply by the random distance
        Vector3 pos = dir * spawn.Distance;

        pos.x = Mathf.Clamp(pos.x, -_arenaDimensions.x, _arenaDimensions.x);
        pos.z = Mathf.Clamp(pos.z, -_arenaDimensions.z, _arenaDimensions.z);
        pos.y = Mathf.Clamp(pos.y, 0, _arenaDimensions.y);

        // Instantiate
        EnemyActor newEnemy = Instantiate(spawn.Prefab, pos, Quaternion.identity);

        AIEntity ai = newEnemy.gameObject.AddComponent<AIEntity>();
        ai.Actor = spawn;

        return newEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 center = Vector3.zero + Vector3.up * (_arenaDimensions.y / 2);
        Gizmos.DrawWireCube(center, _arenaDimensions);
    }
}