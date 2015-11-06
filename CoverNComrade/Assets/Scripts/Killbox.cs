using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Killbox : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if (c != null)
        {
            var scr = c.gameObject.GetComponent<PlayerController>();
            if (scr != null)
            {
                scr.TakeDamage(int.MaxValue);
            }
        }
    }

}
