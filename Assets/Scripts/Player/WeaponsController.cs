﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    const float STEP_TIME = 0.2f;
    [SerializeField] private Weapon _rightWeapon;
    [SerializeField] private Weapon _leftWeapon;
    [SerializeField] private Vector2 _gunBob;
    private Actor _ownerActor;
    private Vector2 _currentGunBob;
    private float _stepTimeCounter;
    private void Awake()
    {
        _ownerActor = GetComponent<Actor>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ControlWeapon("Fire2", _rightWeapon);
        ControlWeapon("Fire1", _leftWeapon);

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input.magnitude > 0.01f)
            _stepTimeCounter += Time.deltaTime * input.normalized.sqrMagnitude;
        else if (Vector3.Distance(_rightWeapon.transform.localPosition, _rightWeapon.OriginalPos) > 0.02f)
        {
            _stepTimeCounter += Time.deltaTime * 1f;
        }
        
        GunBob(_rightWeapon.transform, _rightWeapon.OriginalPos);
        GunBob(_leftWeapon.transform, _leftWeapon.OriginalPos);
    }

    public void GunBob(Transform gunTransform, Vector3 originalPos)
    {
        _currentGunBob.Set(Mathf.Sin(_stepTimeCounter / STEP_TIME) * _gunBob.x, Mathf.Cos((_stepTimeCounter / STEP_TIME)* 2) * _gunBob.y);
        gunTransform.localPosition = new Vector3(_currentGunBob.x + originalPos.x,
            _currentGunBob.y + originalPos.y, originalPos.z);
    }

    private void ControlWeapon(string button, Weapon weapon)
    {
        if (Input.GetButton(button))
        {
            weapon.Shoot(_ownerActor);
        }
    }
}