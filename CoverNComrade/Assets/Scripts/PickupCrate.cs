using UnityEngine;
using System.Collections;

public class PickupCrate : MonoBehaviour
{
    public Weapon[] Weapons;
    public GameObject PickupParticlesPrefab;

    public bool SingleUse = false;
    public float Cooldown = 30;
    public Transform Model = null;

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

                if (SingleUse)
                    Destroy(gameObject);
                else
                {
                    Model.gameObject.SetActive(false);
                    GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(TickCooldown());
                }
            }
        }
    }

    IEnumerator TickCooldown()
    {
        yield return new WaitForSeconds(Cooldown);
        Model.gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;
    }
}
