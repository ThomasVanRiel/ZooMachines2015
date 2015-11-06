using UnityEngine;
using System.Collections;

#if UNITY_EDITOR_WIN
using RawMouseDriver;
using RawInputSharp;
#endif

public class InputManager : MonoBehaviour
{

    public static int AmountOfMice { get; set; }

#if UNITY_EDITOR_WIN
    private RawMouseDriver.RawMouseDriver _mousedriver;
    private static RawMouse[] _mice;

    private static Vector2[] _move;
    private static float[] _scroll;
    private const int NUM_MICE = 10;
    private const int NUM_BUTTONS = 2;
    private bool[][] _prevMouse;
    private static bool[][] _mouseDown;
    private static bool[][] _mouseUp;

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
                // Get mouse
                _mousedriver.GetMouse(i, ref _mice[i]);

                // Cumulative movement
                _move[i] += new Vector2(_mice[i].XDelta, -_mice[i].YDelta);
                if (_move[i].x < 0)
                    _move[i].x = 0;
                if (_move[i].y < 0)
                    _move[i].y = 0;
                if (_move[i].x > Camera.main.pixelWidth)
                    _move[i].x = Camera.main.pixelWidth;
                if (_move[i].y > Camera.main.pixelHeight)
                    _move[i].y = Camera.main.pixelHeight;

                // Cumulative scroll
                _scroll[i] += _mice[i].ZDelta;

                // Button actions
                for (int b = 0; b < NUM_BUTTONS; ++b)
                {
                    _mouseDown[b][i] = !_prevMouse[b][i] &&  _mice[i].Buttons[b];
                    _mouseUp[b][i]   =  _prevMouse[b][i] && !_mice[i].Buttons[b];
                    _prevMouse[b][i] = _mice[i].Buttons[b];
                }
            }
            catch { }
        }
        if (AmountOfMice == 0)
            CalculateAmountOfMice();
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
        // Mouse driver
        if (_mousedriver != null)
            _mousedriver.Dispose();
        _mousedriver = new RawMouseDriver.RawMouseDriver();

        // Values
        _mice = new RawMouse[NUM_MICE];
        _move = new Vector2[NUM_MICE];
        _scroll = new float[NUM_MICE];

        // Buttons
        _prevMouse = new bool[NUM_BUTTONS][];
        _mouseDown = new bool[NUM_BUTTONS][];
        _mouseUp =   new bool[NUM_BUTTONS][];
        for (int b = 0; b < NUM_BUTTONS; ++b)
        {
            _prevMouse[b] = new bool[NUM_MICE];
            _mouseDown[b] = new bool[NUM_MICE];
            _mouseUp[b] = new bool[NUM_MICE];
        }
#endif
    }

    private int CalculateAmountOfMice()
    {
#if UNITY_EDITOR_WIN
        AmountOfMice = 0;
        for (int i = 0; i < _mice.Length; i++)
        {
            try
            {
                // Get mouse
                if (_mice[i] != null)
                    ++AmountOfMice;
            }
            catch { }
        }

#else
        AmountOfMice = 1;
#endif
        return AmountOfMice;
    }

    #region Getters

    /// <summary>
    /// Get position of requested mouse.
    /// </summary>
    /// <param name="id">Mouse id</param>
    /// <returns>X position of requested mouse.</returns>
    public static Vector3 GetMousePosition(int id)
    {
#if UNITY_EDITOR
        // Check if id is recognised
        if (!IsValidMouse(id))
            return Vector3.zero;
#endif
#if UNITY_EDITOR_WIN
        return new Vector3(_move[id].x, _move[id].y, 0);
#else
        return Input.mousePosition;
#endif
    }

    /// <summary>
    /// Get X position of requested mouse.
    /// </summary>
    /// <param name="id">Mouse id</param>
    /// <returns>X position of requested mouse.</returns>
    public static float GetMouseX(int id)
    {
#if UNITY_EDITOR
        // Check if id is recognised
        if (!IsValidMouse(id))
            return 0.0f;
#endif
#if UNITY_EDITOR_WIN
        return _move[id].x;
#else
        return Input.GetAxis("Mouse X");
#endif
    }

    /// <summary>
    /// Get Y position of requested mouse.
    /// </summary>
    /// <param name="id">Mouse id</param>
    /// <returns>Y position of requested mouse.</returns>
    public static float GetMouseY(int id)
    {
#if UNITY_EDITOR
        // Check if id is recognised
        if (!IsValidMouse(id))
            return 0.0f;
#endif
#if UNITY_EDITOR_WIN
        return _move[id].y;
#else
        return Input.GetAxis("Mouse Y");
#endif
    }

    /// <summary>
    /// Get scroll position of requested mouse.
    /// </summary>
    /// <param name="id">Mouse id</param>
    /// <returns>scroll position of requested mouse.</returns>
    public static float GetMouseScroll(int id)
    {
#if UNITY_EDITOR
        // Check if id is recognised
        if (!IsValidMouse(id))
            return 0.0f;
#endif
#if UNITY_EDITOR_WIN
        return _scroll[id];
#else
        return Input.GetAxis("Mouse ScrollWheel");
#endif
    }

    public static bool GetMouseButton(int button, int id)
    {
#if UNITY_EDITOR
        // Check if button and id are recognised
        if (button > 1)
        {
            Debug.LogWarning("InputManager.GetMouseButton() - mouse button " + button + " is not recognised.");
            return false;
        }
        if (!IsValidMouse(id))
            return false;
#endif

#if UNITY_EDITOR_WIN
        return _mice[id].Buttons[button];
#else
        return Input.GetMouseButton(button);
#endif
    }

    public static bool GetMouseButtonDown(int button, int id)
    {
#if UNITY_EDITOR
        // Check if button and id are recognised
        if (button > 1)
        {
            Debug.LogWarning("InputManager.GetMouseButtonDown() - mouse button " + button + " is not recognised.");
            return false;
        }
        if (!IsValidMouse(id))
            return false;
#endif

#if UNITY_EDITOR_WIN
        return _mouseDown[button][id];
#else
        return Input.GetMouseButtonDown(button);
#endif
    }

    public static bool GetMouseButtonUp(int button, int id)
    {
#if UNITY_EDITOR
        // Check if button and id are recognised
        if (button > 1)
        {
            Debug.LogWarning("InputManager.GetMouseButtonDown() - mouse button " + button + " is not recognised.");
            return false;
        }
        if (!IsValidMouse(id))
            return false;
#endif

#if UNITY_EDITOR_WIN
        return _mouseUp[button][id];
#else
        return Input.GetMouseButtonUp(button);
#endif

    }
#endregion


    private static bool IsValidMouse(int id)
    {
        if (AmountOfMice == 0)
            return false;

        if (id >= AmountOfMice)
        {
            Debug.LogWarning("InputManager.IsValidMouse() - mouse " + id + " is not a recognised id.");
            return false;
        }
        return true;
    }

}
