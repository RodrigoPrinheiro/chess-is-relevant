using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private Transform _playerTransform;

    private void Awake() 
    {
        _playerTransform = FindObjectOfType<PlayerActor>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
