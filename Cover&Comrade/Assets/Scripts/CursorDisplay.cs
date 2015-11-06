using UnityEngine;
using System.Collections;

public class CursorDisplay : MonoBehaviour
{
    // Cursor
    public GameObject CursorPrefab;
    public GameObject CursorUI;
    private RectTransform _cursor;

    // Components
    private IInputReceiver _input;

    void Start()
    {
        // Cursor
        GameObject cursor = Instantiate(CursorPrefab);
        cursor.transform.parent = CursorUI.transform;
        _cursor = cursor.GetComponent<RectTransform>();

        // Components
        _input = GetComponent<IInputReceiver>();
    }

    void Update()
    {
        // Update position
        _cursor.anchoredPosition = new Vector2(_input.GetMouseX(), _input.GetMouseX());
    }

}
