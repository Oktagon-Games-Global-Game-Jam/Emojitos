using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    public readonly static int StartGameScene = 0;
    public readonly static int MainMenuScene = 1;
    public readonly static int OnePlayerScene = 2;

    [RuntimeInitializeOnLoadMethod]
    private static void OnRuntimeMethoadLoad()
    {
        if (Application.isPlaying)
        {
            SceneManager.LoadScene(MainMenuScene);
        }        
    }

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
public class HandEvent : UnityEvent<Hand> { }

[System.Serializable]
public class FinishMatchEvent : UnityEvent<Match, string, int> { }