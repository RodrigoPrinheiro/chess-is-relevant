using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody _rb;
    private float _aliveTime;
    [SerializeField] private float _maxLifeTime;
    [SerializeField] private float _damage;
    [SerializeField] private float _speed = 100;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Physics.SphereCast(transform.position + transform.forward, 1f, transform.forward, out RaycastHit hit, 2);

        _aliveTime += Time.deltaTime;
        if (_aliveTime >= _maxLifeTime || hit.collider != null)
        {
            if (hit.collider != null)
            {
                CheckForPlayer(hit.collider.gameObject);
            }
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = transform.forward * _speed;
    }

    private void CheckForPlayer(GameObject collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerActor _player))
        {
            _player.Damage(null, _damage);
            Destroy(gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + transform.forward, 1f);
    }
}
