using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _sameTypeRerollChance;
    [SerializeField] private ActorSpawnSettings[] _spawns;
    [SerializeField] private Vector3 _arenaDimensions;
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
        if (spawn.Type == MovementType.Air)
        {
            dir.y = Random.Range(0, 1);
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
        ai.SetAttackAndMovementMode(spawn.Attack, spawn.Type, 50);

        return newEnemy;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 center = Vector3.zero + Vector3.up * (_arenaDimensions.y / 2);
        Gizmos.DrawWireCube(center, _arenaDimensions);
    }
}