using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenDamageFeedback : MonoBehaviour
{
    [SerializeField] private Image _bloodImage;
    protected PlayerActor player;

    protected virtual void Awake()
    {
        player = FindObjectOfType<PlayerActor>();
        _bloodImage.CrossFadeAlpha(0.0f, 0f, false);
    } 

    private void OnEnable()
    {
        player.hitEvent += Effect;
    }

    private void OnDisable() 
    {
        player.hitEvent -= Effect;
    }

    public virtual void Effect(float damage)
    {
        float percentageDmg = damage / player.MaxHP;
        _bloodImage.CrossFadeAlpha(percentageDmg, 0f, false);
        _bloodImage.CrossFadeAlpha(0.0f, 1f, false);
    }
}
