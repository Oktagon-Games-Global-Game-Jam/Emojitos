using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Press : MonoBehaviour, IInteractable
{
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private UnityEvent _onPress;

    protected virtual void OnEnable()
    {
        if (_collider2D == null)
        {
            _collider2D.GetComponent<Collider2D>();
        }
    }

    public void OnCursorStartDrag(Hand hand)
    {
        Vector2 cursorPosition = Vector2.zero;
        cursorPosition.x = hand.transform.position.x;
        cursorPosition.y = hand.transform.position.y;
        if (_collider2D.OverlapPoint(cursorPosition))
        {
            _onPress?.Invoke();
        }
    }

    public void OnCursorEndDrag(Hand hand)
    {
        // nothing
    }
}
