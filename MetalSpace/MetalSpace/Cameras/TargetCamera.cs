using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MetalSpace.Cameras
{
    /// <summary>
    /// The <c>TargetCamera</c> class represents a camera that see an objetive
    /// point from a source point.
    /// </summary>
    class TargetCamera : Camera
    {
        /// <summary>
        /// Constructor of the <c>TargetCamera</c> class.
        /// </summary>
        /// <param name="position">Position of the camera.</param>
        /// <param name="target">Target of the camera.</param>
        public TargetCamera(Vector3 position, Vector3 target)
            : base()
        {
            this.Position = position;
            this.Target = target;
        }

        /// <summary>
        /// Update the View matrix of the camera.
        /// </summary>
        public override void Update()
        {
            Vector3 forward = this.Target - this.Position;
            Vector3 side = Vector3.Cross(forward, Vector3.Up);
            Vector3 up = Vector3.Cross(forward, side);
            this.View = Matrix.CreateLookAt(this.Position, this.Target, up);
        }
    }
}