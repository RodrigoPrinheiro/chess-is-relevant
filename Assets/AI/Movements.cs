using System;
using UnityEngine;

public class Movements
{
    private readonly Actor _owner;
    private readonly Transform _player;
    private readonly float _minDistanceToPlayer;

    public Movements(Actor pos, Transform player, float minDist)
    {
        _owner = pos;
        _player = player;
        _minDistanceToPlayer = minDist;
    }

    private Vector3 GroundMovement()
    {
        if (IsAtMinDist())
        {
            return Vector3.zero;
        }

        Vector3 dir = (_player.position - _owner.transform.position).normalized;

        dir.y = 0;

        return dir;
    }
    private float _rotateTimer;
    private float _attackTimer;
    private readonly float _timeBetweenAttacks = 20f;
    private bool continueCircle;
    private Vector3 AirMovement()
    {
        _attackTimer += Time.deltaTime;
        _rotateTimer += Time.deltaTime * 0.2f;

        if (Vector3.Distance(_owner.transform.position, _player.position) < 1.5f)
        {
            _attackTimer = 0;
        }

        Vector3 dir;

        // Atack player
        if (_attackTimer >= _timeBetweenAttacks &&
            IsAtMinDist(_minDistanceToPlayer + 15f))
        {
            dir = (_player.position - _owner.transform.position).normalized * 5;
        }
        else
        {
            // Go up after the attack
            if (_owner.transform.position.y < 15)
            {
                dir = _owner.transform.forward;
                dir.y = 5f;
            }
            // Circle player if close
            else if (IsAtMinDist() || continueCircle)
            {
                continueCircle = IsAtMinDist(_minDistanceToPlayer + 15f);
                dir = new Vector3(Mathf.Sin(_rotateTimer), 0.2f, Mathf.Cos(_rotateTimer));
            }
            // Go to player
            else
            {
                dir = (_player.position - _owner.transform.position).normalized;
                dir.y = 0.2f;
            }
        }

        return dir;
    }

    private bool IsAtMinDist(float target = 0)
    {
        Vector3 player = _player.position;
        player.y = 0;
        Vector3 owner = _owner.transform.position;
        owner.y = 0;

        float compare = target == 0 ? _minDistanceToPlayer : target;

        return (Vector3.Distance(player, owner) <= compare);
    }

    public Func<Vector3> ChooseMovement(Movements mov, MovementType movType)
    {
        switch (movType)
        {
            case (MovementType.Ground):
                return new Func<Vector3>(mov.GroundMovement);

            case (MovementType.Air):
                return new Func<Vector3>(mov.AirMovement);

            default:
                return null;
        }
    }
}