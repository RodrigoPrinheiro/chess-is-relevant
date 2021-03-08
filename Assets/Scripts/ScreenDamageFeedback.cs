using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenDamageFeedback : MonoBehaviour
{
    [SerializeField] private Image _bloodImage;
    protected PlayerActor player;
    private float _baseEffectPercentage;

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
        
        // If the player is at 50% hp then let the effect stay on screen;
        if (player.HP < player.MaxHP / 2)
            _baseEffectPercentage += (percentageDmg / 2);
        
        _bloodImage.CrossFadeAlpha(_baseEffectPercentage + percentageDmg, 0f, false);
        _bloodImage.CrossFadeAlpha(_baseEffectPercentage, 1f, false);
        Debug.Log(player.HP);
    }
}
