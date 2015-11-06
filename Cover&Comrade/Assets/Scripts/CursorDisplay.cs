using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CursorDisplay : MonoBehaviour
{
    // Cursor
    public GameObject CursorPrefab;
    public GameObject CursorUI;
    private RectTransform _cursor;

    // Components
    private IInputReceiver _input;
    private PlayerController _controller;

    void Start()
    {
        // Components
        _input = GetComponent<IInputReceiver>();
        _controller = GetComponent<PlayerController>();

        // Cursor
        GameObject cursor = Instantiate(CursorPrefab);
        cursor.transform.SetParent(CursorUI.transform);
        cursor.GetComponent<Image>().color = _controller.PlayerColor;
        _cursor = cursor.GetComponent<RectTransform>();

    }

    void Update()
    {
        // Update position
        _cursor.anchoredPosition = new Vector2(_input.GetMouseX(), _input.GetMouseY());
    }

}
