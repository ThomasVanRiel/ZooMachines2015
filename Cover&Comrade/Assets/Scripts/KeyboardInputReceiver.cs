using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class KeyboardInputReceiver : MonoBehaviour, IInputReceiver
{
    public int PlayerID { get; set; }

    private static Dictionary<int, KeyCode> Button0Keys = new Dictionary<int, KeyCode>()
    {
        {0, KeyCode.Alpha0 },
        {1, KeyCode.Alpha1 },
        {2, KeyCode.Alpha2 },
        {3, KeyCode.Alpha3 },
        {4, KeyCode.Alpha4 },
        {5, KeyCode.Alpha5 }
    };
    private static Dictionary<int, KeyCode> Button1Keys = new Dictionary<int, KeyCode>()
    {
    };

    public Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    public float GetMouseX()
    {
        return Input.mousePosition.x;
    }

    public float GetMouseY()
    {
        return Input.mousePosition.y;
    }

    public bool GetMouseButton(int button)
    {
        if (button == 0 && Button0Keys.ContainsKey(PlayerID))
            return Input.GetKey(Button0Keys[PlayerID]);
        if (button == 1 && Button1Keys.ContainsKey(PlayerID))
            return Input.GetKey(Button1Keys[PlayerID]);

        return false;
    }

    public bool GetMouseButtonDown(int button)
    {
        if (button == 0 && Button0Keys.ContainsKey(PlayerID))
            return Input.GetKeyDown(Button0Keys[PlayerID]);
        if (button == 1 && Button1Keys.ContainsKey(PlayerID))
            return Input.GetKeyDown(Button1Keys[PlayerID]);

        return false;
    }

    public bool GetMouseButtonUp(int button)
    {
        if (button == 0 && Button0Keys.ContainsKey(PlayerID))
            return Input.GetKeyUp(Button0Keys[PlayerID]);
        if (button == 1 && Button1Keys.ContainsKey(PlayerID))
            return Input.GetKeyUp(Button1Keys[PlayerID]);

        return false;
    }
}
