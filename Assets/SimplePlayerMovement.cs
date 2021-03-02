using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector3 _input;
    [SerializeField] private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        _input = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        _input = _input.normalized;
    }

    private void FixedUpdate()
    {
        _rb.velocity += (_input * Time.fixedDeltaTime) * _speed;
    }
}
