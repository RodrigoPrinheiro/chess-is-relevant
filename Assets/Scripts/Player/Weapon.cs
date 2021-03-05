using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private Camera _fpsCamera;

    [Header("Shoot settings")]
    [SerializeField] private LayerMask _hitMask;
    [SerializeField] private Transform _muzzleTransform;
    [SerializeField] private float _weaponRange = 30f;
    [SerializeField] private float _weaponBPS = 2;
    [SerializeField] private AudioClip _shootSound;
    [Header("Bullet line settings")]
    [SerializeField] private LineRenderer _bulletLine;
    [SerializeField] private float _visibleTime = 0.5f;
    [SerializeField] private Gradient _lineGradient;
    public float WeaponDamage {get; set;}
    public bool CanShoot
    {
        get => _cooldownTimer <= 0;
        set
        {
            if (!value)
                _cooldownTimer = _cooldown;
            else
                _cooldownTimer = 0;
        }
    }
    
    private float _cooldown;
    private float _cooldownTimer;
    private WaitForSeconds _bulletLineScreenTime;

    private void Awake() 
    {
        WeaponDamage = 10f;
        _cooldown = 1 / _weaponBPS;
        _bulletLineScreenTime = new WaitForSeconds(_visibleTime);
        _bulletLine.enabled = false;
    }

    private void Update() 
    {
        if (!CanShoot)
        {
            _cooldownTimer -= Time.deltaTime;
        }    
    }

    public void Shoot(Actor owner)
    {
        if (!CanShoot)
        {
            // Shoot Failed
            shootFailedEvent?.Invoke();
            return;
        }

        Debug.Log("Shooting");

        shootEvent?.Invoke();
        Ray ray = _fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * _weaponRange, Color.green, 0.6f);
        if (Physics.Raycast(ray, out hit, _weaponRange, ~_hitMask))
        {
            Actor enemy;
            if (hit.transform.TryGetComponent<Actor>(out enemy))
            {
                enemy.Damage(owner, WeaponDamage);
            }

            ShowBulletLine(hit.point);
        }
        else
        {
            ShowBulletLine(ray.origin + ray.direction * _weaponRange);
        }

        CanShoot = false;
    }

    private void ShowBulletLine(Vector3 hitPoint)
    {
        _bulletLine.enabled = true;
        // Position
        _bulletLine.positionCount = 2;
        _bulletLine.SetPosition(0, _muzzleTransform.position);
        _bulletLine.SetPosition(1, hitPoint);

        // Color
        _bulletLine.colorGradient = _lineGradient;

        StartCoroutine(BulletLine());
    }
    private IEnumerator BulletLine()
    {
        yield return _bulletLineScreenTime;
        _bulletLine.enabled = false;
    }

    public Action shootEvent;
    public Action shootFailedEvent;
}