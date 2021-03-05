using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : Actor
{
    public float HP { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        HP = 100f;
        damageEvent += ReduceHP;
        actorDeathEvent += Die;
    }
    private void Die(Actor actor)
    {
        StartCoroutine(BecomeAFossil(actor));
    }

    private IEnumerator BecomeAFossil(Actor from)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Animator anim = GetComponentInChildren<Animator>();
        AIEntity ent = GetComponent<AIEntity>();

        ent.enabled = false;
        anim.enabled = false;
        rb.drag = 0;
        rb.angularDrag = 0;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        
        rb.AddExplosionForce(1000f, transform.position + 
            (from.transform.position - transform.position).normalized * 5, 100f);

        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    private void ReduceHP(Actor from, Actor act, float damage)
    {
        HP = Mathf.Max(0, HP - damage);

        if (HP <= 0)
        {
            actorDeathEvent?.Invoke(from);
        }
    }
}
