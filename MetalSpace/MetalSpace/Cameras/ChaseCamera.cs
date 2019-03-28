using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Managers;
using MetalSpace.Events;
using MetalSpace.Objects;

namespace MetalSpace.Cameras
{
    /// <summary>
    /// The <c>ChaseCamera</c> class represents a camera that follow the player
    /// and its movement.
    /// </summary>
    class ChaseCamera : Camera
    {
        #region Fields

        /// <summary>
        /// BoundingBox that represent the limits of the map to avoid the camera
        /// movement in the border of the map.
        /// </summary>
        private BoundingBox _mapLimit;

        /// <summary>
        /// Depth of the frontal layer of the map.
        /// </summary>
        private float _depth;

        /// <summary>
        /// Corner points of the BoundingFrustum of the camera.
        /// </summary>
        private Vector3[] _points;

        /// <summary>
        /// X coordinate of the center point of the camera.
        /// </summary>
        private float _centerX;

        /// <summary>
        /// Y coordinate of the center point of the camera.
        /// </summary>
        private float _centerY;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>ChaseCamera</c> class.
        /// </summary>
        /// <param name="position">Position point of the camera.</param>
        /// <param name="target">Target point of the camera.</param>
        /// <param name="origin">Origin point of the map.</param>
        /// <param name="rows">Total rows of the map.</param>
        /// <param name="cols">Total cols ofthe map.</param>
        /// <param name="depth">Total depth of the map.</param>
        public ChaseCamera(Vector3 position, Vector3 target, Vector3 origin, 
            int rows, int cols, int depth)
            : base(position, target)
        {
            _depth = depth;

            _mapLimit = new BoundingBox(
                new Vector3(origin.X, origin.Y, origin.Z - 0.1f), 
                new Vector3(origin.X + cols, origin.Y + rows, origin.Z + 0.1f));
            
            _points = new Vector3[8];
            for (int i = 0; i < 8; i++)
                _points[i] = Vector3.Zero;

            _centerX = 0f;
            _centerY = 0f;
        }

        #endregion

        #region Move

        /// <summary>
        /// Move the camera with the player movement. If the camera reachs the
        /// limits of the map, will be prevented its movement outside these
        /// limits.
        /// </summary>
        /// <param name="player">Reference to the player.</param>
        /// <param name="movement">The relative movement of the player.</param>
        public void Move(Player player, Vector3 movement)
        {
            float xPosition = Position.X + movement.X;
            float yPosition = Position.Y + movement.Y;
            float xTarget = Target.X + movement.X;
            float yTarget = Target.Y + movement.Y;

            float t = (_depth - _points[3].Z) / (_points[3].Z - _points[7].Z);
            float xMin = _points[3].X + ((_points[7].X - _points[3].X) * t);
            float yMin = _points[3].Y + ((_points[7].Y - _points[3].Y) * t);

            t = (_depth - _points[1].Z) / (_points[1].Z - _points[5].Z);
            float xMax = _points[1].X + ((_points[5].X - _points[1].X) * t);
            float yMax = _points[1].Y + ((_points[5].Y - _points[1].Y) * t);

            BoundingBox box = new BoundingBox(
                new Vector3(xMax, yMax, _depth - 0.05f), 
                new Vector3(xMin, yMin, _depth + 0.05f));

            bool playerMoved = false;
            if (box.Min.X <= _mapLimit.Min.X && player.Position.X >= _centerX)
            {
                playerMoved = true;
                xPosition = player.Position.X;
                xTarget = player.Position.X;
            }
            else if (box.Max.X >= _mapLimit.Max.X && player.Position.X <= _centerX)
            {
                playerMoved = true;
                xPosition = player.Position.X;
                xTarget = player.Position.X;
            }

            if (box.Min.Y <= _mapLimit.Min.Y && player.Position.Y + 1f >= _centerY)
            {
                playerMoved = true;
                yPosition = player.Position.Y + 1f;
                yTarget = player.Position.Y + 1f;
            }
            else if (box.Max.Y >= _mapLimit.Max.Y && player.Position.Y + 1f <= _centerY)
            {
                playerMoved = true;
                yPosition = player.Position.Y + 1f;
                yTarget = player.Position.Y + 1f;
            }
            
            if (!playerMoved)
            {
                if (box.Min.X <= _mapLimit.Min.X)
                {
                    xPosition -= movement.X;
                    xTarget -= movement.X;
                    _centerX = ((box.Max.X - box.Min.X) / 2f) + _mapLimit.Min.X;
                }
                else if (box.Max.X >= _mapLimit.Max.X)
                {
                    xPosition -= movement.X;
                    xTarget -= movement.X;
                    _centerX = ((box.Max.X - box.Min.X) / 2f) + box.Min.X;
                }
                else
                    _centerX = ((box.Max.X - box.Min.X) / 2f) + box.Min.X;

                if (box.Min.Y <= _mapLimit.Min.Y)
                {
                    yPosition -= movement.Y;
                    yTarget -= movement.Y;
                    _centerY = (box.Max.Y - box.Min.Y) / 2f + +_mapLimit.Min.Y;
                }
                else if (box.Max.Y >= _mapLimit.Max.Y)
                {
                    yPosition -= movement.Y;
                    yTarget -= movement.Y;
                    _centerY = ((box.Max.Y - box.Min.Y) / 2f) + box.Min.Y;
                }
                else
                    _centerY = ((box.Max.Y - box.Min.Y) / 2f) + box.Min.Y;
            }

            Position = new Vector3(xPosition, yPosition, Position.Z);
            Target = new Vector3(xTarget, yTarget, Target.Z);
        }

        #endregion

        #region Update

        /// <summary>
        /// Update the View matrix of the camera and re-obtain the points of the
        /// BoundingFrustum of the camera.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            View = Matrix.CreateLookAt(Position, Target, Vector3.Up);

            _points = new BoundingFrustum(View * Projection).GetCorners();
        }

        #endregion
    }
}
