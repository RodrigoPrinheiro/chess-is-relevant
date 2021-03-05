using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Attacks
{
    public Transform Position { get; }
    public Transform Player { get; }


    public Attacks(Transform pos, Transform player)
    {
        Position = pos;
        Player = player;
    }

    private void MeleeAttack()
    {
        throw new System.NotImplementedException();
    }

    private void RangedAttack()
    {
        throw new System.NotImplementedException();
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
