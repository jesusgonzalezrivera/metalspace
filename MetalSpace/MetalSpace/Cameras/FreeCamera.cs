using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MetalSpace.Cameras
{
    /// <summary>
    /// The <c>FreeCamera</c> class represents a camera that can move in all
    /// of the scene without any restriction.
    /// </summary>
    class FreeCamera : Camera
    {
        #region Fields

        /// <summary>
        /// Yaw rotation of the camera.
        /// </summary>
        private float _yaw;

        /// <summary>
        /// Pitch rotation of the camera.
        /// </summary>
        private float _pitch;

        /// <summary>
        /// Translation of the camera. Permit the calculation of the
        /// new position.
        /// </summary>
        private Vector3 _translation;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>FreeCamera</c> class.
        /// </summary>
        /// <param name="position">Initial position of the camera.</param>
        /// <param name="yaw">Initial yaw rotation of the camera.</param>
        /// <param name="pitch">Initial pitch rotation of the camera.</param>
        public FreeCamera(Vector3 position, float yaw, float pitch)
        {
            this.Position = position;

            _yaw = yaw;
            _pitch = pitch;

            _translation = Vector3.Zero;
        }

        #endregion

        #region Rotate Method

        /// <summary>
        /// Rotate the camera with the indicated degrees.
        /// </summary>
        /// <param name="yawChange">Yaw camera rotation.</param>
        /// <param name="pitchChange">Pitch camera rotation.</param>
        public void Rotate(float yawChange, float pitchChange)
        {
            _yaw += yawChange;
            _pitch += pitchChange;
        }

        #endregion

        #region Move Method

        /// <summary>
        /// Move the camera with the indicated translation.
        /// </summary>
        /// <param name="translation">Translation of the camera.</param>
        public void Move(Vector3 translation)
        {
            _translation += translation;
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Update the position and rotation of the camera and calculate the
        /// new value of the View matrix of the camera.
        /// </summary>
        public override void Update()
        {
            // Calculate the rotation of the camera
            Matrix rotation = Matrix.CreateFromYawPitchRoll(_yaw, _pitch, 0);

            // Calculate the new position of the camera
            _translation = Vector3.Transform(_translation, rotation);
            Position += _translation;
            _translation = Vector3.Zero;

            Vector3 foward = Vector3.Transform(Vector3.Forward, rotation);
            Target = Position + foward;

            // Update the View matrix of the camera
            Vector3 up = Vector3.Up;// Transform(Vector3.Up, rotation);
            View = Matrix.CreateLookAt(Position, Target, up);
        }

        #endregion
    }
}
