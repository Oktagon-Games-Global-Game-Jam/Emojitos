using UnityEngine;
using UnityEngine.Events;

public enum DeltaTimeType
{
    None,
    DeltaTime,
    SmoothDeltaTime,
    UnscaledDeltaTime,
    FixedDeltaTime,
    FixedUnscaledDeltaTime
}

public enum JoystickIndex
{
    Player1 = 0,
    Player2 = 1,
    Player3 = 2,
    Player4 = 3
}

public class Utils
{
    public static float GetDeltaTime(DeltaTimeType deltaTimeType)
    {
        switch (deltaTimeType)
        {
            case DeltaTimeType.DeltaTime:
                return Time.deltaTime;
            case DeltaTimeType.SmoothDeltaTime:
                return Time.smoothDeltaTime;
            case DeltaTimeType.UnscaledDeltaTime:
                return Time.unscaledDeltaTime;
            case DeltaTimeType.FixedDeltaTime:
                return Time.fixedDeltaTime;
            case DeltaTimeType.FixedUnscaledDeltaTime:
                return Time.fixedUnscaledDeltaTime;
        }
        return 1f;
    }
}

[System.Serializable]
public class GameObjectEvent : UnityEvent<GameObject> { }