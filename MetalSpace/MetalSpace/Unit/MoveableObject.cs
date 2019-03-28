using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Scene;
using MetalSpace.Models;
using MetalSpace.Events;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Unit
{
    public class MoveableObject
    {
        #region Fields

        private bool _checkGravity;
        private bool _checkCollision;

        protected OcTree _octree;

        protected IDrawableModel _model;

        protected Vector3 _correctPosition;

        protected Vector2 _speed;
        protected Vector2 _maxSpeed;

        #endregion

        #region Properties

        public bool CheckGravity
        {
            get { return _checkGravity; }
            set { _checkGravity = value; }
        }

        public bool CheckCollision
        {
            get { return _checkCollision; }
            set { _checkCollision = value; }
        }

        public IDrawableModel DModel
        {
            get { return _model; }
            set { _model = value; }
        }

        public Vector3 Position
        {
            get { return _model.Position; }
            set { _model.Position = value; }
        }

        public Vector3 CPosition
        {
            get { return _correctPosition; }
            set { _correctPosition = value; }
        }

        public Vector3 Rotation
        {
            get { return _model.Rotation; }
            set { _model.Rotation = value; }
        }

        public Vector3 Scale
        {
            get { return _model.Scale; }
            set { _model.Scale = value; }
        }

        public Vector2 Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public Vector2 MaxSpeed
        {
            get { return _maxSpeed; }
            set { _maxSpeed = value; }
        }

        #endregion

        #region Constructor

        public MoveableObject(OcTree octree, IDrawableModel model, Vector2 maxSpeed, 
            bool checkGravity = true, bool checkCollision = true)
        {
            _octree = octree;

            _model = model;

            _speed = new Vector2(0.1f, 0.1f);
            _maxSpeed = maxSpeed;

            _correctPosition = _model.Position;

            _checkGravity = checkGravity;
            _checkCollision = checkCollision;
        }

        #endregion

        #region Load Method

        public virtual void Load()
        {

        }

        #endregion

        #region Unload Method

        public virtual void Unload()
        {
            _correctPosition = Vector3.Zero;

            Speed = Vector2.Zero;
            MaxSpeed = Vector2.Zero;

            _model.Unload();
            _model = null;

            _octree = null;
        }

        #endregion

        #region Update Method

        public virtual void Update(GameTime gameTime)
        {
            _model.Update(gameTime);

            if (_checkGravity)
            {
                float time = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

                _speed.Y += -9.81f * time;
                _speed = new Vector2((float)Math.Round(Speed.X, 3), (float)Math.Round(Speed.Y, 3));

                _correctPosition += new Vector3(
                    Speed.X * time,
                    Speed.Y * time - (0.5f * 9.8f * (float)Math.Pow(time, 2)),
                    0);
            }

            if (_checkCollision)
                Collision(gameTime);
        }

        #endregion

        #region Collision Method

        protected bool _upCollision = false;
        protected bool _downCollision = false;
        protected bool _leftCollision = false;
        protected bool _rightCollision = false;

        protected virtual void Collision(GameTime time)
        {
            _model.BSphere = ((DrawableModel)_model).GetBoundingSphere();

            //Check down collision
            Vector3 downPosition = _model.BSphere.Center;
            downPosition.Y += - _model.BSphere.Radius;
            OcTreeNode node = _octree.SearchNode(downPosition);
            Console.WriteLine("SERA ESTO: " + (node == null ? "NULL" : node.ModelList[0].Key.ToString()));
            if (node != null)
            {
                if (node.ModelList[0].Key == NodeType.Staircase1Down || node.ModelList[0].Key == NodeType.Staircase1Up ||
                    node.ModelList[0].Key == NodeType.Staircase2Down || node.ModelList[0].Key == NodeType.Staircase2Up)
                {
                    EventManager.Trigger(new EventData_UnitCollision(this, ref node,
                                EventData_UnitCollision.CollisionDirection.Down, EventData_UnitCollision.CollisionSurface.Plane));

                    _downCollision = true;
                }
                else if (node.ModelList[0].Key != NodeType.Ladder)
                {
                    EventManager.Trigger(new EventData_UnitCollision(this, ref node,
                                EventData_UnitCollision.CollisionDirection.Down, EventData_UnitCollision.CollisionSurface.Box));

                    _downCollision = true;
                }
            }
            else
                _downCollision = false;

            // Check up collision
            Vector3 upPosition = _model.BSphere.Center;
            upPosition.Y += _model.BSphere.Radius;
            node = _octree.SearchNode(upPosition);
            if (node != null)
            {
                if (node.ModelList[0].Key != NodeType.Ladder)
                {
                    _speed.Y = 0;
                    EventManager.Trigger(new EventData_UnitCollision(this, ref node,
                        EventData_UnitCollision.CollisionDirection.Up, EventData_UnitCollision.CollisionSurface.Box,
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
            node = _octree.SearchNode(leftPosition);
            if (node != null)
            {
                if (node.ModelList[0].Key == NodeType.None)
                {
                    _speed.X = 0;
                    EventManager.Trigger(new EventData_UnitCollision(this, ref node,
                        EventData_UnitCollision.CollisionDirection.Left, EventData_UnitCollision.CollisionSurface.Box,
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
            node = _octree.SearchNode(rightPosition);
            if (node != null)
            {
                if (node.ModelList[0].Key == NodeType.None)
                {
                    _speed.X = 0;
                    EventManager.Trigger(new EventData_UnitCollision(this, ref node,
                        EventData_UnitCollision.CollisionDirection.Right, EventData_UnitCollision.CollisionSurface.Box,
                        -(_model.BSphere.Radius / 6.0f) - 0.1f));

                    _rightCollision = true;
                }
                else
                    _rightCollision = false;
            }
            else
                _rightCollision = false;

            PreCollision();

            Position = _correctPosition;

            PostCollision();
        }

        #endregion

        #region PreCollision Method

        protected virtual void PreCollision()
        {

        }

        #endregion

        #region PostCollision Method

        protected virtual void PostCollision()
        {

        }

        #endregion

        #region Draw Method

        public virtual void Draw(GameTime time)
        {
            _model.Draw(CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection,
                DrawingMethod.HardwareInstancing, new Dictionary<string, Matrix>(), null);
        }

        #endregion
    }
}
