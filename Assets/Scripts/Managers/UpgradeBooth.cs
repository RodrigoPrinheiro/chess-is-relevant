using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBooth : MonoBehaviour
{
    [Header("Upgrades")]
    [SerializeField] private WeaponUpgrade _rightWeaponUpgrade;
    [SerializeField] private WeaponUpgrade _leftWeaponUpgrade;
    [Header("Visual & Audio Elements")]
    [SerializeField] private ParticleSystem _upgradeParticleSystem;
    [SerializeField] private AudioCue _onPickUpCue;
    [SerializeField] private AudioCue _onEnableCue;
    public bool Active {get; private set;}

    private void Awake() 
    {
        _upgradeParticleSystem?.gameObject.SetActive(false);
        Active = false;
    }

    // Called when a new wave is thrown
    public void EnableUpgrade()
    {
        Active = true;

        // Visual & audio
        _onEnableCue?.Play();
        _upgradeParticleSystem?.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!Active) return;

        // If the collision is with the player
        if (other.TryGetComponent<PlayerActor>(out PlayerActor player))
        {
            // Give Upgrade
            GiveUpgrade(player);
        }
    }

    private void GiveUpgrade(PlayerActor player)
    {
        _onPickUpCue?.Play();
        // Get weapons
        WeaponsController playerWeapons = player.GetComponent<WeaponsController>();
        playerWeapons.Right.UpgradeWeapon(_rightWeaponUpgrade); 
        playerWeapons.Left.UpgradeWeapon(_leftWeaponUpgrade); 


        Active = false;
        _upgradeParticleSystem?.gameObject.SetActive(false);
    }

}
