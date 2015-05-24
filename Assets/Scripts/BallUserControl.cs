using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Ball
{
    public class BallUserControl : MonoBehaviour
    {
        private static string firstColor;
        private bool hasController;

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

            if (hasController)
                useController(ref h, ref v);

            // calculate move direction
            return (v * Vector3.forward + h * Vector3.right).normalized;
        }

        private bool isZero(float value)
        {
            return Mathf.Abs(value) <= float.Epsilon;
        }
        private Vector3 fromDrag()
        {
            // determine if, and how much, the analog stick has moved
            float h = CrossPlatformInputManager.GetAxis("Horizontal" + controllerPrefix);
            float v = CrossPlatformInputManager.GetAxis("Vertical" + controllerPrefix);
            if (isZero(h) && isZero(v))
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

        /// <summary>
        /// Override a single players movement with a connected controller
        /// </summary>
        private void useController(ref float h, ref float v)
        {
            float hController = Input.GetAxis("Horizontal");
            float vController = Input.GetAxis("Vertical");

            if (Mathf.Abs(hController) > float.Epsilon || Mathf.Abs(vController) > float.Epsilon)
            {
                h = hController;
                v = vController;
            }
        }

        private void FixedUpdate()
        {
            // Call the Move function of the ball controller
            ball.Move(move);
        }

        public void SetBallMaterial(Material newMaterial)
        {
            controllerPrefix = newMaterial.name;

            // Allow only the first player (which is pink) to use a connected joystick
            if (string.IsNullOrEmpty(firstColor) && Input.GetJoystickNames().Length > 0)
            {
                firstColor = controllerPrefix;
                hasController = true;
            }

            this.GetComponent<Renderer>().material = newMaterial;
        }
    }
}
