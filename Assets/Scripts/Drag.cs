using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour, IInteractable
{
    private Collider2D _collider2D;
    private Hand _cursor;
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

    public void OnCursorStartDrag(Hand hand)
    {
        Vector2 cursorPosition = Vector2.zero;
        cursorPosition.x = hand.transform.position.x;
        cursorPosition.y = hand.transform.position.y;
        if (_collider2D.OverlapPoint(cursorPosition) && !hand.CurrentDragInstance.HasValue)
        {
            _cursor = hand;
            _cursorLastPosition = cursorPosition;

            hand.CurrentDragInstance = GetInstanceID();
        }
        else
        {
            OnCursorEndDrag(hand);
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
            transform.Translate(cursorDelta, Space.World);

            _cursorLastPosition = cursorPosition;
        }
    }

    public void OnCursorEndDrag(Hand hand)
    {
        _cursor = null;

        if (hand.CurrentDragInstance == GetInstanceID())
        {
            hand.CurrentDragInstance = null;
        }
    }
}
