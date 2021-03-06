using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : Actor
{
    [Header("Enemy Variables")]
    [SerializeField] protected AudioCue _deathSound;
    [SerializeField] private float _ragdollTime;
    [SerializeField] private ActiveEnemies _activeEnemies;
    [SerializeField] private GameObject _deathParticles;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        deathEvent += Die;
        _activeEnemies.AddNewEnemyActor(this);
    }
    private void Die(Actor source)
    {
        _deathSound.Play();
        _activeEnemies.RemoveEnemyActor(this);
        StartCoroutine(BecomeAFossil(source));
        
        if (!Dead()) StartCoroutine(BecomeAFossil(source));
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
        
        rb.AddExplosionForce(750f, transform.position + 
            (source.transform.position - transform.position).normalized * 5, 100f);

        yield return new WaitForSeconds(_ragdollTime);

        Instantiate(_deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    
}
