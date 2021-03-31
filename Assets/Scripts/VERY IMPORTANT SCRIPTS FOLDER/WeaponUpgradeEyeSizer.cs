using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeEyeSizer : MonoBehaviour
{
    // This refers to scale of the object in the 3 axis
    [SerializeField] private float _maxSize = 0.6f;
    [SerializeField] private int _upgradeSizeCap = 2;
    [SerializeField] private Weapon _weapon;
    private int _weaponUpgradesCount;
    private float _percentage;

    private Vector3 _startScale;
    private Vector3 _endScale;
    private void Awake() 
    {
        _startScale = transform.localScale;
        _endScale = new Vector3(_maxSize, _maxSize, _maxSize);
    }

    private void OnEnable() {
        _weapon.weaponUpgradeEvent += ChangeSize;
    }

    private void OnDisable() {
        _weapon.weaponUpgradeEvent -= ChangeSize;
        
    }

    private void ChangeSize()
    {
        _weaponUpgradesCount++;
        _percentage = (float)_weaponUpgradesCount / (float)_upgradeSizeCap;
        _percentage = Mathf.Min(_percentage, 1);

        transform.localScale = Vector3.Lerp(_startScale, _endScale, _percentage);
    }
}
