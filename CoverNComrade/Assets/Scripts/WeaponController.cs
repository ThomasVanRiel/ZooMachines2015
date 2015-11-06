using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public Transform WeaponHold;
    public Weapon StartingWeapon = null;

    public Color PlayerColor = Color.blue;

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
            Destroy(_equippedWeapon.gameObject);
        }

        _equippedWeapon = null;
        _equippedWeapon = Instantiate(weaponToEquip, WeaponHold.position, WeaponHold.rotation) as Weapon;
        _equippedWeapon.transform.parent = WeaponHold;
        _equippedWeapon.SetTrailColor(PlayerColor);
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
