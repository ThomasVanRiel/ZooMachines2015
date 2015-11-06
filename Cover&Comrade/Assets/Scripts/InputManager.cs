using UnityEngine;
using System.Collections;

#if UNITY_EDITOR_WIN
using RawMouseDriver;
using RawInputSharp;
#endif

public class InputManager : MonoBehaviour
{
#if UNITY_EDITOR_WIN
    private RawMouseDriver.RawMouseDriver _mousedriver;
    private RawMouse[] _mice;

    private Vector2[] _move;
    private const int NUM_MICE = 10;

    // Use this for initialization
    void Start()
    {
        Refresh();
    }

    void Update()
    {
        // Loop through all the connected mice
        for (int i = 0; i < _mice.Length; i++)
        {
            try
            {
                _mousedriver.GetMouse(i, ref _mice[i]);
                // Cumulative movement
                _move[i] += new Vector2(_mice[i].XDelta, -_mice[i].YDelta);
                //Debug.Log(_mice[i].Name);
            }
            catch { }
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Connected Mice:");
        for (int i = 0; i < _mice.Length; i++)
        {
            if (_mice[i] != null)
                GUILayout.Label("Mouse[" + i.ToString() + "] : " + _move[i] + _mice[i].Buttons[0] + _mice[i].Buttons[1]);
        }
    }
    
    void OnApplicationQuit()
    {
        // Clean up
        _mousedriver.Dispose();
    }
#endif

    public void Refresh()
    {
#if UNITY_EDITOR_WIN
        if (_mousedriver != null)
            _mousedriver.Dispose();

        _mousedriver = new RawMouseDriver.RawMouseDriver();
        _mice = new RawMouse[NUM_MICE];
        _move = new Vector2[NUM_MICE];
#endif
    }
}
