using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody _rb;
    private float _aliveTime;
    [SerializeField] private float _maxLifeTime;
    [SerializeField] private float _damage;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_aliveTime >= _maxLifeTime)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = transform.forward * 10f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerActor _player))
        {
            _player.Damage(null, _damage);
            Destroy(gameObject);
        }
    }
}
