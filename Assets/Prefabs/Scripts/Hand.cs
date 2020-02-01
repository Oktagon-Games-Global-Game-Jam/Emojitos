using UnityEngine;

public class Hand : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private DeltaTimeType _deltaTimeType;
    [SerializeField, Range(1, 10)] private int _horizontalSensibility = 1;
    [SerializeField, Range(1, 10)] private int _verticalSensibility = 1;

    [Header("Player Index")]
    [SerializeField] private JoystickIndex _playerIndex = 0;
    [SerializeField] private TMPro.TMP_Text _playerName;

    protected virtual void OnEnable()
    {
        int playerNumber = ((int)_playerIndex) + 1;
        _playerName.text = string.Concat("P", playerNumber.ToString());
    }

    protected virtual void Update()
    {
        string[] joysticks = Input.GetJoystickNames();
        string horizontalAxisName = string.Format("Horizontal {0}", (int)_playerIndex);
        string verticalAxisName = string.Format("Vertical {0}", (int)_playerIndex);
        float horizontalAxis = Input.GetAxisRaw(horizontalAxisName) * _horizontalSensibility;
        float verticalAxis = Input.GetAxisRaw(verticalAxisName) * _verticalSensibility;
        Vector2 handMovement = new Vector2(horizontalAxis, verticalAxis);
        transform.Translate(handMovement * Utils.GetDeltaTime(_deltaTimeType), Space.World);

        Debug.LogFormat("Horizontal: [{0}], Vertical: [{1}], Input Movement: {2}", horizontalAxisName, verticalAxisName, handMovement);
    }
}
