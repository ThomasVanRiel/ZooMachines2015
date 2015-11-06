﻿using UnityEngine;
using System.Collections;

public interface IInputReceiver
{
    int PlayerID { get; set; }

    Vector3 GetMousePosition();
    float GetMouseX();
    float GetMouseY();
    float GetMouseScroll();
    bool GetMouseButton(int button);
    bool GetMouseButtonDown(int button);
    bool GetMouseButtonUp(int button);
}
