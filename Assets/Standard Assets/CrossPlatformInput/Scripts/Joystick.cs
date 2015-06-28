using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        /// <summary>
        /// Visual that is used to indicate what direction the joystick is being pulled in
        /// </summary>
        public RectTransform JoystickThumb { get; set; }
        public const float Deadzone = 0.1f;

        public enum AxisOption
        {
            // Options for which axes to use
            Both, // Use both
            OnlyHorizontal, // Only horizontal
            OnlyVertical // Only vertical
        }

        private int movementRange { get; set; }
        public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use

        // SHAWN changed from unity inspector set properties to properties managed by JoystickType.cs
        public string horizontalAxisName { get; set; } // = "Horizontal"; // The name given to the horizontal axis for the cross platform input
        public string verticalAxisName { get; set; } // = "Vertical"; // The name given to the vertical axis for the cross platform input

        Vector3 m_StartPos;
        bool m_UseX; // Toggle for using the x axis
        bool m_UseY; // Toggle for using the Y axis
        CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
        CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

        void OnEnable() // Don't CreateVirtualAxes() on Start() as, when this is disabled, it may overwrite Axes with the same name that are enabled
        {
            if (JoystickThumb == null)
                JoystickThumb = transform as RectTransform;
            m_StartPos = JoystickThumb.position;
            CreateVirtualAxes();
        }

        void UpdateVirtualAxes(Vector3 value)
        {
            var delta = m_StartPos - value;
            delta.y = -delta.y;
            delta /= movementRange;
            if (m_UseX)
            {
                m_HorizontalVirtualAxis.Update(-delta.x);
            }

            if (m_UseY)
            {
                m_VerticalVirtualAxis.Update(delta.y);
            }
        }

        ///<summary>Uses JoystickThumb and Interaction zone to determine movement range</summary>
        public void SetMovementRange(RectTransform rectTransform)
        {
            int radiusThumb = (int)Mathf.Min(JoystickThumb.rect.width, JoystickThumb.rect.height) / 2;
            int radiusInteractZone = (int)Mathf.Min(rectTransform.rect.width, rectTransform.rect.height) / 2;
            movementRange = radiusInteractZone - radiusThumb;
        }

        /// <param name="axisValue">a -1 to 1 axis value</param>
        public void SetAxesFromNonTouch(Vector3 axisValue)
        {
            if (axisValue == Vector3.zero)
                return;

            m_HorizontalVirtualAxis.Update(axisValue.x);
            m_VerticalVirtualAxis.Update(axisValue.y);
        }

        void CreateVirtualAxes()
        {
            // set axes to use
            m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
            m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

            // create new axes based on axes to use
            if (m_UseX)
            {
                m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
            }
            if (m_UseY)
            {
                m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
                CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
            }
        }

        public void OnDrag(PointerEventData data)
        {
            Vector3 newPos = Vector3.zero;

            if (m_UseX)
            {
                int delta = (int)(data.position.x - m_StartPos.x);
                newPos.x = delta;
            }

            if (m_UseY)
            {
                int delta = (int)(data.position.y - m_StartPos.y);
                newPos.y = delta;
            }

            // SHFEAT replaced square Mathf.Clamp with circular Vector3.ClampMagnitude
            JoystickThumb.position = Vector3.ClampMagnitude(newPos, movementRange) + m_StartPos;
            UpdateVirtualAxes(JoystickThumb.position);
        }

        public void OnPointerUp(PointerEventData data)
        {
            JoystickThumb.position = m_StartPos;
            UpdateVirtualAxes(m_StartPos);
        }

        public void OnPointerDown(PointerEventData data)
        {
            OnDrag(data); // Allow a tap and hold to move the joystick thumb
        }

        void OnDisable()
        {
            // remove the joysticks from the cross platform input
            if (m_UseX)
            {
                m_HorizontalVirtualAxis.Remove();
            }
            if (m_UseY)
            {
                m_VerticalVirtualAxis.Remove();
            }
        }
    }
}