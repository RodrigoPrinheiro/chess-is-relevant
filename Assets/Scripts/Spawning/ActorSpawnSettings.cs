using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "Enemy Settings")]
public class ActorSpawnSettings : ScriptableObject
{
    [SerializeField] private EnemyType _type;
    [SerializeField] private AttackType _attackType;
    [SerializeField] private float _powerLevelGate = 1f;
    [SerializeField] private EnemyActor _prefab;
    [SerializeField] private float _distanceToOtherSpawn;
    [SerializeField, Range(0, 50f)] private float _minDistance;
    [SerializeField, Range(0, 100f)] private float _maxDistance = 50f;

    public float Distance => Random.Range(_minDistance, _maxDistance);
    public EnemyActor Prefab => _prefab;
    public float DistanceToOtherActor => _distanceToOtherSpawn;
    public float PowerLevelGate => _powerLevelGate;
    public EnemyType Type => _type;
    public AttackType Attack => _attackType;
}

public enum EnemyType
{
    Ground,
    Flying
}