using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// This class enables playmodes for Attract and Trace. The Torque playmode is handled by Joystick.cs. Control to Joystick.cs is given in JoystickType.cs.
/// </summary>
public class JoystickDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public string horizontalAxisName { get; set; }
    public string verticalAxisName { get; set; }

    /// <summary> Determines if the Trace control mode is used. If false, attract mode is used </summary>
    public bool IsTraceMode { get; set; }

    /// <summary> The linerenderer that draws the trace path for Trace control mode</summary>
    public DrawPath DrawPathObj;

    float speedOfController = 100;
    bool useNonTouch;
    Vector3 m_StartPos;
    CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

    // Use this for initialization
    void Start()
    {
        m_StartPos = transform.position;
    }

    void OnEnable()
    {
        CreateVirtualAxes();
    }

    void OnDisable()
    {
        // remove the joysticks from cross platform input so they can be recreated for different control schemes
        m_HorizontalVirtualAxis.Remove();
        m_VerticalVirtualAxis.Remove();
    }

    // Update is called once per frame
    void Update() { }

    void CreateVirtualAxes()
    {
        m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
        m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
    }

    void UpdateVirtualAxes(Vector3 value)
    {
        if (useNonTouch)
            return;

        if (value == Vector3.zero)
        {
            transform.position = m_StartPos;
        }
        else
        {
            transform.position = value;
        }

        m_HorizontalVirtualAxis.Update(value.x);
        m_VerticalVirtualAxis.Update(value.y);
    }

    /// <summary>
    /// Override a single players movement with a connected controller
    /// </summary>
    /// <param name="axisValue">a -1 to 1 axis value</param>
    public void SetAxesFromNonTouch(Vector3 axisValue)
    {
        // if external controller is no longer being used, free up for touch control
        if (axisValue.magnitude < Joystick.Deadzone)
        {
            useNonTouch = false;
            return;
        }
        // else apply external controller input

        // don't let stick thumb go out of screen bounds
        if ((transform.position.x < 0 && axisValue.x < 0)
            || (transform.position.x > Screen.width && axisValue.x > 0))
        {
            axisValue.x = 0;
        }
        if ((transform.position.y < 0 && axisValue.y < 0)
            || (transform.position.y > Screen.height && axisValue.y > 0))
        {
            axisValue.y = 0;
        }

        // move stick thumb position
        var newPosition = transform.position;
        newPosition.x += axisValue.x * Time.deltaTime * speedOfController;
        newPosition.y += axisValue.y * Time.deltaTime * speedOfController;
        transform.position = newPosition;

        // update axes
        m_HorizontalVirtualAxis.Update(newPosition.x);
        m_VerticalVirtualAxis.Update(newPosition.y);
        useNonTouch = true;
    }

    public void OnDrag(PointerEventData data)
    {
        UpdateVirtualAxes(data.position);
    }

    public void OnPointerUp(PointerEventData data)
    {
        UpdateVirtualAxes(Vector3.zero);

        if (IsTraceMode) // disable path tracing
        {
            DrawPathObj.TouchIndex = -1;
        }
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (IsTraceMode)
        {
            if (data.pointerId != -1) // activate the draw path for this touch id
                DrawPathObj.TouchIndex = data.pointerId;
            else // allow a device to use non-touch pointers (pointers of index -1)
                DrawPathObj.TouchIndex = 0;

            //Debug.Log(" OnPointerDown" + data.pointerId + "DrawPathObj.TouchIndex" + DrawPathObj.TouchIndex);
        }

        OnDrag(data); // Allow a tap and hold to move the joystick thumb
    }
}
