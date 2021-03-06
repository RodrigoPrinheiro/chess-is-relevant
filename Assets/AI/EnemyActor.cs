using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : Actor
{
    [Header("Enemy Variables")]
    [SerializeField] protected AudioCue _deathSound;
    [SerializeField] private float _ragdollTime;
    [SerializeField] private GameObject _deathParticles;

    private bool isDead;

    // Start is called before the first frame update
    protected override void Start()
    {
        HP = 100f;
        deathEvent += Die;
    }
    private void Die(Actor source)
    {
        _deathSound.Play();
        if (!isDead) StartCoroutine(BecomeAFossil(source));
    }

    private IEnumerator BecomeAFossil(Actor source)
    {
        isDead = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        Animator anim = GetComponentInChildren<Animator>();
        AIEntity ent = GetComponent<AIEntity>();

        ent.enabled = false;
        anim.enabled = false;
        rb.drag = 0;
        rb.angularDrag = 0;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        
        rb.AddExplosionForce(750f, transform.position + 
            (source.transform.position - transform.position).normalized * 5, 100f);

        yield return new WaitForSeconds(_ragdollTime);

        Instantiate(_deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    
}
