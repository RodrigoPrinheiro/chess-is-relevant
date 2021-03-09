﻿using System;
using UnityEngine;

public class AIEntity : MonoBehaviour
{
    private float _speed;
    private Action _attack;
    private Func<Vector3> _movement;
    private Vector3 _direction;
    private Rigidbody _rb;
    private Actor _owner;
    private Transform _player;

    public void SetAttackAndMovementMode(AttackType attkType, MovementType movType, float speed)
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _owner = GetComponent<Actor>();

        float attkrange = movType == MovementType.Ground ? 1.5f : 4f;
        float minDist = movType == MovementType.Ground ? 1f : 15f;

        Movements mov = new Movements(_owner, _player, minDist);
        Attacks attk = new Attacks(_owner, _player, 10, attkrange);

        _attack = attk.ChooseAttack(attk, attkType);
        _movement = mov.ChooseMovement(mov, movType);
        _speed = speed;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 oldDir = _direction;

        _direction = _movement.Invoke() * _speed;
        _attack?.Invoke();

        transform.LookAt(Vector3.Lerp(oldDir, _direction, 0.5f));
    }

    private void FixedUpdate()
    {
        _rb.velocity += _direction * Time.fixedDeltaTime;
    }
}