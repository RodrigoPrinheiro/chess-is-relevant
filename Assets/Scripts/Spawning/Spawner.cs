using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _sameTypeRerollChance;
    [SerializeField] private ActorSpawnSettings[] _spawns;
    private Transform _playerTransform;
    private ActorSpawnSettings _lastSpawned;
    private void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerActor>().transform;
    }

    public void Spawn(float power)
    {
        if (_spawns.Length <= 0) return;
        
        // 1 + power = increased stats
        ActorSpawnSettings newSpawn = _spawns[Random.Range(0, _spawns.Length)];
        if (_lastSpawned != null &&
            newSpawn.Type == _lastSpawned.Type && newSpawn.Attack == _lastSpawned.Attack)
        {
            if (Random.value < _sameTypeRerollChance)
            {
                ActorSpawnSettings first = newSpawn;
                
                while(newSpawn == first)
                    newSpawn = _spawns[Random.Range(0, _spawns.Length)];
            }     
        }

        EnemyActor spawned = Create(newSpawn);
        spawned.SetPower(power);

        _lastSpawned = newSpawn;
    }

    private EnemyActor Create(ActorSpawnSettings spawn)
    {
        EnemyActor newEnemy = null;
        // get random direction,
        float angle = Random.Range(0.0f, Mathf.PI * 2);
        Vector3 dir = new Vector3(Mathf.Sin(angle), 0.0f, Mathf.Cos(angle));
        // If its a flying enemy get random y direction aswell,
        if (spawn.Type == EnemyType.Flying)
        {
            dir.y = Random.Range(0, 1);
            dir.Normalize();
        }
        // multiply by the random distance
        Vector3 pos = dir * spawn.Distance;
        // Instantiate
        newEnemy = Instantiate<EnemyActor>(spawn.Prefab, pos, Quaternion.identity);

        return newEnemy;
    }
}
