using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Events;
using MetalSpace.Scene;
using MetalSpace.Models;
using MetalSpace.Managers;
using MetalSpace.Settings;
using MetalSpace.GameScreens;
using MetalSpace.Particle_Systems;
using MetalSpace.ArtificialIntelligence;

namespace MetalSpace.Objects
{
    /// <summary>
    /// The <c>Enemy</c> class represents one of the existing enemy of the game.
    /// </summary>
    class Enemy : Character
    {
        #region Fields

        /// <summary>
        /// State of the enemy in the game.
        /// </summary>
        public enum State
        {
            /// <summary>
            /// Waiting (the player is not visible).
            /// </summary>
            Waiting,
            /// <summary>
            /// Running (the enemy has detected the player and it is following him).
            /// </summary>
            Running,
            /// <summary>
            /// Attacking (the enemy is so close to the player that it can attack him).
            /// </summary>
            Attacking,
            /// <summary>
            /// Attacked (the player has shooted to the enemy and the shot has collided with it).
            /// </summary>
            Attacked,
            /// <summary>
            /// Dead (the player has ended with the enemy).
            /// </summary>
            Dead
        }

        /// <summary>
        /// Store for the Context property.
        /// </summary>
        private EnemyContext _context;

        /// <summary>
        /// Store for the LastEnemyState property.
        /// </summary>
        private State _lastEnemyState;

        /// <summary>
        /// Store for the EnemyState property.
        /// </summary>
        private State _enemyState;

        /// <summary>
        /// Amount of time to control some states (unconscious, dying...).
        /// </summary>
        private float _amount;

        /// <summary>
        /// true if the enemy is dying, false otherwise.
        /// </summary>
        private bool _dying;

        /// <summary>
        /// true if the enemy is unconscious, false otherwise.
        /// </summary>
        private bool _unconscious;

        /// <summary>
        /// true if the enemy is attacking, false otherwise.
        /// </summary>
        private bool _attacking;

        /// <summary>
        /// Store for the AttackMade property
        /// </summary>
        private bool _attackMade;

        /// <summary>
        /// true if the enemy is die and the points are free.
        /// </summary>
        private bool _endPoints;

        /// <summary>
        /// Particle system that control the liberation fo the points when the enemy is dead.
        /// </summary>
        private PointParticle _points;

        /// <summary>
        /// List of object that the enemy throw away when is dead.
        /// </summary>
        private List<MoveableObject> _objects;

        #endregion

        #region Properties

        /// <summary>
        /// Context property
        /// </summary>
        /// <value>
        /// Context of the artificial intelligence.
        /// </value>
        public EnemyContext Context
        {
            get { return _context; }
            set { _context = value; }
        }

        /// <summary>
        /// LastEnemyState property
        /// </summary>
        /// <value>
        /// Last state that the enemy had.
        /// </value>
        public State LastEnemyState
        {
            get { return _lastEnemyState; }
            set { _lastEnemyState = value; }
        }

        /// <summary>
        /// EnemyState property
        /// </summary>
        /// <value>
        /// Current state of the enemy.
        /// </value>
        public State EnemyState
        {
            get { return _enemyState; }
            set { _enemyState = value; }
        }

        /// <summary>
        /// AttackMade property
        /// </summary>
        /// <value>
        /// true if the enemy has made an attack, false otherwise.
        /// </value>
        public bool AttackMade
        {
            get { return _attackMade; }
            set { _attackMade = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>Enemy</c> class.
        /// </summary>
        /// <param name="scene">Reference to the scene where the enemy moves.</param>
        /// <param name="modelName">File that contains the model that represents the enemy.</param>
        /// <param name="position">Initial position of the enemy.</param>
        /// <param name="rotation">Initial rotation of the enemy.</param>
        /// <param name="scale">Initial scale of the enemy.</param>
        /// <param name="maxSpeed">Max speed of the enemy.</param>
        /// <param name="maxLife">Max life of the enemy.</param>
        /// <param name="attack">Attack of the enemy.</param>
        public Enemy(SceneRenderer scene, string modelName, Vector3 position, Vector3 rotation,
            Vector3 scale, Vector2 maxSpeed, int maxLife, int attack)
            : base(scene, new AnimatedModel((GameModel)ModelManager.GetModel(modelName),
                position, rotation, scale, 0), maxSpeed, maxLife, maxLife, attack)
        {

            _lastEnemyState = State.Waiting;
            _enemyState = State.Waiting;

            _lastPlayerXDirection = XDirection.Right;
            _playerXDirection = XDirection.Right;

            ((AnimatedModel)this.DModel).TimeSpeed = 0.25f;
            ((AnimatedModel)this.DModel).Animation.StartClip("Waiting", true);

            _amount = 0.0f;
            _dying = false;
            _unconscious = false;
            _attacking = false;
            _attackMade = false;
            _endPoints = false;

            _points = new PointParticle();
            _points.AutoInitialize(EngineManager.GameGraphicsDevice, 
                EngineManager.ContentManager, null);//ScreenManager.SpriteBatch);
            _points.PointsReceived += new EventHandler(PointsHandler);
        }

        public Enemy(SceneRenderer scene, string modelName, Vector3 position, Vector3 rotation,
            Vector3 scale, Vector2 maxSpeed, int maxLife, int attack, List<string> objects)
            : this(scene, modelName, position, rotation, scale, maxSpeed, maxLife, attack)
        {
            _objects = new List<MoveableObject>();
            foreach (string individualObject in objects)
            {
                _objects.Add(new MoveableObject(_scene, new DrawableModel(
                    (GameModel)ModelManager.GetModel(individualObject + "Object"),
                    Position, Rotation, Scale, 0), Speed, true, true));
                _objects[0].Speed = Vector2.Zero;
            }
        }

        #endregion

        #region PointsHandler Method

        /// <summary>
        /// Create an event handler to control the points.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointsHandler(object sender, EventArgs e)
        {
            // Now that the Splash Screen is done displaying, clean it up.
            _points.PointsReceived -= new EventHandler(PointsHandler);
            EngineManager.ParticleManager.ParticleSystem.RemoveParticleSystem(_points);
            _points.Destroy();
            _points = null;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load all needed content of the <c>Enemy</c>.
        /// </summary>
        public override void Load()
        {
            base.Load();
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload all needed content of the <c>Enemy</c>.
        /// </summary>
        public override void Unload()
        {
            base.Unload();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the state of the <c>Enemey</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void Update(GameTime gameTime)
        {
            float time = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

            if (_points != null)
            {
                _points.EmitterPosition = Position;
                _points.DestionationPosition = _context.Player.Position + 
                    new Vector3(0, _model.BSphere.Radius, 0.1f);
            }

            if (_context.CurrentState == Unconscious.Instance)
            {
                if (!_unconscious)
                {
                    _amount = 0.0f;
                    _unconscious = true;
                }
                else
                {
                    _amount += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                    if (_amount >= 7)
                    {
                        _amount = 0.0f;
                        _unconscious = false;
                        
                        _life = _maxLife;
                        _context.ChangeState(WakeUp.Instance);
                    }
                }
            }
            else if (_context.CurrentState == Attacking.Instance)
            {
                if (!_attacking)
                {
                    _amount = 0.0f;
                    _attacking = true;
                    if (_amount == 0.0f)
                        _attackMade = true;
                    else
                        _attackMade = false;
                }
                else
                {
                    _amount += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                    if (_amount >= 2)
                    {
                        //_amount = 0.0f;
                        _attacking = false;
                        _attackMade = true;
                    }
                }
            }
            else if (_context.CurrentState == Dying.Instance)
            {
                if (!_dying)
                {
                    _amount = 0.0f;
                    _dying = true;

                    _checkGravity = false;
                    _checkCollision = false;
                    
                    if (_points != null && !_endPoints)
                    {
                        _endPoints = true;
                        SoundManager.GetSound("PointsReceived").Volume = GameSettings.DefaultInstance.SoundVolume;
                        SoundManager.GetSound("PointsReceived").Play(true);
                        EngineManager.ParticleManager.ParticleSystem.AddParticleSystem(_points);
                    }
                }
                else
                {
                    _correctPosition += new Vector3(0, -gameTime.ElapsedGameTime.Milliseconds / 1000f, 0f);
                    _amount += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                    if (_amount >= 1)
                    {
                        _amount = 0.0f;
                        _dying = false;
                        _visible = false;
                    }
                }
            }

            if (_context.CurrentState != Attacked.Instance &&
                _context.CurrentState != WakeUp.Instance &&
                _context.CurrentState != Unconscious.Instance &&
                _context.CurrentState != Dying.Instance)
            {
                BoundingSphere fieldOfView = new BoundingSphere(_model.BSphere.Center, 4f);
                if (fieldOfView.Intersects(_context.Player.DModel.BSphere))
                {
                    fieldOfView.Radius = 1f;
                    if (fieldOfView.Intersects(_context.Player.DModel.BSphere))
                    {
                        _context.ChangeState(Attacking.Instance);
                    }
                    else
                    {
                        _context.ChangeState(Following.Instance);
                        _amount = 0.0f;
                        _attacking = false;
                        _attackMade = false;
                    }
                }
                else
                {
                    _context.ChangeState(Inactive.Instance);
                    _amount = 0.0f;
                    _attacking = false;
                    _attackMade = false;
                }
            }

            _context.Update(gameTime);
            if (!_endPoints && _objects != null)
            {
                foreach (MoveableObject mObject in _objects)
                    mObject.Position = Position;
            }
            else if(_objects != null)
            {
                foreach (MoveableObject mObject in _objects)
                    ((MainGameScreen)ScreenManager.GetScreen("ContinueGame")).ThrownObjectsManager.AddObject(mObject);

                //    mObject.Rotation += new Vector3((float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f), 0, 0);
                _objects = null;
            }

            base.Update(gameTime);
        }

        #endregion

        #region PreCollision Method

        /// <summary>
        /// Change the result of the collision test before apply the changes.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        protected override void PreCollision(GameTime gameTime)
        {
            base.PreCollision(gameTime);
        }

        #endregion

        #region PostCollision Method

        /// <summary>
        /// Permits to detect elements in the scene to be considered in the next loop.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        protected override void PostCollision(GameTime gameTime)
        {
            base.PostCollision(gameTime);
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>Enemy</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            EngineManager.ParticleManager.ParticleSystem.DrawAllParticleSystems();

            if (_endPoints && _objects != null)
            {
                foreach (MoveableObject mObject in _objects)
                    mObject.Draw(gameTime);
            }
        }

        #endregion
    }
}
