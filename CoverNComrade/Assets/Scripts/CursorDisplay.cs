using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CursorDisplay : MonoBehaviour
{
    // Cursor
    public GameObject CursorPrefab;
    public GameObject CursorUI;
    private Canvas _cursorCanvas;
    private RectTransform _cursorTransform;
    private GameObject _cursor;

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
            _cursor = Instantiate(CursorPrefab);
            _cursor.transform.SetParent(CursorUI.transform);
            _cursor.transform.localScale = Vector3.one;
            _cursor.GetComponent<Image>().color = _controller.PlayerColor;
            _cursorTransform = _cursor.GetComponent<RectTransform>();

            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            _input.SetMousePositionOffset((new Vector2(pos.x, pos.y) - new Vector2(_input.GetMouseX(), _input.GetMouseY()))/_cursorCanvas.scaleFactor);

            _hasSpawnedCursor = true;
        }

        // Update position
        if (_controller.IsAlive() )
            SetCursorPosition(new Vector2(_input.GetMouseX(), _input.GetMouseY()));
        // Or disable
        else
            _cursor.SetActive(false);
    }

    void SetCursorPosition(Vector2 pos, bool scaled = true)
    {
        _cursorTransform.anchoredPosition = pos;
        if (scaled)
            _cursorTransform.anchoredPosition /= _cursorCanvas.scaleFactor;
    }

}
