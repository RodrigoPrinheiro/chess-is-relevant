using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBar : MonoBehaviour
{
    private HealthBar _healthBar;
    private CanvasGroup _group;

    private void Awake() 
    {
        _group = GetComponent<CanvasGroup>();
        _healthBar = GetComponent<HealthBar>();
        HideBar();
    }
    private void OnEnable() 
    {
        Spawner.bossSpawnEvent += ShowBar;
        _healthBar.emptyBarEvent += HideBar;
    }
    
    private void OnDisable() 
    {
        Spawner.bossSpawnEvent -= ShowBar;
        _healthBar.emptyBarEvent -= HideBar;
    }

    private void ShowBar()
    {
        _group.alpha = 1f;
        _healthBar.UpdateHealth();
    }

    private void HideBar() => _group.alpha = 0f;
}
