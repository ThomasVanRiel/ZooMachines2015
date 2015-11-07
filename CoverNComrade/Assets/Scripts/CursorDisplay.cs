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
    //private TeamController _teamController;
    private PlayerController _playerController;

    void Start()
    {
        // Components
        _input = GetComponent<IInputReceiver>();
        //_teamController = GetComponent<TeamController>();
        _playerController = GetComponent<PlayerController>();
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
            //_cursor.GetComponent<Image>().color = _teamController.TeamColor;
            _cursorTransform = _cursor.GetComponent<RectTransform>();

            //_input.SetMousePositionOffset((new Vector2(pos.x, pos.y) - new Vector2(_input.GetMouseX(), _input.GetMouseY()))/_cursorCanvas.scaleFactor);

            _hasSpawnedCursor = true;
        }

        // Update position
        if (_playerController.IsAlive())
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            SetCursorPosition(new Vector2(pos.x, pos.y) + new Vector2(_input.GetMouseX(), _input.GetMouseY()));
        }
        // Or disable
        else
            _cursor.SetActive(false);
    }

    void SetCursorPosition(Vector2 pos, bool scaled = true)
    {
        if (pos.x < 0)
            pos.x = 0;
        if (pos.y < 0)
            pos.y = 0;
        if (pos.x > Camera.main.pixelWidth)
            pos.x = Camera.main.pixelWidth;
        if (pos.y > Camera.main.pixelHeight)
            pos.y = Camera.main.pixelHeight;

        if (scaled)
            pos /= _cursorCanvas.scaleFactor;

        _cursorTransform.anchoredPosition = pos;
    }

}
