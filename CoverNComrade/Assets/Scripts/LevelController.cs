using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{
    public Transform[] spawnPositions;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;

        foreach (var item in spawnPositions)
        {
            Gizmos.DrawLine(item.transform.position, item.transform.position + Vector3.up * 20);
            Gizmos.DrawSphere(item.transform.position, 1);
        }
    }
}
