using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIEntity : MonoBehaviour
{
    private float _speed;
    private Action _attack;
    private Func<Vector3> _movement;
    private Transform _player;
    private Rigidbody _rb;
    private Vector3 _direction;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();

        SetAttackAndMovementMode(AttackType.Melee, MovementType.Ground, 50);
    }

    public void SetAttackAndMovementMode(AttackType attkType, MovementType movType, float speed)
    {
        Movements mov = new Movements(transform, _player);
        Attacks attk = new Attacks(transform, _player);

        _attack = attk.ChooseAttack(attk, attkType);
        _movement = mov.ChooseMovement(mov, movType);
        _speed = speed;
    }

    // Update is called once per frame
    private void Update()
    {
        _direction = _movement.Invoke() * _speed;
        transform.LookAt(_direction);
    }
    private void FixedUpdate()
    {
        _rb.velocity += _direction * Time.fixedDeltaTime;
    }
}
