using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TeamController))]
public class WeaponController : MonoBehaviour
{
    public Transform WeaponHold;
    public Weapon StartingWeapon = null;

    //[HideInInspector]
    //public Color TeamColor = Color.magenta;

    private Weapon _equippedWeapon = null;
    [HideInInspector]
    public TeamController _teamController = null;

    void Start()
    {
        _teamController = gameObject.GetComponent<TeamController>();

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

        _equippedWeapon = Instantiate(weaponToEquip, WeaponHold.position, WeaponHold.rotation) as Weapon;
        _equippedWeapon.transform.parent = WeaponHold;
        _equippedWeapon.SetWeaponControllerReference(this);
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
