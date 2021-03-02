using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks
{
    public Transform Position { get; }
    public Transform Player { get; }


    public Attacks(Transform pos, Transform player)
    {
        Position = pos;
        Player = player;
    }


    public void MeleeAttack()
    {
        throw new System.NotImplementedException();
    }

    public void RangedAttack()
    {
        throw new System.NotImplementedException();
    }
}
