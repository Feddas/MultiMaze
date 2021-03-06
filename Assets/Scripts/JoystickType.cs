﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System;
using UnityEngine.UI;

public class JoystickType : MonoBehaviour
{
    [SerializeField]
    private Joystick joystickCrossInput;
    [SerializeField]
    private JoystickDrag joystickDrag;
    [SerializeField]
    private RectTransform joystickThumb;
    [SerializeField][Tooltip("visualization of where the joystick can be interacted with")]
    private Image interactZoneImage;

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
        Color zoneColor = interactZoneImage.color;
        switch (controlMode)
        {
            case ControlModeEnum.Torque:
                zoneColor.a = .25f;
                joystickDrag.enabled = false;
                joystickCrossInput.JoystickThumb = joystickThumb;
                joystickCrossInput.SetMovementRange(this.transform as RectTransform);
                joystickCrossInput.horizontalAxisName = horizontalAxisName + controllerPrefix;
                joystickCrossInput.verticalAxisName = verticalAxisName + controllerPrefix;
                joystickCrossInput.enabled = true;
                break;
            case ControlModeEnum.Attract:
            case ControlModeEnum.Trace:
                zoneColor.a = 0;
                joystickCrossInput.enabled = false;
                joystickDrag.horizontalAxisName = horizontalAxisName + controllerPrefix;
                joystickDrag.verticalAxisName = verticalAxisName + controllerPrefix;
                joystickDrag.enabled = true;
                joystickDrag.IsTraceMode = controlMode == ControlModeEnum.Trace;
                break;
            case ControlModeEnum.None:
            default:
                break;
        }
        interactZoneImage.color = zoneColor;
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

        switch (controlMode)
        {
            case ControlModeEnum.Torque:
                joystickCrossInput.SetAxesFromNonTouch(axes);
                break;
            case ControlModeEnum.Attract:
            case ControlModeEnum.Trace:
                joystickDrag.SetAxesFromNonTouch(axes);
                break;
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
        
        return new Vector3(hController, vController, 0);

        // deadzone and bounds checks are handled in JoystickDrag or JoystickCrossInput so that no input can be custom handled
    }
}
