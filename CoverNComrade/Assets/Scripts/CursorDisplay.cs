using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CursorDisplay : MonoBehaviour
{
    // Cursor
    public GameObject CursorPrefab;
    public GameObject CursorUI;
    private Canvas _cursorCanvas;
    private RectTransform _cursor;

    private bool _hasSpawnedCursor = false;

    // Components
    private IInputReceiver _input;
    private PlayerController _controller;

    void Start()
    {
        // Components
        _input = GetComponent<IInputReceiver>();
        _controller = GetComponent<PlayerController>();
        _cursorCanvas = CursorUI.GetComponent<Canvas>();

    }

    void Update()
    {
        if (!_hasSpawnedCursor)
        {
            // Cursor
            GameObject cursor = Instantiate(CursorPrefab);
            cursor.transform.SetParent(CursorUI.transform);
            cursor.transform.localScale = Vector3.one;
            cursor.GetComponent<Image>().color = _controller.PlayerColor;
            _cursor = cursor.GetComponent<RectTransform>();

            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            _input.SetMousePositionOffset((new Vector2(pos.x, pos.y) - new Vector2(_input.GetMouseX(), _input.GetMouseY()))/_cursorCanvas.scaleFactor);

            _hasSpawnedCursor = true;
        }

        // Update position
        SetCursorPosition(new Vector2(_input.GetMouseX(), _input.GetMouseY()));
    }

    void SetCursorPosition(Vector2 pos, bool scaled = true)
    {
        _cursor.anchoredPosition = pos;
        if (scaled)
            _cursor.anchoredPosition /= _cursorCanvas.scaleFactor;
    }

}
