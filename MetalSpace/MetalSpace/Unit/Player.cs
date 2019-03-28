using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Scene;
using MetalSpace.Models;
using MetalSpace.Events;
using MetalSpace.Managers;

namespace MetalSpace.Unit
{
    public class Player : Character
    {
        #region Fields

        public enum XDirection
        {
            None,
            Left,
            Right
        }

        public enum YDirection
        {
            None,
            Up,
            Down
        }

        public enum State
        {
            Waiting,
            Running,
            Jumping,
            Ducking,
            Climbing
        }

        public enum Action
        {
            None,
            Wounded, // Herido
            Dead,

            DuckingUp,
            DuckingDown,
            DuckingLeft,
            DuckingRight,

            ClimbingUp,
            ClimbingDown,

            GunAttacking,
            GunReloading,
            GunChanging,

            GrenadeAttacking,

            ShotgunAttacking,
            ShotgunReloading,
            ShotgunChanging,

            MachineGunAttacking,
            MachineGunReloading,
            MachineGunChanging,

            LaserGunAttacking,
            LaserGunReloading,
            LaserGunChanging
        }

        private State _lastPlayerState;
        private State _playerState;

        private Action _lastPlayerAction;
        private Action _playerAction;

        private XDirection _lastPlayerXDirection;
        private XDirection _playerXDirection;

        private YDirection _lastPlayerYDirection;
        private YDirection _playerYDirection;
        
        private int _directionIndicator;
        
        private bool _jump;
        private bool _endJump;

        #endregion

        #region Properties

        public State LastPlayerState
        {
            get { return _lastPlayerState; }
            set { _lastPlayerState = value; }
        }

        public State PlayerState
        {
            get { return _playerState; }
            set { _playerState = value; }
        }

        public Action LastPlayerAction
        {
            get { return _lastPlayerAction; }
            set { _lastPlayerAction = value; }
        }

        public Action PlayerAction
        {
            get { return _playerAction; }
            set { _playerAction = value; }
        }

        public XDirection LastPlayerXDirection
        {
            get { return _lastPlayerXDirection; }
            set { _lastPlayerXDirection = value; }
        }

        public XDirection PlayerXDirection
        {
            get { return _playerXDirection; }
            set { _playerXDirection = value; }
        }

        public YDirection LastPlayerYDirection
        {
            get { return _lastPlayerYDirection; }
            set { _lastPlayerYDirection = value; }
        }

        public YDirection PlayerYDirection
        {
            get { return _playerYDirection; }
            set { _playerYDirection = value; }
        }

        public int DirectionIndicator
        {
            get { return _directionIndicator; }
            set { _directionIndicator = value; }
        }

        public bool Jump
        {
            get { return _jump; }
            set { _jump = value; }
        }

        public bool EndJump
        {
            get { return _endJump; }
            set { _endJump = value; }
        }

        #endregion

        #region Constructor

        public Player(OcTree octree, string modelName, Vector3 position, Vector3 rotation,
            Vector3 scale, Vector2 maxSpeed, int maxLife, int attack)
            : base(octree, new AnimatedModel((GameModel)ModelManager.GetModel(modelName),
                position, rotation, scale, 0), maxSpeed, maxLife, attack)
        {
            _jump = false;
            _endJump = true;

            _lastPlayerState = State.Waiting;
            _playerState = State.Waiting;

            _lastPlayerAction = Action.None;
            _playerAction = Action.None;

            _lastPlayerXDirection = XDirection.Right;
            _playerXDirection = XDirection.Right;
            _lastPlayerYDirection = YDirection.None;
            _playerYDirection = YDirection.None;

            ((AnimatedModel) this.DModel).Animation.StartClip("Waiting", true);
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
            float time = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

            if (PlayerState == State.Waiting)
                _speed.X = 0;

            if (PlayerState == State.Running)
            {
                if (_speed.X != MaxSpeed.X * _directionIndicator)
                    _speed.X = MaxSpeed.X * _directionIndicator;
            }

            if (PlayerState == State.Jumping)
            {
                if (PlayerXDirection != XDirection.None)
                {
                    if (Speed.X != MaxSpeed.X * _directionIndicator)
                        _speed.X = MaxSpeed.X * _directionIndicator;
                }

                if (_jump == true && _endJump == false)
                {
                    _jump = false;
                    _speed.Y = 7f;
                }
            }

            base.Update(gameTime);
        }

        #endregion

        #region PreCollision Method

        protected override void PreCollision()
        {
            base.PreCollision();

            OcTreeNode node;
            Vector3 downPosition;
            
            if (_startStaircase)
            {
                Console.WriteLine("INICIO ESCALERA");
                downPosition = _model.BSphere.Center;
                downPosition.Y += -_model.BSphere.Radius;
                node = _octree.SearchNode(downPosition);
                if (node == null)
                {
                    Console.WriteLine("INICIO REAL");
                    _climbStaircase = true;
                    _startStaircase = false;

                    downPosition.Y += 0.2f;
                    node = _octree.SearchNode(downPosition);
                    EventManager.Trigger(new EventData_UnitCollision(this, ref node,
                        EventData_UnitCollision.CollisionDirection.Down, 
                        EventData_UnitCollision.CollisionSurface.Plane));
                }
            }
            else if (_finnishStaircase)
            {
                Console.WriteLine("FINAL ESCALERA");
                downPosition = _model.BSphere.Center;
                downPosition.Y += -_model.BSphere.Radius;
                node = _octree.SearchNode(downPosition);
                if (node == null)
                {
                    Console.WriteLine("FIN REAL1");
                    _climbStaircase = false;
                    _finnishStaircase = false;

                    downPosition.Y += 0.2f;
                    if (_playerXDirection == XDirection.Left)
                        downPosition.X += -0.1f;
                    else
                        downPosition.X += 0.1f;

                    node = _octree.SearchNode(downPosition);
                    EventManager.Trigger(new EventData_UnitCollision(this, ref node,
                        EventData_UnitCollision.CollisionDirection.Down,
                        EventData_UnitCollision.CollisionSurface.Box));
                }
                else if (node.ModelList[0].Key == NodeType.None)
                {
                    Console.WriteLine("FIN REAL2");
                    _climbStaircase = false;
                    _finnishStaircase = false;
                    EventManager.Trigger(new EventData_UnitCollision(this, ref node,
                        EventData_UnitCollision.CollisionDirection.Down,
                        EventData_UnitCollision.CollisionSurface.Box));
                }
            }
            
            if (_climbStaircase)
            {
                Console.WriteLine("CLIMBING ESCALERA");
                downPosition = _model.BSphere.Center;
                node = _octree.SearchNode(downPosition);
                if (node != null)
                {
                    EventManager.Trigger(new EventData_UnitCollision(this, ref node,
                        EventData_UnitCollision.CollisionDirection.Down,
                        EventData_UnitCollision.CollisionSurface.Plane));
                }
            }
        }

        #endregion

        #region PostCollision Method

        private bool _startStaircase = false;
        private bool _climbStaircase = false;
        private bool _finnishStaircase = false;

        protected override void PostCollision()
        {
            base.PostCollision();

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
                    node1 = _octree.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += -_model.BSphere.Radius;
                    point2.Y += -_model.BSphere.Radius;
                    node2 = _octree.SearchNode(point2);

                    if (node1 != null && node2 == null)
                    {
                        if (node1.ModelList[0].Key == NodeType.Staircase1Down || node1.ModelList[0].Key == NodeType.Staircase1Up ||
                           node1.ModelList[0].Key == NodeType.Staircase2Down || node1.ModelList[0].Key == NodeType.Staircase2Up)
                        {
                            Console.WriteLine("DETECTADO INICIO ESCALERA1");
                            _startStaircase = true;

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
                    node1 = _octree.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += _model.BSphere.Radius;
                    point2.Y += -_model.BSphere.Radius;
                    node2 = _octree.SearchNode(point2);

                    if (node1 != null && node2 == null)
                    {
                        if (node1.ModelList[0].Key == NodeType.Staircase1Down || node1.ModelList[0].Key == NodeType.Staircase1Up ||
                           node1.ModelList[0].Key == NodeType.Staircase2Down || node1.ModelList[0].Key == NodeType.Staircase2Up)
                        {
                            Console.WriteLine("DETECTADO INICIO ESCALERA2");
                            _startStaircase = true;

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
                    node1 = _octree.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += -_model.BSphere.Radius;
                    point2.Y += -_model.BSphere.Radius;
                    node2 = _octree.SearchNode(point2);

                    if (node1 == null && node2 != null && node2.ModelList[0].Key == NodeType.None)
                    {
                        Console.WriteLine("DETECTADO FIN ESCALERA1");
                        _finnishStaircase = true;

                        return;
                    }
                    else
                        _finnishStaircase = false;
                }

                // Detect right finnish staircase
                if (_playerXDirection == XDirection.Right ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Right))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += _model.BSphere.Radius;
                    node1 = _octree.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += _model.BSphere.Radius;
                    point2.Y += -_model.BSphere.Radius;
                    node2 = _octree.SearchNode(point2);

                    if (node1 == null && node2 != null)
                    {
                        Console.WriteLine("DETECTADO FIN ESCALERA");
                        _finnishStaircase = true;

                        return;
                    }
                    else
                        _finnishStaircase = false;
                }
            }


        }

        #endregion

        #region Draw Method

        public override void Draw(GameTime time)
        {
            base.Draw(time);
        }

        #endregion
    }
}
