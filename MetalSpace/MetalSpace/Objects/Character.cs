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
    /// <summary>
    /// The <c>Character</c> class represents a basic class used by the players
    /// and enemies with the common behaviour of all of them.
    /// </summary>
    public class Character : MoveableObject
    {
        #region Fields

        /// <summary>
        /// X direction of the model.
        /// </summary>
        public enum XDirection
        {
            /// <summary>
            /// None direction (0).
            /// </summary>
            None,
            /// <summary>
            /// Left direction (-X).
            /// </summary>
            Left,
            /// <summary>
            /// Right direction (+X).
            /// </summary>
            Right
        }

        /// <summary>
        /// Y direction of the model.
        /// </summary>
        public enum YDirection
        {
            /// <summary>
            /// None direction (0).
            /// </summary>
            None,
            /// <summary>
            /// Up direction (+Y).
            /// </summary>
            Up,
            /// <summary>
            /// Down direction (-Y).
            /// </summary>
            Down
        }

        /// <summary>
        /// Store for the Visible property.
        /// </summary>
        protected bool _visible;

        /// <summary>
        /// Store for the Life property.
        /// </summary>
        protected int _life;

        /// <summary>
        /// Store for the MaxLife property.
        /// </summary>
        protected int _maxLife;

        /// <summary>
        /// Store for the Attack property.
        /// </summary>
        protected int _attack;

        /// <summary>
        /// Store for the LastPlayerXDirection property.
        /// </summary>
        protected XDirection _lastPlayerXDirection;

        /// <summary>
        /// Store for the PlayerXDirection property.
        /// </summary>
        protected XDirection _playerXDirection;

        /// <summary>
        /// Store for the LasPlayerYDirection property.
        /// </summary>
        protected YDirection _lastPlayerYDirection;

        /// <summary>
        /// Store for the PlayerYDirection property.
        /// </summary>
        protected YDirection _playerYDirection;

        /// <summary>
        /// Store for the DirectionIndicator property.
        /// </summary>
        protected int _directionIndicator;

        /// <summary>
        /// true if it is detected the beginning of a staircase, false otherwise.
        /// </summary>
        protected bool _startStaircase;

        /// <summary>
        /// true if it is detected the climbing of a staircase, false otherwise.
        /// </summary>
        protected bool _climbStaircase;

        /// <summary>
        /// true if it is detected the finishing of a staircase, false otherwise.
        /// </summary>
        protected bool _finishStaircase;

        protected bool _startDownStaircase;
        protected bool _climbDownStaircase;
        protected bool _finishDownStaircase;

        #endregion

        #region Properties

        /// <summary>
        /// Visible property
        /// </summary>
        /// <value>
        /// true if the current character is visible, false otherwise.
        /// </value>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// Life property
        /// </summary>
        /// <value>
        /// Amount of life that the character has.
        /// </value>
        public int Life
        {
            get { return _life; }
            set { _life = value; }
        }

        /// <summary>
        /// MaxLife property
        /// </summary>
        /// <value>
        /// Max amount of life that the character is able to have.
        /// </value>
        public int MaxLife
        {
            get { return _maxLife; }
            set { _maxLife = value; }
        }

        /// <summary>
        /// Attack property
        /// </summary>
        /// <value>
        /// Damage that the character can do.
        /// </value>
        public int Attack
        {
            get { return _attack; }
            set { _attack = value; }
        }

        /// <summary>
        /// LastPlayerXDirection property
        /// </summary>
        /// <value>
        /// Last X direction that the character had.
        /// </value>
        public XDirection LastPlayerXDirection
        {
            get { return _lastPlayerXDirection; }
            set { _lastPlayerXDirection = value; }
        }

        /// <summary>
        /// PlayerXDirection property
        /// </summary>
        /// <value>
        /// Current X direction of the character.
        /// </value>
        public XDirection PlayerXDirection
        {
            get { return _playerXDirection; }
            set { _playerXDirection = value; }
        }

        /// <summary>
        /// LastPlayerYDirection property
        /// </summary>
        /// <value>
        /// Last Y direction that the character had.
        /// </value>
        public YDirection LastPlayerYDirection
        {
            get { return _lastPlayerYDirection; }
            set { _lastPlayerYDirection = value; }
        }

        /// <summary>
        /// PlayerYDirection property
        /// </summary>
        /// <value>
        /// Current Y direction of the character.
        /// </value>
        public YDirection PlayerYDirection
        {
            get { return _playerYDirection; }
            set { _playerYDirection = value; }
        }

        /// <summary>
        /// DirectionIndicator property
        /// </summary>
        /// <value>
        /// Get a numeric indication of the X direction of the character (-1 left, 1 right, 0 none).
        /// </value>
        public int DirectionIndicator
        {
            get { return _directionIndicator; }
            set { _directionIndicator = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>Character</c> class.
        /// </summary>
        /// <param name="scene">Reference to the scene where the character moves.</param>
        /// <param name="model">Reference to the model that represents the character.</param>
        /// <param name="maxSpeed">Max speed of the character.</param>
        /// <param name="maxLife">Max life of the character.</param>
        /// <param name="attack">Attack of the character.</param>
        public Character(SceneRenderer scene, IDrawableModel model, 
            Vector2 maxSpeed, int maxLife, int life, int attack)
            : base(scene, model, maxSpeed, true, true)
        {
            _visible = true;

            _life = life;
            _maxLife = maxLife;

            _attack = attack;

            _startStaircase = false;
            _climbStaircase = false;
            _finishStaircase = false;

            _startDownStaircase = false;
            _climbDownStaircase = false;
            _finishDownStaircase = false;

            _lastPlayerXDirection = XDirection.Right;
            _playerXDirection = XDirection.Right;
            _lastPlayerYDirection = YDirection.None;
            _playerYDirection = YDirection.None;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load all needed content of the <c>Character</c>.
        /// </summary>
        public override void Load()
        {
            base.Load();
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload all needed content of the <c>Character</c>.
        /// </summary>
        public override void Unload()
        {
            base.Unload();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the <c>Character</c>.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion

        #region Hurt Method

        /// <summary>
        /// Reduce the life of the character.
        /// </summary>
        /// <param name="power">Amount of life to be reduced.</param>
        public void Hurt(int power)
        {
            _life -= power;
        }

        #endregion

        #region PreCollision Method

        /// <summary>
        /// Change the result of the collision test before apply the changes. In this
        /// class permits to apply the needed changed when it is detected a staircase.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        protected override void PreCollision(GameTime gameTime)
        {
            base.PreCollision(gameTime);
            
            OcTreeNode node1, node2;
            Vector3 checkPosition;

            if (_startStaircase)
            {
                checkPosition = _model.BSphere.Center;
                checkPosition.Y += -_model.BSphere.Radius;
                node1 = _scene.MainLayer.SearchNode(checkPosition);
                if (node1 != null)
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
                    if (_playerXDirection == XDirection.Left)
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

            if (_startDownStaircase)
            {
                checkPosition = _model.BSphere.Center;
                checkPosition.Y += -_model.BSphere.Radius;
                node1 = _scene.MainLayer.SearchNode(checkPosition);
                if (node1 != null)
                {
                    checkPosition.Y += -1f;
                    node2 = _scene.MainLayer.SearchNode(checkPosition);
                    if (node2 == null)
                    {
                        _climbDownStaircase = true;
                        _startDownStaircase = false;
                        EventManager.Trigger(new EventData_ObjectsCollision(this, ref node1,
                            EventData_ObjectsCollision.CollisionDirection.Down,
                            EventData_ObjectsCollision.CollisionSurface.Plane));
                    }
                }
            }
            else if (_finishDownStaircase)
            {
                checkPosition = _model.BSphere.Center;
                checkPosition.Y += -_model.BSphere.Radius;
                node1 = _scene.MainLayer.SearchNode(checkPosition);
                if (node1 == null)
                {
                    checkPosition.Y += 0.2f;
                    if (_playerXDirection == XDirection.Left)
                        checkPosition.X += -0.1f;
                    else
                        checkPosition.X += 0.1f;

                    node1 = _scene.MainLayer.SearchNode(checkPosition);
                    if (node1 != null)
                    {
                        _climbDownStaircase = false;
                        _finishDownStaircase = false;
                        EventManager.Trigger(new EventData_ObjectsCollision(this, ref node1,
                            EventData_ObjectsCollision.CollisionDirection.Down,
                            EventData_ObjectsCollision.CollisionSurface.Box));
                    }
                }
                else if (node1.ModelList[0].Key == NodeType.None)
                {
                    _climbDownStaircase = false;
                    _finishDownStaircase = false;
                    EventManager.Trigger(new EventData_ObjectsCollision(this, ref node1,
                        EventData_ObjectsCollision.CollisionDirection.Down,
                        EventData_ObjectsCollision.CollisionSurface.Box));
                }
            }

            if (_climbDownStaircase)
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

        /// <summary>
        /// Permits to detect elements in the scene to be considered in the next loop. In this
        /// class permits to detect a staircase.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        protected override void PostCollision(GameTime gameTime)
        {
            base.PostCollision(gameTime);

            Vector3 point1, point2;
            OcTreeNode node1, node2;
            
            if (!_climbStaircase)
            {
                // Detect left start staircase
                if (_playerXDirection == XDirection.Left ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Left))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += -_model.BSphere.Radius;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += -_model.BSphere.Radius;
                    point2.Y += -_model.BSphere.Radius;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    if (node1 != null && node2 != null)
                    {
                        if ((node1.ModelList[0].Key == NodeType.Staircase1Down || node1.ModelList[0].Key == NodeType.Staircase1Up ||
                             node1.ModelList[0].Key == NodeType.Staircase2Down || node1.ModelList[0].Key == NodeType.Staircase2Up) &&
                             node1.ModelList[0].Value.Rotation.X == MathHelper.ToRadians(90) &&
                            (node2.ModelList[0].Key == NodeType.Staircase1Down || node2.ModelList[0].Key == NodeType.Staircase1Up ||
                             node2.ModelList[0].Key == NodeType.Staircase2Down || node2.ModelList[0].Key == NodeType.Staircase2Up))
                        {
                            _startStaircase = true;
                            
                            Vector3 rotation = _model.Rotation;
                            _model.Rotation = new Vector3(MathHelper.ToRadians(270), rotation.Y, rotation.Z);

                            return;
                        }
                        else
                            _startStaircase = false;
                    }
                    else
                        _startStaircase = false;
                }

                // Detect right start staircase
                if (_playerXDirection == XDirection.Right ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Right))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += _model.BSphere.Radius;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += _model.BSphere.Radius;
                    point2.Y += -_model.BSphere.Radius;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    if (node1 != null && node2 != null)
                    {
                        if ((node1.ModelList[0].Key == NodeType.Staircase1Down || node1.ModelList[0].Key == NodeType.Staircase1Up ||
                            node1.ModelList[0].Key == NodeType.Staircase2Down || node1.ModelList[0].Key == NodeType.Staircase2Up) &&
                            node1.ModelList[0].Value.Rotation.X == MathHelper.ToRadians(270) &&
                            (node2.ModelList[0].Key == NodeType.Staircase1Down || node2.ModelList[0].Key == NodeType.Staircase1Up ||
                             node2.ModelList[0].Key == NodeType.Staircase2Down || node2.ModelList[0].Key == NodeType.Staircase2Up))
                        {
                            _startStaircase = true;

                            Vector3 rotation = _model.Rotation;
                            _model.Rotation = new Vector3(MathHelper.ToRadians(90), rotation.Y, rotation.Z);

                            return;
                        }
                        else
                            _startStaircase = false;
                    }
                    else
                        _startStaircase = false;
                }
            }
            else
            {
                // Detect left finnish staircase
                if (_playerXDirection == XDirection.Left ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Left))
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

                        Vector3 rotation = _model.Rotation;
                        _model.Rotation = new Vector3(MathHelper.ToRadians(270), rotation.Y, rotation.Z);

                        return;
                    }
                    else
                        _finishStaircase = false;
                }

                // Detect right finnish staircase
                if (_playerXDirection == XDirection.Right ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Right))
                {
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

                        Vector3 rotation = _model.Rotation;
                        _model.Rotation = new Vector3(MathHelper.ToRadians(90), rotation.Y, rotation.Z);

                        return;
                    }
                    else
                        _finishStaircase = false;
                }
            }

            if (!_climbDownStaircase)
            {
                // Detect left start down staircase
                if (_playerXDirection == XDirection.Left ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Left))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += -_model.BSphere.Radius;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += -_model.BSphere.Radius;
                    point2.Y += -_model.BSphere.Radius + 0.1f;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    if (node1 == null && node2 != null)
                    {
                        if (node2.ModelList[0].Key == NodeType.Staircase1Down || node2.ModelList[0].Key == NodeType.Staircase1Up ||
                            node2.ModelList[0].Key == NodeType.Staircase2Down || node2.ModelList[0].Key == NodeType.Staircase2Up)
                        {
                            _startDownStaircase = true;

                            Vector3 rotation = _model.Rotation;
                            _model.Rotation = new Vector3(MathHelper.ToRadians(270), rotation.Y, rotation.Z);

                            return;
                        }
                        else
                            _startDownStaircase = false;
                    }
                    else
                        _startDownStaircase = false;
                }

                // Detect right start down staircase
                if (_playerXDirection == XDirection.Right ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Right))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += _model.BSphere.Radius;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += _model.BSphere.Radius;
                    point2.Y += -_model.BSphere.Radius + 0.1f;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    if (node1 == null && node2 != null)
                    {
                        if (node2.ModelList[0].Key == NodeType.Staircase1Down || node2.ModelList[0].Key == NodeType.Staircase1Up ||
                            node2.ModelList[0].Key == NodeType.Staircase2Down || node2.ModelList[0].Key == NodeType.Staircase2Up)
                        {
                            _startDownStaircase = true;

                            Vector3 rotation = _model.Rotation;
                            _model.Rotation = new Vector3(MathHelper.ToRadians(90), rotation.Y, rotation.Z);

                            return;
                        }
                        else
                            _startDownStaircase = false;
                    }
                    else
                        _startDownStaircase = false;
                }
            }
            else
            {
                // Detect left finish down staircase
                if (_playerXDirection == XDirection.Left ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Left))
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
                        _finishDownStaircase = true;

                        Vector3 rotation = _model.Rotation;
                        _model.Rotation = new Vector3(MathHelper.ToRadians(270), rotation.Y, rotation.Z);

                        return;
                    }
                    else
                        _finishDownStaircase = false;
                }

                // Detect right finish down staircase
                if (_playerXDirection == XDirection.Right ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Right))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += _model.BSphere.Radius;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += _model.BSphere.Radius;
                    point2.Y += -_model.BSphere.Radius;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    if (node1 == null && node2 != null)
                    {
                        _finishDownStaircase = true;

                        Vector3 rotation = _model.Rotation;
                        _model.Rotation = new Vector3(MathHelper.ToRadians(90), rotation.Y, rotation.Z);

                        return;
                    }
                    else
                        _finishDownStaircase = false;
                }
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>Character</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            if(_visible)
                base.Draw(gameTime);
        }

        #endregion
    }
}
