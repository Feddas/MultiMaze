using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Ball
{
    public class BallUserControl : MonoBehaviour
    {
        ///// <summary>
        ///// sends the new color being changed to
        ///// </summary>
        //public event EventHandler<EventArgs<string>> ColorChanged;

        private Ball ball; // Reference to the ball controller.

        private Vector3 move; // the world-relative desired move direction, calculated from user input.

        /// <summary> controllerPrefix is set via GameManager.cs createPlayers() </summary>
        private string controllerPrefix;
        private ControlModeEnum controlMode; // Whether or not to use torque to move the ball.

        private void Awake()
        {
            // Set up the reference.
            ball = GetComponent<Ball>();
            controlMode = PlayerPref.Get(PlayerPrefEnum.ControlMode, defaultValue: ControlModeEnum.Torque);
        }

        private void Update()
        {
            // calculate move direction
            switch (controlMode)
            {
                case ControlModeEnum.Torque:
                    move = fromTorque();
                    break;
                case ControlModeEnum.Drag:
                    move = fromDrag();
                    break;
                case ControlModeEnum.Path:
                case ControlModeEnum.None:
                default:
                    throw new NotImplementedException(controlMode.ToString() + " is not yet supported in BallUserControl.cs");
            }
        }

        private Vector3 fromTorque()
        {
            // Get the axis input. These axis names match the material names; i.e. Pink & Blue
            float h = CrossPlatformInputManager.GetAxis("Horizontal" + controllerPrefix);
            float v = CrossPlatformInputManager.GetAxis("Vertical" + controllerPrefix);

            // calculate move direction
            return (v * Vector3.forward + h * Vector3.right).normalized;
        }

        private Vector3 fromDrag()
        {
            // determine if, and how much, the analog stick has moved
            float h = CrossPlatformInputManager.GetAxis("Horizontal" + controllerPrefix);
            float v = CrossPlatformInputManager.GetAxis("Vertical" + controllerPrefix);
            if (h.IsZero() && v.IsZero())
            {
                return Vector3.zero;
            }

            // get the position of the colors analog stick in world space
            var analogStick = Camera.main.ScreenToWorldPoint(new Vector3(h, v, 0));

            // find the players analog stick relative to their balls location
            var result = analogStick - ball.transform.position;
            result.y = 0; // don't make the ball jump
            //Debug.Log(ball.transform.position + " postion analogStick=>" + analogStick + " == " + result);
            return result;
        }

        private void FixedUpdate()
        {
            // Call the Move function of the ball controller
            ball.Move(move);
        }

        public void SetBallMaterial(Material newMaterial)
        {
            controllerPrefix = newMaterial.name;
            StartCoroutine(ball.StartBallTrail(newMaterial));
            //OnColorChanged();

            this.GetComponent<Renderer>().material = newMaterial;
        }

        //private void OnColorChanged()
        //{
        //    if (ColorChanged != null)
        //    {
        //        ColorChanged(this, ColorChanged.Arg(controllerPrefix));
        //    }
        //}
    }
}
