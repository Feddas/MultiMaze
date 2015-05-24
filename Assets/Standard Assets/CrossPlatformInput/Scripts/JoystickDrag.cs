using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// This class enables playmodes for Drag and Path. The Torque playmode is handled by Joystick.cs. Control to Joystick.cs is given in JoystickType.cs.
/// </summary>
public class JoystickDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public string horizontalAxisName { get; set; }
    public string verticalAxisName { get; set; }

    float speedOfController = 100;
    //bool useNonTouch;  // TODO: get this to work
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
        //if (useNonTouch)
        //    return;

        m_HorizontalVirtualAxis.Update(value.x);
        m_VerticalVirtualAxis.Update(value.y);
    }

    /// <summary>
    /// Override a single players movement with a connected controller
    /// </summary>
    /// <param name="axisValue">a -1 to 1 axis value</param>
    public void SetAxesFromNonTouch(Vector3 axisValue)
    {
        if (axisValue == Vector3.zero)
        {
            //useNonTouch = false;
            return;
        }

        var newPosition = transform.position;
        newPosition.x += axisValue.x * Time.deltaTime * speedOfController;
        newPosition.y += axisValue.y * Time.deltaTime * speedOfController;
        transform.position = newPosition;

        m_HorizontalVirtualAxis.Update(newPosition.x);
        m_VerticalVirtualAxis.Update(newPosition.y);
        //useNonTouch = true;
    }

    public void OnDrag(PointerEventData data)
    {
        transform.position = new Vector3(data.position.x, data.position.y, 0);
        UpdateVirtualAxes(transform.position);
    }

    public void OnPointerUp(PointerEventData data)
    {
        transform.position = m_StartPos;
        UpdateVirtualAxes(Vector3.zero);
    }

    public void OnPointerDown(PointerEventData data)
    {
        OnDrag(data); // Allow a tap and hold to move the joystick thumb
    }
}
