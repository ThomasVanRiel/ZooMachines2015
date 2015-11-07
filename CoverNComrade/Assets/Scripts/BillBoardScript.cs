using UnityEngine;
using System.Collections;

public class BillBoardScript : MonoBehaviour {

	void Update () {
        //transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position, Vector3.up);
        transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(0, 180, 0);
    }
}
