using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Vehicles.Ball
{
    public class BallUserControl : MonoBehaviour
    {
        /// <summary> only raised during the Trace control mode as the ball is following along a path </summary>
        public event EventHandler<EventArgs> TraceLineChanged;

        ///// <summary>
        ///// sends the new color being changed to
        ///// </summary>
        //public event EventHandler<EventArgs<string>> ColorChanged;

        private Ball ball; // Reference to the ball controller.

        private Vector3 move; // the world-relative desired move direction, calculated from user input.

        /// <summary> only used by Trace control mode </summary>
        public List<Vector3> LinePositions
        {
            get { return _linePositions; }
            set { _linePositions = value; }
        }
        private List<Vector3> _linePositions = new List<Vector3>();

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
                case ControlModeEnum.Attract:
                    move = fromAttract();
                    break;
                case ControlModeEnum.Trace:
                    move = fromTracePath();
                    break;
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

        private Vector3 fromAttract()
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

        private Vector3 fromTracePath()
        {
            // there's not enough line to follow
            if (LinePositions == null || LinePositions.Count < 2)
                return Vector3.zero;

            // find the next position of the line relative to their balls location
            var result = LinePositions[0] - ball.transform.position;
            result.y = 0; // don't make the ball jump
            //Debug.Log(ball.transform.position + " postion LinePositions[0]=>" + LinePositions[0] + " == " + result);

            // consider the position reached and remove it from the drawn trace path line
            if (result.magnitude < 0.5f)
            {
                LinePositions.RemoveAt(0);
                OnTraceLineChanged(); // enables the drawn line reflects the ball moving along
            }
            return result;
        }

        private void OnTraceLineChanged()
        {
            if (this.TraceLineChanged != null)
                this.TraceLineChanged(this, new EventArgs());
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
