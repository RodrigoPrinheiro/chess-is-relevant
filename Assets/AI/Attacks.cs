using System;
using UnityEngine;

public class Attacks
{
    private readonly Actor _owner;
    private readonly Transform _player;

    private readonly float _damage;
    private readonly float _attackRange;

    public Attacks(Actor pos, Transform player, float damage, float attackRange)
    {
        _owner = pos;
        _player = player;
        _damage = damage;
        _attackRange = attackRange;
    }

    private void MeleeAttack()
    {
        Collider[] cols = Physics.OverlapSphere(_owner.transform.position + _owner.transform.forward, _attackRange);

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.TryGetComponent(out Actor _player))
            {
                Debug.Log("Hit player");
                _player.Damage(_owner, _damage);
                break;
            }
        }
    }

    private void RangedAttack()
    {
        Vector3 dir = _owner.transform.position - _player.position;
    }

    public Action ChooseAttack(Attacks attk, AttackType attkType)
    {
        switch (attkType)
        {
            case (AttackType.Melee):
                return new Action(attk.MeleeAttack);

            case (AttackType.Ranged):
                return new Action(attk.RangedAttack);

            default: return null;
        }
    }
}