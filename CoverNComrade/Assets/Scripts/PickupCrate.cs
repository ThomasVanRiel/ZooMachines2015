using UnityEngine;
using System.Collections;

public class PickupCrate : MonoBehaviour
{
    public Weapon[] Weapons;
    public GameObject PickupParticlesPrefab;

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
                GameObject p = Instantiate(PickupParticlesPrefab, scr.transform.position + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
                p.transform.SetParent(scr.transform);
                Destroy(p, 4);
                Destroy(gameObject);
            }
        }
    }
}
