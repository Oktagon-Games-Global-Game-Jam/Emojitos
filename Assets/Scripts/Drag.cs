using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private Collider2D _collider2D;
    private GameObject _cursor;
    private Vector2? _cursorLastPosition;

    protected virtual void OnEnable()
    {
        if (_collider2D == null)
        {
            _collider2D = GetComponent<Collider2D>();
        }
        _cursor = null;
        _cursorLastPosition = null;
    }

    public void OnCursorStartDrag(GameObject gameObject)
    {
        Vector2 cursorPosition = Vector2.zero;
        cursorPosition.x = gameObject.transform.position.x;
        cursorPosition.y = gameObject.transform.position.y;
        if (_collider2D.OverlapPoint(cursorPosition))
        {
            _cursor = gameObject;
            _cursorLastPosition = cursorPosition;
        }
        else
        {
            _cursor = null;
            _cursorLastPosition = null;
        }
    }

    protected virtual void Update()
    {
        if (_cursor != null && _cursorLastPosition.HasValue)
        {
            Vector2 cursorPosition = Vector2.zero;
            cursorPosition.x = _cursor.transform.position.x;
            cursorPosition.y = _cursor.transform.position.y;

            Vector2 cursorDelta = cursorPosition - _cursorLastPosition.Value;
            Debug.Log(cursorDelta);
            transform.Translate(cursorDelta, Space.World);

            _cursorLastPosition = cursorPosition;
        }
    }

    public void OnCursorEndDrag(GameObject gameObject)
    {
        _cursor = null;
    }
}
