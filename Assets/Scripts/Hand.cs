using UnityEngine;
using UnityEngine.Events;

public class Hand : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private DeltaTimeType _deltaTimeType;
    [SerializeField, Range(1, 10)] private int _horizontalSensibility = 1;
    [SerializeField, Range(1, 10)] private int _verticalSensibility = 1;

    [Header("Player Index")]
    [SerializeField] private JoystickIndex _playerIndex = 0;
    [SerializeField] private TMPro.TMP_Text _playerName;

    [Header("Player Movement Bounds")]
    [SerializeField] private Vector2 _horizontalBounds;
    [SerializeField] private Vector2 _verticalBounds;
    [SerializeField] private Vector2 _centerBounds;

    [Header("Player Events")]
    [SerializeField] private HandEvent _hold;
    [SerializeField] private HandEvent _release;

    public int? CurrentDragInstance = null;

    protected virtual void OnEnable()
    {
        int playerNumber = ((int)_playerIndex) + 1;
        _playerName.text = string.Concat("P", playerNumber.ToString());
        CurrentDragInstance = null;
    }

    protected virtual void Update()
    {
        // move hand
        string horizontalAxisName = "Horizontal"; // string.Format( {0}", (int)_playerIndex);
        string verticalAxisName = "Vertical"; // string.Format("Vertical {0}", (int)_playerIndex);
        float horizontalAxis = Input.GetAxisRaw(horizontalAxisName) * _horizontalSensibility;
        float verticalAxis = Input.GetAxisRaw(verticalAxisName) * _verticalSensibility;
        Vector2 handMovement = new Vector2(horizontalAxis, verticalAxis);
        transform.Translate(handMovement * Utils.GetDeltaTime(_deltaTimeType), Space.World);

        // clamp hand
        Vector2 position = transform.position;
        position.x = Mathf.Clamp(position.x, _horizontalBounds.x + _centerBounds.x, _horizontalBounds.y + _centerBounds.x);
        position.y = Mathf.Clamp(position.y, _verticalBounds.x + _centerBounds.y, _verticalBounds.y + _centerBounds.y);
        transform.position = position;

        string submitButtonName = "Submit";// string.Format("Submit {0}", (int)_playerIndex);
        if (Input.GetButtonDown(submitButtonName))
        {
            _hold?.Invoke(this);
        }
        else if (Input.GetButtonUp(submitButtonName))
        {
            _release?.Invoke(this);
        }
    }

    public void AddDragListener(Drag drag)
    {
        _hold.AddListener(drag.OnCursorStartDrag);
        _release.AddListener(drag.OnCursorEndDrag);
    }

    public void RemoveDragListener(Drag drag)
    {
        _hold.RemoveListener(drag.OnCursorStartDrag);
        _release.RemoveListener(drag.OnCursorEndDrag);
    }

    public string GetPlayerName()
    {
        int playerIndex = (int)_playerIndex;
        return string.Format("Player {0}", playerIndex.ToString());
    }
}
