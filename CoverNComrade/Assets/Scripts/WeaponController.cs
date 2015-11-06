using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public Transform WeaponHold;
    public Weapon StartingWeapon = null;

    private Weapon _equippedWeapon = null;

    void Start()
    {
        if (StartingWeapon != null)
        {
            EquipWeapon(StartingWeapon);
        }
    }

    public void EquipWeapon(Weapon weaponToEquip)
    {
        if (_equippedWeapon != null)
        {
            DestroyImmediate(_equippedWeapon);
        }

        _equippedWeapon = Instantiate(weaponToEquip, WeaponHold.position, WeaponHold.rotation) as Weapon;
        _equippedWeapon.transform.parent = WeaponHold;

    }

    public void OnTriggerHold()
    {
        if (_equippedWeapon != null)
        {
            _equippedWeapon.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (_equippedWeapon != null)
        {
            _equippedWeapon.OnTriggerRelease();
        }
    }
}
