using UnityEngine;
using System.Collections;

public class DisableCursor : MonoBehaviour
{
	void Update ()
    {
	    Cursor.lockState = CursorLockMode.Locked;
	    Cursor.visible = false;
	}
}
