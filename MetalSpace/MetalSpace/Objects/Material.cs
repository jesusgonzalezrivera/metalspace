using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Objects;
using MetalSpace.Scene;
using MetalSpace.Models;
using MetalSpace.Events;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Objects
{
    class Material : MoveableObject
    {
        #region Fields

        protected bool _visible;

        protected bool _startStaircase;
        protected bool _climbStaircase;
        protected bool _finishStaircase;

        #endregion

        #region Properties

        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        #endregion

        #region Constructor

        public Material(SceneRenderer scene, IDrawableModel model, Vector2 maxSpeed)
            : base(scene, model, maxSpeed, true, true)
        {
            _visible = true;

            _startStaircase = false;
            _climbStaircase = false;
            _finishStaircase = false;
        }

        #endregion

        #region Load Method

        public override void Load()
        {
            base.Load();
        }

        #endregion

        #region Unload Method

        public override void Unload()
        {
            base.Unload();
        }

        #endregion

        #region Update Method

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion

        #region PreCollision Method

        protected override void PreCollision(GameTime gameTime)
        {
            base.PreCollision(gameTime);

            OcTreeNode node1;
            Vector3 checkPosition;

            if (_startStaircase)
            {
                checkPosition = _model.BSphere.Center;
                checkPosition.Y += -_model.BSphere.Radius;
                node1 = _scene.MainLayer.SearchNode(checkPosition);
                if (node1 == null)
                {
                    checkPosition.Y += 1f;
                    node1 = _scene.MainLayer.SearchNode(checkPosition);
                    if (node1 != null)
                    {
                        _climbStaircase = true;
                        _startStaircase = false;
                        EventManager.Trigger(new EventData_ObjectsCollision(this, ref node1,
                            EventData_ObjectsCollision.CollisionDirection.Down,
                            EventData_ObjectsCollision.CollisionSurface.Plane));
                    }
                }
            }
            else if (_finishStaircase)
            {
                checkPosition = _model.BSphere.Center;
                checkPosition.Y += -_model.BSphere.Radius;
                node1 = _scene.MainLayer.SearchNode(checkPosition);
                if (node1 == null)
                {
                    checkPosition.Y += 0.2f;
                    if (Speed.X < 0)
                        checkPosition.X += -0.1f;
                    else
                        checkPosition.X += 0.1f;

                    node1 = _scene.MainLayer.SearchNode(checkPosition);
                    if (node1 != null)
                    {
                        _climbStaircase = false;
                        _finishStaircase = false;
                        EventManager.Trigger(new EventData_ObjectsCollision(this, ref node1,
                            EventData_ObjectsCollision.CollisionDirection.Down,
                            EventData_ObjectsCollision.CollisionSurface.Box));
                    }
                }
                else if (node1.ModelList[0].Key == NodeType.None)
                {
                    _climbStaircase = false;
                    _finishStaircase = false;
                    EventManager.Trigger(new EventData_ObjectsCollision(this, ref node1,
                        EventData_ObjectsCollision.CollisionDirection.Down,
                        EventData_ObjectsCollision.CollisionSurface.Box));
                }
            }

            if (_climbStaircase)
            {
                checkPosition = _model.BSphere.Center;
                node1 = _scene.MainLayer.SearchNode(checkPosition);
                if (node1 != null)
                {
                    EventManager.Trigger(new EventData_ObjectsCollision(this, ref node1,
                        EventData_ObjectsCollision.CollisionDirection.Down,
                        EventData_ObjectsCollision.CollisionSurface.Plane));
                }
            }
        }

        #endregion

        #region PostCollision Methd

        protected override void PostCollision(GameTime gameTime)
        {
            base.PostCollision(gameTime);

            Vector3 point1, point2;
            OcTreeNode node1, node2;

            if (!_climbStaircase)
            {
                point1 = _model.BSphere.Center;
                point1.X += -_model.BSphere.Radius;
                node1 = _scene.MainLayer.SearchNode(point1);

                point2 = _model.BSphere.Center;
                point2.X += -_model.BSphere.Radius;
                point2.Y += -_model.BSphere.Radius;
                node2 = _scene.MainLayer.SearchNode(point2);

                if (node1 != null && node2 == null)
                {
                    if (node1.ModelList[0].Key == NodeType.Staircase1Down || node1.ModelList[0].Key == NodeType.Staircase1Up ||
                       node1.ModelList[0].Key == NodeType.Staircase2Down || node1.ModelList[0].Key == NodeType.Staircase2Up)
                    {
                        _startStaircase = true;

                        return;
                    }
                    else
                        _startStaircase = false;
                }
                else
                    _startStaircase = false;

                point1 = _model.BSphere.Center;
                point1.X += _model.BSphere.Radius;
                node1 = _scene.MainLayer.SearchNode(point1);

                point2 = _model.BSphere.Center;
                point2.X += _model.BSphere.Radius;
                point2.Y += -_model.BSphere.Radius;
                node2 = _scene.MainLayer.SearchNode(point2);

                if (node1 != null && node2 == null)
                {
                    if (node1.ModelList[0].Key == NodeType.Staircase1Down || node1.ModelList[0].Key == NodeType.Staircase1Up ||
                       node1.ModelList[0].Key == NodeType.Staircase2Down || node1.ModelList[0].Key == NodeType.Staircase2Up)
                    {
                        _startStaircase = true;

                        return;
                    }
                    else
                        _startStaircase = false;
                }
                else
                    _startStaircase = false;
            }
            else
            {
                point1 = _model.BSphere.Center;
                point1.X += -_model.BSphere.Radius;
                node1 = _scene.MainLayer.SearchNode(point1);

                point2 = _model.BSphere.Center;
                point2.X += -_model.BSphere.Radius;
                point2.Y += -_model.BSphere.Radius;
                node2 = _scene.MainLayer.SearchNode(point2);

                if (node1 == null && node2 != null && node2.ModelList[0].Key == NodeType.None)
                {
                    _finishStaircase = true;

                    return;
                }
                else
                    _finishStaircase = false;

                point1 = _model.BSphere.Center;
                point1.X += _model.BSphere.Radius;
                node1 = _scene.MainLayer.SearchNode(point1);

                point2 = _model.BSphere.Center;
                point2.X += _model.BSphere.Radius;
                point2.Y += -_model.BSphere.Radius;
                node2 = _scene.MainLayer.SearchNode(point2);

                if (node1 == null && node2 != null)
                {
                    _finishStaircase = true;

                    return;
                }
                else
                    _finishStaircase = false;
            }
        }

        #endregion

        #region Draw Method

        public override void Draw(GameTime gameTime)
        {
            if(_visible)
                base.Draw(gameTime);
        }

        #endregion
    }
}
