using UnityEngine;

public class PlayerActor : Actor
{
    [SerializeField] private ModularVariables.FloatReference _playerHpReference;
    [SerializeField] private ModularVariables.FloatReference _playerMaxHP;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        if (_playerHpReference != null && _playerMaxHP != null)
            SetHealthReference(_playerHpReference, _playerMaxHP);
    }

    private void OnEnable() 
    {
        
    }

    private void OnDisable() 
    {
    }

    // Update is called once per frame
    private void Update()
    {

    }
}