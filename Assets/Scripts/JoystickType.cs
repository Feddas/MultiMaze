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

    [Tooltip("The color of this controller. This value is used to bind to the controllerPrefix set in the ball (case sensitive)")]
    public string controllerPrefix; // The name given to the vertical axis for the cross platform input

    private const string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
    private const string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

    private ControlModeEnum controlMode; // Whether or not to use torque to move the ball.
    private static string firstColor;
    private bool hasController;

    // Use this for initialization
    void Start()
    {
        hasController = hasController_();
        controlMode = PlayerPref.Get(PlayerPrefEnum.ControlMode, defaultValue: ControlModeEnum.Torque);
        switch (controlMode)
        {
            case ControlModeEnum.Torque:
                joystickDrag.enabled = false;
                joystickCrossInput.horizontalAxisName = horizontalAxisName + controllerPrefix;
                joystickCrossInput.verticalAxisName = verticalAxisName + controllerPrefix;
                joystickCrossInput.enabled = true;
                break;
            case ControlModeEnum.Drag:
                joystickCrossInput.enabled = false;
                joystickDrag.horizontalAxisName = horizontalAxisName + controllerPrefix;
                joystickDrag.verticalAxisName = verticalAxisName + controllerPrefix;
                joystickDrag.enabled = true;
                break;
            case ControlModeEnum.Path:
            case ControlModeEnum.None:
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasController)
            useController();
    }

    private void useController()
    {
        var axes = externalAxes();
        if (axes == Vector3.zero)
            return;

        switch (controlMode)
        {
            case ControlModeEnum.Torque:
                joystickCrossInput.SetAxesFromNonTouch(axes);
                break;
            case ControlModeEnum.Drag:
                joystickDrag.SetAxesFromNonTouch(axes);
                break;
            case ControlModeEnum.Path:
            case ControlModeEnum.None:
            default:
                break;
        }
    }

    private bool hasController_()
    {
        // Allow only the first player (which is pink) to use a connected joystick
        if (string.IsNullOrEmpty(firstColor) && Input.GetJoystickNames().Length > 0)
        {
            firstColor = controllerPrefix;
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector3 externalAxes()
    {
        float hController = Input.GetAxis("Horizontal");
        float vController = Input.GetAxis("Vertical");

        if (Mathf.Abs(hController) > float.Epsilon || Mathf.Abs(vController) > float.Epsilon)
        {
            return new Vector3(hController, vController, 0);
        }
        else
        {
            return Vector3.zero;
        }
    }
}
