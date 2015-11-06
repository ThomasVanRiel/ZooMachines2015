using UnityEngine;
using System.Collections;

public class PickupCrate : MonoBehaviour
{
    public Weapon[] Weapons;

    void Start()
    {
        if (Weapons.Length <= 0)
            Destroy(gameObject);
    }

    Weapon GetRandomWeapon()
    {
        if (Weapons.Length == 1)
            return Weapons[0];
        else
        {
            int r = Random.Range(0, Weapons.Length);
            return Weapons[r];
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c != null)
        {
            var scr = c.gameObject.GetComponent<PlayerController>();
            if (scr != null)
            {
                scr.SetWeapon(GetRandomWeapon());
                Destroy(gameObject);
            }
        }
    }
}
