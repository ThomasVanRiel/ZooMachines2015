﻿using UnityEngine;
using System.Collections;

public class DisableCursor : MonoBehaviour
{
	void Update ()
    {
#if UNITY_EDITOR_WIN
	    Cursor.lockState = CursorLockMode.Locked;
#endif
	    Cursor.visible = false;
	}
}
