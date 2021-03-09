using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _worldRotation;
    [SerializeField] private Vector3 _objectRotation;
    [SerializeField] private float _heightDiference;
    [SerializeField] private float _speed;

    private Vector3 _initialPos;
    private Vector3 _newPos;
    private Vector3 _eulerAngles;
    private void Start() 
    {
        _initialPos = transform.position;
    }

    private void Update() 
    {
        _newPos = _initialPos + Vector3.up * Mathf.Sin(Time.time * _speed) * _heightDiference;
        
        transform.position = _newPos;
        transform.RotateAround(transform.position, Vector3.up, _worldRotation.y * Time.deltaTime);
        transform.RotateAround(transform.position, Vector3.right, _worldRotation.x * Time.deltaTime);
        transform.RotateAround(transform.position, Vector3.forward, _worldRotation.z * Time.deltaTime);
    }
}
