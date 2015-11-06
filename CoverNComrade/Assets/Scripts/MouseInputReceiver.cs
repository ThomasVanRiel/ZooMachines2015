using UnityEngine;
using System.Collections;

public class MouseInputReceiver : MonoBehaviour, IInputReceiver
{
    public int PlayerID { get; set; }
    

    public Vector3 GetMousePosition()
    {
        return InputManager.GetMousePosition(PlayerID);
    }
    public float GetMouseX()
    {
        return InputManager.GetMouseX(PlayerID);
    }

    public float GetMouseY()
    {
        return InputManager.GetMouseY(PlayerID);
    }

    public float GetMouseScroll()
    {
        return InputManager.GetMouseScroll(PlayerID);
    }

    public bool GetMouseButton(int button)
    {
        return InputManager.GetMouseButton(button, PlayerID);
    }

    public bool GetMouseButtonDown(int button)
    {
        return InputManager.GetMouseButtonDown(button, PlayerID);
    }

    public bool GetMouseButtonUp(int button)
    {
        return InputManager.GetMouseButtonUp(button, PlayerID);
    }
}
