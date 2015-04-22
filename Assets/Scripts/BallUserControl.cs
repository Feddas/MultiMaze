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

        private string controllerPrefix;

        private void Awake()
        {
            // Set up the reference.
            ball = GetComponent<Ball>();
        }

        private void Update()
        {
            // Get the axis and jump input. These axis names match the material names; i.e. Pink & Blue
            float h = CrossPlatformInputManager.GetAxis("Horizontal" + controllerPrefix);
            float v = CrossPlatformInputManager.GetAxis("Vertical" + controllerPrefix);

            if (hasController)
                useController(ref h, ref v);

            // calculate move direction
            move = (v * Vector3.forward + h * Vector3.right).normalized;
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

            // Allow this player to use a connected joystick
            if (string.IsNullOrEmpty(firstColor) && Input.GetJoystickNames().Length > 0)
            {
                firstColor = controllerPrefix;
                hasController = true;
            }

            this.GetComponent<Renderer>().material = newMaterial;
        }
    }
}
