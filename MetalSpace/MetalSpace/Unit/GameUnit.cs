using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Models;
using MetalSpace.Managers;
using MetalSpace.Scene;
using MetalSpace.Events;

namespace MetalSpace.Unit
{
    class GameUnit
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

        protected Action _lastPlayerAction;
        protected Action _playerAction;

        protected State _lastPlayerState;
        protected XDirection _lastPlayerXDirection;
        protected YDirection _lastPlayerYDirection;

        protected State _playerState;
        protected int _directionIndicator;
        protected XDirection _playerXDirection;
        protected YDirection _playerYDirection;

        private int _life;
        private int _maxLife;

        protected bool _jump;
        protected bool _endJump;

        public Vector2 Speed;
        public Vector2 MaxSpeed;

        public BoundingSphere _boundingSphere;
        //private DrawableModel _model;
        private AnimatedModel _model;

        #endregion

        #region Properties

        public int Life
        {
            get { return _life; }
            set { _life = value; }
        }

        public int MaxLife
        {
            get { return _maxLife; }
            set { _maxLife = value; }
        }

        public AnimatedModel Model
        {
            get { return _model; }
            set { _model = value; }
        }
        /*public DrawableModel Model
        {
            get { return _model; }
            set { _model = value; }
        }*/

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

        public int DirectionIndicator
        {
            get { return _directionIndicator; }
            set { _directionIndicator = value; }
        }

        public BoundingSphere BoundingSphere
        {
            get { return _boundingSphere; }
            set { _boundingSphere = value; }
        }

        #endregion

        #region Constructor

        public GameUnit()
        {
            _life = 100;
            _maxLife = 100;
        }

        public GameUnit(string modelName, Vector3 position, Vector3 rotation, Vector3 scale,
            int life, int maxLife, Vector2 speed)
        {
            _life = life;
            _maxLife = maxLife;

            Speed = speed;

            //_model = new AnimatedModel(new DrawableModel((GameModel)ModelManager.GetModel(modelName), position, rotation, scale, 0));
            _model = new AnimatedModel((GameModel)ModelManager.GetModel(modelName), position, rotation, scale, 0);
            
            _boundingSphere = _model.BSphere;
        }

        #endregion

        #region Load Method

        public void Load()
        {

        }

        #endregion

        #region Unload Method

        public void Unload()
        {
            Speed = Vector2.Zero;
            MaxSpeed = Vector2.Zero;

            _model = null;
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            float time = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

            if (PlayerState == State.Waiting)
            {
                Speed.X = 0;
                Speed.Y += -9.81f * time;
            }

            if (PlayerState == State.Running)
            {
                if (Speed.X != MaxSpeed.X * _directionIndicator)
                    Speed.X = MaxSpeed.X * _directionIndicator;

                Speed.Y += -9.81f * time;
            }

            if (PlayerState == State.Jumping)
            {
                if (PlayerXDirection != XDirection.None)
                    if (Speed.X != MaxSpeed.X * _directionIndicator)
                        Speed.X = MaxSpeed.X * _directionIndicator;

                if (_jump == true && _endJump == false)
                {
                    _jump = false;
                    Speed.Y = 7f;
                }
                else
                    Speed.Y += -9.81f * time;
            }

            if (PlayerState == State.Climbing)
            {
                Speed.X = 0;

                if (PlayerYDirection == YDirection.Up)
                    Speed.Y = MaxSpeed.Y;
                else if (PlayerYDirection == YDirection.Down)
                    Speed.Y = -MaxSpeed.Y;
                else
                    Speed.Y = 0f;
            }

            Speed = new Vector2((float)Math.Round(Speed.X, 3), (float)Math.Round(Speed.Y, 3));
            if (PlayerState != State.Climbing)
            {
                Model.Position += new Vector3(
                    _directionIndicator == 0 ? 0f : Speed.X * time,
                    Speed.Y * time - (0.5f * 9.8f * (float)Math.Pow(time, 2)),
                    0);
            }
            else
            {
                Model.Position += new Vector3(0, Speed.Y * time, 0);
            }
        }

        #endregion

        #region Draw

        public void Draw(GameTime gametime)
        {

        }

        #endregion
    }
}
