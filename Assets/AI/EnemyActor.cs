using System.Collections;
using UnityEngine;

public class EnemyActor : Actor
{
    [Header("Enemy Variables")]
    [SerializeField] protected AudioCue _deathSound;

    [SerializeField] private float _ragdollTime;
    [SerializeField] private ActiveEnemies _activeEnemies;
    [SerializeField] private GameObject _deathParticles;

    public float PowerLevel { get; protected set; }
    private bool _ragdolled;
    private Rigidbody _rb;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _activeEnemies.AddNewEnemyActor(this);
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() 
    {
        deathEvent += Die;
    }

    private void OnDisable() {
        deathEvent -= Die;
    }
    private void Die(Actor source)
    {
        _deathSound.Play();
        _activeEnemies.RemoveEnemyActor(this);
        StartCoroutine(BecomeAFossil(source));

        if (!_ragdolled) StartCoroutine(BecomeAFossil(source));
    }

    // Refreshes HP
    public void SetPower(float power)
    {
        PowerLevel = power + 1;
        HP = _baseHP * PowerLevel;
        MaxHP = HP;
    }

    private IEnumerator BecomeAFossil(Actor source)
    {
        Animator anim = GetComponentInChildren<Animator>();
        AIEntity ent = GetComponent<AIEntity>();

        ent.enabled = false;
        anim.enabled = false;
        _rb.drag = 0;
        _rb.angularDrag = 0;
        _rb.useGravity = true;
        _rb.constraints = RigidbodyConstraints.None;

        _ragdolled = true;

        _rb.AddExplosionForce(750f, transform.position +
            (source.transform.position - transform.position).normalized * 5, 100f);

        yield return new WaitForSeconds(_ragdollTime);

        if (_deathParticles)
            Instantiate(_deathParticles, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}