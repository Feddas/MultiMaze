using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System;

public class JoystickType : MonoBehaviour
{
    [SerializeField]
    private Joystick joystickCrossInput;
    [SerializeField]
    private JoystickDrag joystickDrag;

    public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
    public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

    private ControlModeEnum controlMode; // Whether or not to use torque to move the ball.

    // Use this for initialization
    void Start()
    {
        controlMode = PlayerPref.Get(PlayerPrefEnum.ControlMode, defaultValue: ControlModeEnum.Torque);
        switch (controlMode)
        {
            case ControlModeEnum.Torque:
                joystickDrag.enabled = false;
                joystickCrossInput.horizontalAxisName = horizontalAxisName;
                joystickCrossInput.verticalAxisName = verticalAxisName;
                joystickCrossInput.enabled = true;
                break;
            case ControlModeEnum.Drag:
                joystickCrossInput.enabled = false;
                joystickDrag.horizontalAxisName = horizontalAxisName;
                joystickDrag.verticalAxisName = verticalAxisName;
                joystickDrag.enabled = true;
                break;
            case ControlModeEnum.Path:
            case ControlModeEnum.None:
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update() { }
}
