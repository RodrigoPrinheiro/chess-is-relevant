using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBooth : MonoBehaviour
{
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

        // Build the upgrade to be applied

        Active = false;
        _upgradeParticleSystem?.gameObject.SetActive(false);
    }

}
