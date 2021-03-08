using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _bulletHitPrefab;
    private Weapon _weapon;
    private Pool<ParticleSystem> _bulletHitPool;

    private void Awake() 
    {
        _bulletHitPool = new Pool<ParticleSystem>(_bulletHitPrefab);
        _bulletHitPool.Initialize(8);
        _bulletHitPool.SetRequestCondition((x) => !x.isPlaying);
        _weapon = GetComponent<Weapon>();
    }

    private void OnEnable() {
        _weapon.bulletHit += BulletHitParticles;
    }

    private void OnDisable() {
        _weapon.bulletHit -= BulletHitParticles;
    }

    private void BulletHitParticles(RaycastHit hit)
    {
        ParticleSystem bulletHit = _bulletHitPool.Request();
        bulletHit.transform.position = hit.point;
        bulletHit.transform.forward = hit.normal;

        bulletHit.Play();
    }
}
