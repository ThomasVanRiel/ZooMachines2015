using UnityEngine;
using System.Collections;

public interface IInputReceiver
{
    int PlayerID { get; set; }

    float GetMouseX();
    float GetMouseY();
    bool GetMouseButton(int button);
    bool GetMouseButtonDown(int button);
    bool GetMouseButtonUp(int button);
}
