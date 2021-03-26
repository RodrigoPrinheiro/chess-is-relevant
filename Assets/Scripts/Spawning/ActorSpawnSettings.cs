using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Settings")]
public class ActorSpawnSettings : ScriptableObject
{
    [SerializeField] public AISettings aiValues;
    [SerializeField] private EnemyActor _prefab;

    [SerializeField] private float _moveSpeed = 50f;
    [SerializeField] private float _powerLevelGate = 1f;
    [SerializeField] private float _distanceToOtherSpawn;

    [SerializeField, Range(0, 50f)] private float _minDistance;
    [SerializeField, Range(0, 100f)] private float _maxDistance = 50f;

    public float Distance => Random.Range(_minDistance, _maxDistance);
    public EnemyActor Prefab => _prefab;
    public float DistanceToOtherActor => _distanceToOtherSpawn;
    public float PowerLevelGate => _powerLevelGate;
    public float MoveSpeed => _moveSpeed;
}