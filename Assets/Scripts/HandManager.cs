using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        string[] joystickNames = Input.GetJoystickNames();
        Debug.Log(joystickNames);
    }
}
