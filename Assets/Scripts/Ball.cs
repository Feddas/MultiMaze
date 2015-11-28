using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Ball
{
    public class Ball : MonoBehaviour
    {
        [SerializeField]
        private float m_MovePower = 5; // The force added to the ball to move it.
        [SerializeField]
        private float m_MaxAngularVelocity = 25; // The maximum velocity the ball can rotate at.

        private const float k_GroundRayLength = 1f; // The length of the ray to check if the ball is grounded.
        private ControlModeEnum controlMode; // Whether or not to use torque to move the ball.
        private Rigidbody m_Rigidbody;

        private ColoredTrail ballTrail;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            // Set the maximum angular velocity.
            GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;

            controlMode = PlayerPref.Get(PlayerPrefEnum.ControlMode, defaultValue: ControlModeEnum.Torque);
        }

        private void Update()
        {
        }

        public IEnumerator StartBallTrail(Material ballTrailMaterial)
        {
            yield return null; // wait a frame for balls to be positioned

            ballTrail = new ColoredTrail(ballTrailMaterial);
            StartCoroutine(ballTrail.StartTrail(this.transform));
        }

        public IEnumerator ResetBall()
        {
            yield return null; // wait a frame for balls to be positioned

            if (ballTrail != null)
                ballTrail.PurgeLines();
        }

        public void Move(Vector3 moveDirection)
        {
            switch (controlMode)
            {
                case ControlModeEnum.None:
                    break;
                case ControlModeEnum.Torque:
                    // ... add torque around the axis defined by the move direction.
                    m_Rigidbody.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x) * m_MovePower);
                    break;
                case ControlModeEnum.Attract:
                    // Otherwise add force in the move direction.
                    m_Rigidbody.AddForce(moveDirection * m_MovePower);
                    break;
                case ControlModeEnum.Trace:
                    // Otherwise add force along a players drawn path.
                    m_Rigidbody.AddForce(moveDirection * m_MovePower);
                    break;
                default:
                    break;
            }
        }
    }
}
