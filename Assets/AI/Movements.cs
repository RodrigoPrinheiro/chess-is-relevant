using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movements
{
    private Transform _position;
    private Transform _player;

    public Movements(Transform pos, Transform player)
    {
        _position = pos;
        _player = player;
    }

    public Vector3 GroundMovement()
    {
        Vector3 dir = _player.position - _position.position;
        dir.y = 0;
        return dir.normalized;
    }

    public Vector3 AirMovement()
    {
        Vector3 dir = _player.position - _position.position;
        dir.y = 0;

        if (_position.position.y < 5)
        {
            dir.y = 1;
        }

        return dir.normalized;
    }

    public Vector3 MixedMovement()
    {
        return Vector3.zero;
    }
}
