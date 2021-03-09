using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBooth : MonoBehaviour
{
    private WaveManager _waveManager;
    public bool Active {get; private set;}

    private void Awake() {
        // Wave Manager for event
        _waveManager = FindObjectOfType<WaveManager>();
    }
    
    private void OnEnable() 
    {
        _waveManager.waveEndEvent += NewUpgrade;
    }

    private void OnDisable() 
    {
        _waveManager.waveEndEvent -= NewUpgrade;
    }


    // Called when a new wave is thrown
    public void NewUpgrade(int wave)
    {
        
    }

}
