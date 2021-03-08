using System;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [Header("Actor Variables")]
    [SerializeField] protected float _baseHP = 100f;

    public float HP { get; protected set; }
    public float MaxHP {get; protected set;}

    // Start is called before the first frame update
    protected virtual void Start()
    {
        HP = _baseHP;
        MaxHP = _baseHP;
    }

    public virtual void Damage(Actor source, float damage)
    {
        staticDamageEvent?.Invoke(source, this, damage);
        hitEvent?.Invoke(damage);
        ReduceHP(source, damage);
    }

    protected virtual void ReduceHP(Actor source, float damage)
    {
        HP = Mathf.Max(0, HP - damage);

        if (Dead())
        {
            staticActorDeathEvent?.Invoke(source, this);
            deathEvent?.Invoke(source);
        }
    }

    public virtual bool Dead()
    {
        return HP <= 0;
    }

    /// <summary>
    /// Takes in from whom the damage came from, who took the damage and the amount
    /// </summary>
    public static event Action<Actor, Actor, float> staticDamageEvent;

    public static event Action<Actor, Actor> staticActorDeathEvent;

    public event Action<Actor> deathEvent;
    public event Action<float> hitEvent;
}
