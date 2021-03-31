using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ModularVariables;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _targetImage;
    [SerializeField] private FloatReference _currentHP;
    [SerializeField] private FloatReference _maxHP;

    private void OnEnable() {
        Actor.StaticDamageEvent += UpdateHealth;
        Actor.StaticActorDeathEvent += UpdateHealth;
    }

    private void OnDisable() {
        Actor.StaticDamageEvent -= UpdateHealth;
        Actor.StaticActorDeathEvent -= UpdateHealth;
    }

    private void UpdateHealth(Actor src, Actor target, float dmg)
    {
        _targetImage.fillAmount = _currentHP / _maxHP;
        if (_currentHP <= 0)
        {
            emptyBarEvent?.Invoke();
        }
    }

    private void UpdateHealth(Actor src, Actor target)
    {
        _targetImage.fillAmount = _currentHP / _maxHP;
        if (_currentHP <= 0)
        {
            emptyBarEvent?.Invoke();
        }
    }

    public void UpdateHealth()
    {
        _targetImage.fillAmount = _currentHP / _maxHP;

        if (_currentHP <= 0)
        {
            emptyBarEvent?.Invoke();
        }
    }

    public event System.Action emptyBarEvent;
}
