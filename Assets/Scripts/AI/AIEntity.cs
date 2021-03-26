using System;
using UnityEngine;

public class AIEntity : MonoBehaviour
{
    public ActorSpawnSettings Actor { get; set; }
    private Vector3 _direction;
    private Rigidbody _rb;
    private Actor _owner;

    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _owner = GetComponent<Actor>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 oldDir = _direction;

        _direction = Actor.aiValues.doMovement.Invoke(_owner.transform) * Actor.MoveSpeed;
        Actor.aiValues.doAttack?.Invoke(_owner);

        transform.LookAt(Vector3.Lerp(oldDir + transform.position, _direction + transform.position, 0.5f));
    }

    private void FixedUpdate()
    {
        _rb.velocity += _direction * Time.fixedDeltaTime;
    }
}