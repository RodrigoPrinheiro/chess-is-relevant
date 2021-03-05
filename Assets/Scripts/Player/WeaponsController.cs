using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    [SerializeField] private Weapon _rightWeapon;
    [SerializeField] private Weapon _leftWeapon;
    private Actor _ownerActor;

    private void Awake() {
        _ownerActor = GetComponent<Actor>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ControlWeapon("Fire2", _rightWeapon);
        ControlWeapon("Fire1", _leftWeapon);
    }

    private void ControlWeapon(string button, Weapon weapon)
    {
        if (Input.GetButton(button))
        {
            weapon.Shoot(_ownerActor);
        }
    }
}
