using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    const float SPREAD_PER_PROJECTILE = 0.015f;
    const float MAX_BPS = 30f;
    const float MIN_BPS = 0.2f;
    [SerializeField] private Transform _head;
    [SerializeField] private Camera _fpsCamera;

    [Header("Shoot settings")]
    [SerializeField] private LayerMask _hitMask;

    [SerializeField] private Transform _muzzleTransform;
    [SerializeField] private float _weaponDamage = 10f;
    [SerializeField] private float _weaponRange = 30f;
    [SerializeField] private float _weaponBPS = 2;
    [SerializeField, Range(0, 1000)] private int _projectilesOverride = 0;
    [SerializeField, Range(0, 1)] private float _recoilXAxis;
    [SerializeField, Range(0, 1)] private float _recoilYAxis;
    [SerializeField, Range(5, 50)] private float _kickDecay = 25f;
    [SerializeField] private AudioCue _shootSound;

    [Header("Bullet line settings")]
    [SerializeField] private LineRenderer _bulletLine;

    [SerializeField] private float _visibleTime = 0.5f;
    [SerializeField] private Gradient _lineGradient;

    public float WeaponDamage
    {
        get => _weaponDamage;
        set => _weaponDamage = value;
    }

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
    private Vector3 _originalPos;
    private Vector2 _recoilRemaining;
    private float _spread;
    private int _projectiles;
    private Pool<LineRenderer> _bulletLinePool;
    public Vector3 OriginalPos => _originalPos;

    private void Awake()
    {
        // Setup
        _projectiles = _projectilesOverride != 0 ? _projectilesOverride : 1;
        _bulletLineScreenTime = new WaitForSeconds(_visibleTime);
        _originalPos = transform.localPosition;

        // Bullet line
        _bulletLinePool = new Pool<LineRenderer>(_bulletLine, transform);
        _bulletLinePool.SetRequestCondition((l) => !l.enabled);
        _bulletLinePool.Initialize(_projectiles);

        // Weapon settings Setup
        SetupWeapon();
    }

    private void Update()
    {
        if (!CanShoot)
        {
            _cooldownTimer -= Time.deltaTime;
        }

        // Recoil
        _head.parent.Rotate(Vector3.up, _recoilRemaining.x);
        _head.Rotate(Vector3.right, _recoilRemaining.y, Space.Self);

        transform.Translate(-transform.forward * _recoilRemaining.y, Space.Self);
        transform.Translate(transform.up * _recoilRemaining.x, Space.Self);
        _recoilRemaining *= (1 - _kickDecay * Time.deltaTime);
    }

    public void Shoot(Actor owner)
    {
        if (!CanShoot)
        {
            // Shoot Failed
            shootFailedEvent?.Invoke();
            return;
        }

        _recoilRemaining.x = UnityEngine.Random.Range(-_recoilXAxis, _recoilXAxis);
        _recoilRemaining.y = -UnityEngine.Random.Range(0, _recoilYAxis);

        shootEvent?.Invoke();

        // Get the default ray and record it to a variable
        Ray ray = _fpsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Ray defaultRay = ray;

        // For each projectile in the weapon, give spread and shoot it using the new ray
        for (int i = 0; i < _projectiles; i++)
        {
            ray = defaultRay;

            ray.direction += new Vector3(
            UnityEngine.Random.Range(-_spread, _spread),
            UnityEngine.Random.Range(-_spread, _spread),
            UnityEngine.Random.Range(-_spread, _spread));

            // Shoot weapon (what it had before this new adition basically)
            ShootRay(ray, owner);
        }

        CanShoot = false;
    }

    private void ShootRay(Ray ray, Actor owner)
    {

        Debug.DrawRay(ray.origin, ray.direction * _weaponRange, Color.green, 0.6f);

        if (Physics.Raycast(ray, out RaycastHit hit, _weaponRange, ~_hitMask))
        {
            if (hit.transform.TryGetComponent<Actor>(out Actor enemy))
            {
                enemy.Damage(owner, WeaponDamage);
                bulletHit?.Invoke(hit);
            }

            ShowBulletLine(hit.point);
        }
        else
        {
            ShowBulletLine(ray.origin + ray.direction * _weaponRange);
        }
    }

    private void SetupWeapon()
    {
        _cooldown = 1 / _weaponBPS;
        _spread = _projectiles * SPREAD_PER_PROJECTILE;
    }

    public void UpgradeWeapon(WeaponUpgrade upgrade)
    {
        _weaponRange += upgrade.range;
        _weaponDamage += upgrade.damage;
        _weaponBPS += upgrade.bps;
        _projectiles += upgrade.projectiles;

        _weaponBPS = Mathf.Clamp(_weaponBPS, MIN_BPS, MAX_BPS);
        _weaponRange = Mathf.Max(_weaponRange, 5f);
        _weaponDamage = Mathf.Max(_weaponDamage, 1f);
        _projectiles = Mathf.Max(_projectiles, 1);
        
        SetupWeapon();
    }

    private void ShowBulletLine(Vector3 hitPoint)
    {
        LineRenderer line = _bulletLinePool.Request();
        line.enabled = true;
        // Position
        line.positionCount = 2;
        line.SetPosition(0, _muzzleTransform.position);
        line.SetPosition(1, hitPoint);

        // Color
        line.colorGradient = _lineGradient;

        StartCoroutine(BulletLine(line));
    }

    private IEnumerator BulletLine(LineRenderer line)
    {
        yield return _bulletLineScreenTime;
        line.enabled = false;
    }

    public UnityEngine.Events.UnityEvent shootEvent;
    public Action<RaycastHit> bulletHit;
    public Action shootFailedEvent;
}

[System.Serializable]
public struct WeaponUpgrade
{
    public float damage;
    public float range;
    public float bps;
    public int projectiles;
}