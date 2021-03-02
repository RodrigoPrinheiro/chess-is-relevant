using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIEntity : MonoBehaviour
{
    [SerializeField] private float _speed;

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

        SetAttackAndMovementMode(AttackType.Melee, MovementType.Ground);
    }

    public void SetAttackAndMovementMode(AttackType attkType, MovementType movType)
    {
        Movements mov = new Movements(transform, _player);
        Attacks attk = new Attacks(transform, _player);

        _attack = ChooseAttack(attk, attkType);
        _movement = ChooseMovement(mov, movType);
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

    private Action ChooseAttack(Attacks attk, AttackType attkType)
    {
        switch (attkType)
        {
            case (AttackType.Melee):
                return new Action(attk.MeleeAttack);
            case (AttackType.Ranged):
                return new Action(attk.RangedAttack);
        }
        return null;
    }
    private Func<Vector3> ChooseMovement(Movements mov, MovementType movType)
    {
        switch (movType)
        {
            case (MovementType.Ground):
                return new Func<Vector3>(mov.GroundMovement);
            case (MovementType.Air):
                return new Func<Vector3>(mov.AirMovement);
        }
        return null;
    }
}
