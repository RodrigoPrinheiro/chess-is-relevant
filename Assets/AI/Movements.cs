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
        if (isAtMinDist())
        {
            return Vector3.zero;
        }

        Vector3 dir = (_player.position - _owner.transform.position).normalized;

        dir.y = 0;

        return dir;
    }

    private Vector3 AirMovement()
    {
        if (isAtMinDist())
        {
            return new Vector3(0, -0.5f, 0);
        }

        Vector3 dir = (_player.position - _owner.transform.position).normalized;

        if (_owner.transform.position.y < 5)
        {
            dir.y = 1;
        }

        return dir;
    }

    private bool isAtMinDist()
    {
        Vector3 player = _player.position;
        player.y = 0;
        Vector3 owner = _owner.transform.position;
        owner.y = 0;

        return (Vector3.Distance(player, owner) <= _minDistanceToPlayer);
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