using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AI vals", menuName = "AI Settings/New settings")]
public class AISettings : ScriptableObject
{

    [Tooltip("Movement Settings")]
    [SerializeField] private float _minDistanceToPlayer;
    [SerializeField] private MovementType movType;
    [Space]
    [Tooltip("Attack Settings")]
    [SerializeField] private AttackType _attackType;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _timeBetweenAttacks = 10f;

    public AttackType AttackType { get => _attackType; }
    public MovementType MovementType { get => movType; }

    public Action<Actor> doAttack;
    public Func<Transform, Vector3> doMovement;

    private Transform _player;
    private float _rotateTimer;
    private float _attackTimer;
    private bool continueCircle;

    Vector3 lastPpos;

    private void OnValidate()
    {
        switch (movType)
        {
            case (MovementType.Ground): doMovement = new Func<Transform, Vector3>(GroundMovement); break;
            case (MovementType.Air): doMovement = new Func<Transform, Vector3>(AirMovement); break;
            default: doMovement = null; break;
        };

        switch (_attackType)
        {
            case (AttackType.Melee): doAttack = new Action<Actor>(MeleeAttack); break;
            case (AttackType.Ranged): doAttack = new Action<Actor>(RangedAttack); break;
            default: doAttack = null; break;
        };
    }
    private void OnEnable()
    {
        switch (movType)
        {
            case (MovementType.Ground): doMovement = new Func<Transform, Vector3>(GroundMovement); break;
            case (MovementType.Air): doMovement = new Func<Transform, Vector3>(AirMovement); break;
            default: doMovement = null; break;
        };

        switch (_attackType)
        {
            case (AttackType.Melee): doAttack = new Action<Actor>(MeleeAttack); break;
            case (AttackType.Ranged): doAttack = new Action<Actor>(RangedAttack); break;
            default: doAttack = null; break;
        };
    }
    public void Init(Transform pPos)
    {
        _player = pPos;
        
        if (_player)
            lastPpos = _player.position;
    }

    private Vector3 GroundMovement(Transform owner)
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        Vector3 dir = (_player.position - owner.position).normalized;

        dir.y = 0;

        if (IsAtMinDist(owner))
        {
            return dir / 10;
        }

        return dir;
    }

    private Vector3 AirMovement(Transform owner)
    {
        _rotateTimer += Time.deltaTime * 0.2f;

        Vector3 dir;

        // Attack player
        if (AttackType != AttackType.Ranged &&
            _attackTimer >= _timeBetweenAttacks &&
            IsAtMinDist(owner, _minDistanceToPlayer + 15f))
        {
            dir = (_player.position - owner.position).normalized * 3;
        }
        else
        {
            // Go up after the AttackType
            if (owner.position.y < 15)
            {
                dir = owner.forward;
                dir.y = 5f;
            }
            // Circle player if closes
            else if (IsAtMinDist(owner) || continueCircle)
            {
                continueCircle = IsAtMinDist(owner, _minDistanceToPlayer + 15f);
                dir = new Vector3(Mathf.Sin(_rotateTimer), 0.2f, Mathf.Cos(_rotateTimer));
            }
            // Go to player
            else
            {
                dir = (_player.position - owner.position).normalized;
                dir.y = 0.2f;
            }
        }

        return dir;
    }

    private bool IsAtMinDist(Transform owner, float target = 0)
    {
        Vector3 player = _player.position;
        player.y = 0;
        Vector3 ai = owner.position;
        ai.y = 0;

        float compare = target == 0 ? _minDistanceToPlayer : target;

        return (Vector3.Distance(player, ai) <= compare);
    }
    private void MeleeAttack(Actor owner)
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer < _timeBetweenAttacks) return;

        Collider[] cols = Physics.OverlapSphere(owner.transform.position +
            owner.transform.forward, _attackRange);

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.TryGetComponent(out Actor _player))
            {
                Debug.Log("Hit player");
                _player.Damage(owner, _damage);
                _attackTimer = 0;
                break;
            }
        }
    }
    private void RangedAttack(Actor owner)
    {

        _attackTimer += Time.deltaTime;
        if (_attackTimer < _timeBetweenAttacks) return;
        _attackTimer = 0;

        Vector3 projectilePos = owner.transform.position;
        projectilePos.y += 1f;

        Vector3 dir = _player.position - owner.transform.position;
        Vector3 offset = _player.position - lastPpos;
        float compensation = Vector3.Distance(_player.position, owner.transform.position);

        GameObject.Instantiate(bullet, projectilePos,
            Quaternion.LookRotation(dir + (offset ), Vector3.up));

        if (_player.hasChanged)
        {
            lastPpos = _player.position;
        }
    }
}