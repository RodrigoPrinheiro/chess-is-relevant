using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : Actor
{
    [Header("Enemy Variables")]
    [SerializeField] protected AudioCue _deathSound;
    // Start is called before the first frame update
    protected override void Start()
    {
        HP = 100f;
        deathEvent += Die;
    }
    private void Die(Actor source)
    {
        _deathSound.Play();
        StartCoroutine(BecomeAFossil(source));
    }

    private IEnumerator BecomeAFossil(Actor source)
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
            (source.transform.position - transform.position).normalized * 5, 100f);

        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }

    
}
