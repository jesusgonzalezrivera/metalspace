using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Scene;
using MetalSpace.Models;
using MetalSpace.Events;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Objects
{
    /// <summary>
    /// The <c>MoveableObject</c> class represents an individual object that can
    /// move along the scene, checking gravity and basic collisions (without staircase
    /// and stairs).
    /// </summary>
    public class MoveableObject
    {
        #region Fields

        /// <summary>
        /// Store for the CheckGravity property.
        /// </summary>
        protected bool _checkGravity;

        /// <summary>
        /// Store for the CheckCollision property.
        /// </summary>
        protected bool _checkCollision;

        /// <summary>
        /// Reference to the scene where the object moves.
        /// </summary>
        protected SceneRenderer _scene;

        /// <summary>
        /// Store for the DModel property.
        /// </summary>
        protected IDrawableModel _model;

        /// <summary>
        /// Store for the CPosition property.
        /// </summary>
        protected Vector3 _correctPosition;

        /// <summary>
        /// Store for the Speed property.
        /// </summary>
        protected Vector2 _speed;

        /// <summary>
        /// Store for the MaxSpeed property.
        /// </summary>
        protected Vector2 _maxSpeed;

        /// <summary>
        /// true if a collision has ocurred in the upper part, false otherwise.
        /// </summary>
        protected bool _upCollision = false;

        /// <summary>
        /// true if a collision has ocurred at the bottom part, false otherwise.
        /// </summary>
        protected bool _downCollision = false;

        /// <summary>
        /// true if a collision has ocurred in the left part, false otherwise.
        /// </summary>
        protected bool _leftCollision = false;

        /// <summary>
        /// true if a collision has ocurred in the right part, false otherwise.
        /// </summary>
        protected bool _rightCollision = false;

        #endregion

        #region Properties

        /// <summary>
        /// CheckGravity property
        /// </summary>
        /// <value>
        /// true if it is necessary to update the position with gravity, false otherwise.
        /// </value>
        public bool CheckGravity
        {
            get { return _checkGravity; }
            set { _checkGravity = value; }
        }

        /// <summary>
        /// CheckCollision property
        /// </summary>
        /// <value>
        /// true if it is necessary to update the position detecting collision with the scene, false otherwise.
        /// </value>
        public bool CheckCollision
        {
            get { return _checkCollision; }
            set { _checkCollision = value; }
        }

        /// <summary>
        /// DModel property
        /// </summary>
        /// <value>
        /// Reference to the model in the <c>ModelManager</c>.
        /// </value>
        public IDrawableModel DModel
        {
            get { return _model; }
            set { _model = value; }
        }

        /// <summary>
        /// Position property
        /// </summary>
        /// <value>
        /// Position of the model in the scene.
        /// </value>
        public Vector3 Position
        {
            get { return _model.Position; }
            set { _model.Position = value; }
        }

        /// <summary>
        /// CPosition property
        /// </summary>
        /// <value>
        /// Position of the model after checking gravity and collisions.
        /// </value>
        public Vector3 CPosition
        {
            get { return _correctPosition; }
            set { _correctPosition = value; }
        }

        /// <summary>
        /// Rotation property
        /// </summary>
        /// <value>
        /// Rotation of the model in the scene.
        /// </value>
        public Vector3 Rotation
        {
            get { return _model.Rotation; }
            set { _model.Rotation = value; }
        }

        /// <summary>
        /// Scale property
        /// </summary>
        /// <value>
        /// Scale of the model in the scene.
        /// </value>
        public Vector3 Scale
        {
            get { return _model.Scale; }
            set { _model.Scale = value; }
        }

        /// <summary>
        /// Speed property
        /// </summary>
        /// <value>
        /// Current speed of the model when moves.
        /// </value>
        public Vector2 Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        /// <summary>
        /// MaxSpeed property
        /// </summary>
        /// <value>
        /// Maximun speed of the model when moves.
        /// </value>
        public Vector2 MaxSpeed
        {
            get { return _maxSpeed; }
            set { _maxSpeed = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>MoveableObject</c> class.
        /// </summary>
        /// <param name="scene">Reference to the scene where the character moves.</param>
        /// <param name="model">Reference to the model that represents the object.</param>
        /// <param name="maxSpeed">Max speed of the character.</param>
        /// <param name="checkGravity">true if it is necessary to check gravity, false otherwise.</param>
        /// <param name="checkCollision">true if it is necessary to check collisions, false otherwise.</param>
        public MoveableObject(SceneRenderer scene, IDrawableModel model, Vector2 maxSpeed, 
            bool checkGravity = true, bool checkCollision = true)
        {
            _scene = scene;

            _model = model;

            _speed = new Vector2(0.1f, 0.1f);//Vector2.Zero;
            _maxSpeed = maxSpeed;

            _correctPosition = _model.Position;

            _checkGravity = checkGravity;
            _checkCollision = checkCollision;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load all needed content of the <c>MoveableObject</c>.
        /// </summary>
        public virtual void Load()
        {

        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload all needed content of the <c>MoveableObject</c>.
        /// </summary>
        public virtual void Unload()
        {
            _correctPosition = Vector3.Zero;

            Speed = Vector2.Zero;
            MaxSpeed = Vector2.Zero;

            _model.Unload();
            _model = null;

            _scene = null;
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the state of the <c>MoveableObject</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public virtual void Update(GameTime gameTime)
        {
            if (_checkGravity)
            {
                float time = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

                _speed.Y += -9.81f * time;
                _speed = new Vector2((float)Math.Round(Speed.X, 3), (float)Math.Round(Speed.Y, 3));

                _correctPosition += new Vector3(
                    Speed.X * time,
                    Speed.Y * time - (0.5f * 9.81f * (float)Math.Pow(time, 2)),
                    0);
            }
            
            if (_checkCollision)
                Collision(gameTime);
            else
                Position = _correctPosition;

            _model.Update(gameTime);
        }

        #endregion

        #region Collision Method

        /// <summary>
        /// Check collision of the moveable model with the scene.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        protected virtual void Collision(GameTime gameTime)
        {
            _model.BSphere = ((DrawableModel)_model).GetBoundingSphere();
            _model.BSphere = new BoundingSphere(_model.BSphere.Center, 1.0f);

            //Check down collision
            Vector3 downPosition = _model.BSphere.Center;
            downPosition.Y += - _model.BSphere.Radius;
            OcTreeNode node = _scene.MainLayer.SearchNode(downPosition);
            if (node != null)
            {
                if (node.ModelList[0].Key == NodeType.Staircase1Down || node.ModelList[0].Key == NodeType.Staircase1Up ||
                    node.ModelList[0].Key == NodeType.Staircase2Down || node.ModelList[0].Key == NodeType.Staircase2Up)
                {
                    _speed.Y = 0;
                    EventManager.Trigger(new EventData_ObjectsCollision(this, ref node,
                        EventData_ObjectsCollision.CollisionDirection.Down, EventData_ObjectsCollision.CollisionSurface.Plane));

                    _downCollision = true;
                }
                else if (node.ModelList[0].Key != NodeType.Ladder)
                {
                    _speed.Y = 0;
                    EventManager.Trigger(new EventData_ObjectsCollision(this, ref node,
                        EventData_ObjectsCollision.CollisionDirection.Down, EventData_ObjectsCollision.CollisionSurface.Box));

                    _downCollision = true;
                }
            }
            else
                _downCollision = false;
            
            foreach (GameLayer2D layer in _scene.Layers2D)
            {
                if (DModel.BSphere.Intersects(layer.BoundingPlane) == PlaneIntersectionType.Intersecting)
                {
                    EventManager.Trigger(new EventData_PlaneCollision(this, layer.BoundingPlane, 0));

                    break;
                }
            }

            // Check up collision
            Vector3 upPosition = _model.BSphere.Center;
            upPosition.Y += _model.BSphere.Radius;
            node = _scene.MainLayer.SearchNode(upPosition);
            if (node != null)
            {
                if (node.ModelList[0].Key != NodeType.Ladder)
                {
                    _speed.Y = 0;
                    EventManager.Trigger(new EventData_ObjectsCollision(this, ref node,
                        EventData_ObjectsCollision.CollisionDirection.Up, EventData_ObjectsCollision.CollisionSurface.Box,
                        -_model.BSphere.Radius * 2));

                    _upCollision = true;
                }
                else
                    _upCollision = false;
            }
            else
                _upCollision = false;

            // Check left collision
            Vector3 leftPosition = _model.BSphere.Center;
            leftPosition.X += - _model.BSphere.Radius / 6.0f;
            node = _scene.MainLayer.SearchNode(leftPosition);
            if (node != null)
            {
                if (node.ModelList[0].Key == NodeType.None)
                {
                    _speed.X = 0;
                    EventManager.Trigger(new EventData_ObjectsCollision(this, ref node,
                        EventData_ObjectsCollision.CollisionDirection.Left, EventData_ObjectsCollision.CollisionSurface.Box,
                        _model.BSphere.Radius / 6.0f));

                    _leftCollision = true;
                }
                else
                    _leftCollision = false;
            }
            else
                _leftCollision = false;

            // Check right collision
            Vector3 rightPosition = _model.BSphere.Center;
            rightPosition.X += _model.BSphere.Radius / 6.0f;
            node = _scene.MainLayer.SearchNode(rightPosition);
            if (node != null)
            {
                if (node.ModelList[0].Key == NodeType.None)
                {
                    _speed.X = 0;
                    EventManager.Trigger(new EventData_ObjectsCollision(this, ref node,
                        EventData_ObjectsCollision.CollisionDirection.Right, EventData_ObjectsCollision.CollisionSurface.Box,
                        -(_model.BSphere.Radius / 6.0f) - 0.1f));

                    _rightCollision = true;
                }
                else
                    _rightCollision = false;
            }
            else
                _rightCollision = false;

            PreCollision(gameTime);
            
            Position = _correctPosition;
            
            PostCollision(gameTime);
        }

        #endregion

        #region PreCollision Method

        /// <summary>
        /// Change the result of the collision test before apply the changes.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        protected virtual void PreCollision(GameTime gameTime)
        {

        }

        #endregion

        #region PostCollision Method

        /// <summary>
        /// Permits to detect elements in the scene to be considered in the next loop.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        protected virtual void PostCollision(GameTime gameTime)
        {

        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>MoveableObject</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public virtual void Draw(GameTime gameTime)
        {
            _model.Draw(CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection,
                DrawingMethod.HardwareInstancing, new Dictionary<string, Matrix>(), null);
        }

        #endregion
    }
}
