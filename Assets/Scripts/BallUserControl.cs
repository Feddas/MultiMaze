using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Ball
{
    public class BallUserControl : MonoBehaviour
    {
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
            
            // calculate move direction
            move = (v * Vector3.forward + h * Vector3.right).normalized;
        }

        private void FixedUpdate()
        {
            // Call the Move function of the ball controller
            ball.Move(move);
        }

        public void SetBallMaterial(Material newMaterial)
        {
            controllerPrefix = newMaterial.name;

            this.GetComponent<Renderer>().material = newMaterial;
        }
    }
}
