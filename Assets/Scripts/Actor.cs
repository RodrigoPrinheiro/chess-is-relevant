using System;
using UnityEngine;
using ModularVariables;

public class Actor : MonoBehaviour
{
    [Header("Actor Variables")]
    [SerializeField] protected float _baseHP = 100f;
    private FloatReference _healthReference;
    private FloatReference _maxHealthReference;

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
        StaticDamageEvent?.Invoke(source, this, damage);
        HitEvent?.Invoke(damage);
        ReduceHP(source, damage);
    }

    protected virtual void ReduceHP(Actor source, float damage)
    {
        HP = Mathf.Max(0, HP - damage);
        if (_healthReference != null) _healthReference.Value = HP;

        if (Dead())
        {
            StaticActorDeathEvent?.Invoke(source, this);
            DeathEvent?.Invoke(source);
        }
    }

    public virtual bool Dead()
    {
        return HP <= 0;
    }

    public void SetHealthReference(FloatReference hp, FloatReference maxHP)
    {
        _healthReference = hp;
        _healthReference.Value = HP;
        _maxHealthReference = maxHP;
        _maxHealthReference.Value = MaxHP;
    }

    /// <summary>
    /// Takes in from whom the damage came from, who took the damage and the amount
    /// </summary>
    public static event Action<Actor, Actor, float> StaticDamageEvent;

    public static event Action<Actor, Actor> StaticActorDeathEvent;

    public event Action<Actor> DeathEvent;
    public event Action<float> HitEvent;
}
