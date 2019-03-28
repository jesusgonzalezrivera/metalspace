using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Scene;
using MetalSpace.Models;
using MetalSpace.Events;
using MetalSpace.Settings;
using MetalSpace.Managers;
using MetalSpace.GameScreens;

namespace MetalSpace.Objects
{
    /// <summary>
    /// The <c>Player</c> class represents the main player that the user controls.
    /// </summary>
    public class Player : Character
    {
        #region Fields

        /// <summary>
        /// State of the player when moves.
        /// </summary>
        public enum State
        {
            /// <summary>
            /// Waiting (the player does not moves).
            /// </summary>
            Waiting,
            /// <summary>
            /// Running (the player moves in left or right direction).
            /// </summary>
            Running,
            /// <summary>
            /// Jumping (the player do not collision with the ground).
            /// </summary>
            Jumping,
            /// <summary>
            /// Ducking (the player is ducking in the ground).
            /// </summary>
            Ducking,
            /// <summary>
            /// Climbing (the player is climbing a ladder).
            /// </summary>
            Climbing
        }

        /// <summary>
        /// Store for the LastPlayerState property.
        /// </summary>
        private State _lastPlayerState;

        /// <summary>
        /// Store for the PlayerState property.
        /// </summary>
        private State _playerState;

        /// <summary>
        /// Store for the Jump property.
        /// </summary>
        private bool _jump;

        /// <summary>
        /// Store for the EndJump property.
        /// </summary>
        private bool _endJump;

        /// <summary>
        /// true if the player is starting a jump, false otherwise.
        /// </summary>
        private bool _startJump;

        /// <summary>
        /// true if the player is finishing a jump, false otherwise.
        /// </summary>
        private bool _finishJump;

        /// <summary>
        /// true if a door is detected, false otherwise.
        /// </summary>
        private bool _doorDetected;

        /// <summary>
        /// true if the player has entered in a door, false otherwise.
        /// </summary>
        private bool _doorActivated;

        /// <summary>
        /// Bounding box that wrap the door.
        /// </summary>
        private OcTreeNode _door;
        private BoundingBox _doorBox;

        /// <summary>
        /// true if the player has started to climb a ladder, false otherwise.
        /// </summary>
        private bool _startLadder;

        /// <summary>
        /// true if the player is climbing a ladder, false otherwise.
        /// </summary>
        private bool _climbLadder;

        /// <summary>
        /// true if the player is finishing to climb a ladder, false otherwise.
        /// </summary>
        private bool _finishLadder;

        private bool _stopLadder;

        /// <summary>
        /// Store for the Debolio property.
        /// </summary>
        private int _debolio;

        /// <summary>
        /// Store for the Aerogel property.
        /// </summary>
        private int _aerogel;

        /// <summary>
        /// Store for the Fulereno property.
        /// </summary>
        private int _fulereno;

        /// <summary>
        /// Store for the TotalPoints property.
        /// </summary>
        private int _totalPoints;
        
        /// <summary>
        /// Store for the ShotSpeed property.
        /// </summary>
        private float _shotSpeed = 0.135f;

        /// <summary>
        /// Store for the PlayerGun property.
        /// </summary>
        protected Gun _gun;

        /// <summary>
        /// Store for the Objects property.
        /// </summary>
        protected List<Objects.Object> _objects;

        /// <summary>
        /// Store for the LifeBar property.
        /// </summary>
        private HealthBar _lifeBar;

        #endregion

        #region Properties

        /// <summary>
        /// LastPlayerState property
        /// </summary>
        /// <value>
        /// Last state of the player.
        /// </value>
        public State LastPlayerState
        {
            get { return _lastPlayerState; }
            set { _lastPlayerState = value; }
        }

        /// <summary>
        /// PlayerState property
        /// </summary>
        /// <value>
        /// Current state of the player.
        /// </value>
        public State PlayerState
        {
            get { return _playerState; }
            set { _playerState = value; }
        }

        /// <summary>
        /// Jump property
        /// </summary>
        /// <value>
        /// true if the player is jumping, false otherwise.
        /// </value>
        public bool Jump
        {
            get { return _jump; }
            set { _jump = value; }
        }

        /// <summary>
        /// EndJump property
        /// </summary>
        /// <value>
        /// true if the player has finished a jump, false otherwise.
        /// </value>
        public bool EndJump
        {
            get { return _endJump; }
            set { _endJump = value; }
        }

        /// <summary>
        /// ShotSpeed property
        /// </summary>
        /// <value>
        /// Speed of the shooting process.
        /// </value>
        public float ShotSpeed
        {
            get { return _shotSpeed; }
            set { _shotSpeed = value; }
        }

        /// <summary>
        /// PlayerGun property
        /// </summary>
        /// <value>
        /// Gun used by the player.
        /// </value>
        public Gun PlayerGun
        {
            get { return _gun; }
            set { _gun = value; }
        }

        /// <summary>
        /// Objects property
        /// </summary>
        /// <value>
        /// List of objects (equipped and not equipped) of the player.
        /// </value>
        public List<Object> Objects
        {
            get { return _objects; }
            set { _objects = value; }
        }

        /// <summary>
        /// Debolio property
        /// </summary>
        /// <value>
        /// Amount of debolio that the player has.
        /// </value>
        public int Debolio
        {
            get { return _debolio; }
            set { _debolio = value; }
        }

        /// <summary>
        /// Aerogel property
        /// </summary>
        /// <value>
        /// Amount of aerogel that the player has.
        /// </value>
        public int Aerogel
        {
            get { return _aerogel; }
            set { _aerogel = value; }
        }

        /// <summary>
        /// Fulereno property
        /// </summary>
        /// <value>
        /// Amount of fulereno that the player has.
        /// </value>
        public int Fulereno
        {
            get { return _fulereno; }
            set { _fulereno = value; }
        }

        /// <summary>
        /// TotalPoints property
        /// </summary>
        /// <value>
        /// Total number of points that the player has.
        /// </value>
        public int TotalPoints
        {
            get { return _totalPoints; }
            set { _totalPoints = value; }
        }

        /// <summary>
        /// LifeBar property
        /// </summary>
        /// <value>
        /// Life bar that show the amount of life of the player.
        /// </value>
        public HealthBar LifeBar
        {
            get { return _lifeBar; }
            set { _lifeBar = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>Player</c> class.
        /// </summary>
        /// <param name="scene">Reference to the scene where the player moves.</param>
        /// <param name="modelName">File that contains the model that represents the player.</param>
        /// <param name="position">Initial position of the player.</param>
        /// <param name="rotation">Initial rotation of the player.</param>
        /// <param name="scale">Initial scale of the player.</param>
        /// <param name="maxSpeed">Max speed of the player.</param>
        /// <param name="maxLife">Max life of the player.</param>
        /// <param name="attack">Attack of the player.</param>
        public Player(SceneRenderer scene, string modelName, Vector3 position, Vector3 rotation,
            Vector3 scale, Vector2 maxSpeed, int maxLife, int life, int attack)
            : base(scene, new AnimatedModel((GameModel)ModelManager.GetModel(modelName),
                position, rotation, scale, 0, true), maxSpeed, maxLife, life, attack)
        {
            _jump = false;
            _endJump = true;

            _startJump = false;
            _finishJump = false;

            _doorDetected = false;
            _doorActivated = false;

            _startLadder = false;
            _climbLadder = false;
            _finishLadder = false;
            _stopLadder = false;

            _lastPlayerState = State.Waiting;
            _playerState = State.Waiting;

            _debolio = 0;
            _aerogel = 0;
            _fulereno = 0;
            _totalPoints = 0;

            _objects = new List<Objects.Object>();

            _lifeBar = new HealthBar(_maxLife, _life);

            ((AnimatedModel)this.DModel).Animation.StartClip("Waiting", true);
            Console.WriteLine("ESTOY ENTRANDO TAMBIEN AQUI2: " + _life + " - " + _maxLife);
        }

        /// <summary>
        /// Constructor of the <c>Player</c> class.
        /// </summary>
        /// <param name="scene">Reference to the scene where the player moves.</param>
        /// <param name="modelName">File that contains the model that represents the player.</param>
        /// <param name="position">Initial position of the player.</param>
        /// <param name="rotation">Initial rotation of the player.</param>
        /// <param name="scale">Initial scale of the player.</param>
        /// <param name="maxSpeed">Max speed of the player.</param>
        /// <param name="maxLife">Max life of the player.</param>
        /// <param name="currentLife">Current life of the player.</param>
        /// <param name="attack">Attack of the player.</param>
        /// <param name="debolio">Amount of debolio of the player.</param>
        /// <param name="aerogel">Amount of aerogel of the player.</param>
        /// <param name="fulereno">Amount of fulereno of the player.</param>
        /// <param name="totalPoints">Total number of points of the player.</param>
        /// <param name="objects">List of objects (equipped and not equipped) that the player has.</param>
        public Player(SceneRenderer scene, string modelName, Vector3 position, Vector3 rotation,
            Vector3 scale, Vector2 maxSpeed, int maxLife, int currentLife, int attack, int debolio,
            int aerogel, int fulereno, int totalPoints, List<Objects.Object> objects) : 
            this(scene, modelName, position, rotation, scale, maxSpeed, maxLife, currentLife, attack)
        {
            Console.WriteLine("ESTOY ENTRANDO TAMBIEN AQUI: " + currentLife);

            _debolio = debolio;
            _aerogel = aerogel;
            _fulereno = fulereno;
            _totalPoints = totalPoints;

            _objects = objects;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load all the needed elements of the <c>Player</c>.
        /// </summary>
        public override void Load()
        {
            base.Load();

            _lifeBar.Load();
            /*_objects.Add(new Weapon("GunNormal", "GunNormalObjectTexture", 0, false, Gun.ShotType.Laser, 10, 25));
            _objects.Add(new Armature("Helmet", "HelmetObjectTexture", 1, false, 10, 10, Armature.ArmatureType.Helmet));
            _objects.Add(new Armature("Boots1", "BootsObjectTexture", 2, false, 10, 10, Armature.ArmatureType.Foot));
            _objects.Add(new Armature("Armature1", "ArmorObjectTexture", 3, false, 10, 10, Armature.ArmatureType.Body));
            _objects.Add(new Ammo("LaserShot", "LaserShotObjectTexture", 4, false, 50, Gun.ShotType.Laser));
            _objects.Add(new Ammo("NormalShot", "NormalShotObjectTexture", 5, false, 100, Gun.ShotType.Normal));
            _objects.Add(new Objects.Object("CardKey", "CardKey", 6, false));*/
        }

        #endregion

        #region AddObject Method

        /// <summary>
        /// Add a new object to the list of objects.
        /// </summary>
        /// <param name="newObject">Object to be added to the list.</param>
        /// <returns>true if the object was added, false otherwise.</returns>
        public bool AddObject(Objects.Object newObject)
        {
            if (_objects.Count == 9)
                return false;
            else
            {
                if (newObject is Ammo)
                {
                    foreach (Objects.Object playerObject in _objects)
                        if (playerObject is Ammo && ((Ammo)playerObject).Type == ((Ammo)newObject).Type)
                            ((Ammo)playerObject).Amount += ((Ammo)newObject).Amount;
                }
                else
                {
                    _objects.Add(newObject);

                    OrderObjects();
                }

                return true;
            }
        }

        #endregion

        #region OrderObjects Method

        /// <summary>
        /// Order the not equipped objects position.
        /// </summary>
        private void OrderObjects()
        {
            // Order position of elements
            bool first = true;
            int _notEquippedIndex = 0;
            for (int i = 0; i < _objects.Count; i++)
            {
                if (!_objects[i].IsEquipped && first)
                {
                    first = false;
                    _objects[i].Position = _notEquippedIndex;
                    _notEquippedIndex++;
                }
                else if (!_objects[i].IsEquipped)
                {
                    _objects[i].Position = _notEquippedIndex;
                    _notEquippedIndex++;
                }
            }
        }

        #endregion

        #region ChangeState Method

        /// <summary>
        /// Change the state (equipped, not equipped) of an object.
        /// </summary>
        /// <param name="newObject">Object to be changed.</param>
        /// <param name="newState">New state of the object.</param>
        /// <returns></returns>
        public bool ChangeState(Objects.Object newObject, bool newState)
        {
            if (newState)
            {
                if (!_objects.Contains(newObject))
                    return false;

                // Search for a equiped object
                foreach (Objects.Object playerObject in _objects)
                    if ((playerObject is Objects.Armature && playerObject.IsEquipped && newObject is Objects.Armature &&
                         ((Objects.Armature)playerObject).Type == ((Objects.Armature)newObject).Type) ||
                        (playerObject is Objects.Weapon && newObject is Objects.Weapon && playerObject.IsEquipped))
                    {
                        playerObject.IsEquipped = false;
                        ((InventoryScreen)ScreenManager.GetScreen("Inventory")).Inventory.AddObject(playerObject);

                        ((AnimatedModel)DModel).MeshesToDraw.Remove(playerObject.Name);
                    }

                // Change the state of the new Object
                foreach (Objects.Object playerObject in _objects)
                    if (playerObject == newObject)
                    {
                        playerObject.IsEquipped = true;
                        if(playerObject.Name.Contains("Gun"))
                            _gun = new Gun(playerObject.Name, ((Weapon) playerObject).Type, this,
                                ((Weapon) playerObject).Power, ((Weapon) playerObject).CurrentAmmo);
                        else
                            ((AnimatedModel)DModel).MeshesToDraw.Add(playerObject.Name);
                    }

                OrderObjects();

                return true;
            }
            else
            {
                if (!_objects.Contains(newObject))
                    return false;

                foreach (Objects.Object playerObject in _objects)
                    if (playerObject == newObject)
                    {
                        if(playerObject.IsEquipped)
                            ((AnimatedModel)DModel).MeshesToDraw.Remove(playerObject.Name);
                        playerObject.IsEquipped = false;
                        if (playerObject.Name.Contains("Gun"))
                            _gun = null;
                    }

                OrderObjects();

                return true;
            }
        }

        #endregion

        #region RemoveObject Method

        /// <summary>
        /// Remove an object from the list of objects.
        /// </summary>
        /// <param name="index">Index of the object.</param>
        /// <returns>true if the object was removed, false otherwise.</returns>
        public bool RemoveObject(int index)
        {
            if (index >= _objects.Count)
                return false;

            List<Objects.Object> newList = new List<Object>();
            for (int i = 0; i < _objects.Count; i++)
                if (i != index)
                    newList.Add(_objects[i]);
                else if(_objects[i].IsEquipped == true)
                    ((AnimatedModel)DModel).MeshesToDraw.Remove(_objects[i].Name);

            _objects = newList;

            OrderObjects();

            return true;
        }

        #endregion

        #region RemoveObject Method

        /// <summary>
        /// Remove an object from the list of objects.
        /// </summary>
        /// <param name="removableObject">Object to be removed.</param>
        /// <returns>true if the object was removed, false otherwise.</returns>
        public bool RemoveObject(Objects.Object removableObject)
        {
            if (!_objects.Contains(removableObject))
                return false;

            List<Objects.Object> newList = new List<Object>();
            for (int i = 0; i < _objects.Count; i++)
                if(_objects[i] != removableObject)
                    newList.Add(_objects[i]);
                else if (_objects[i].IsEquipped == true)
                    ((AnimatedModel)DModel).MeshesToDraw.Remove(_objects[i].Name);

            _objects = newList;

            // Order position of elements
            bool first = true;
            int _notEquippedIndex = 0;
            for (int i = 0; i < _objects.Count; i++)
            {
                if (!_objects[i].IsEquipped && first)
                {
                    first = false;
                    _objects[i].Position = _notEquippedIndex;
                    _notEquippedIndex++;
                }
                else if (!_objects[i].IsEquipped)
                {
                    _objects[i].Position = _notEquippedIndex;
                    _notEquippedIndex++;
                }
            }

            return true;
        }

        #endregion

        #region CheckObject Method

        public bool CheckObject(string name)
        {
            if (name == null)
                return true ;

            foreach (Objects.Object inventoryObject in _objects)
                if (inventoryObject.Name.Contains(name))
                    return true;

            return false;
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload all the needed elements of the <c>Player</c>.
        /// </summary>
        public override void Unload()
        {
            base.Unload();

            _lifeBar.Unload();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the <c>Player</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void Update(GameTime gameTime)
        {
            OcTreeNode node;
            Vector3 checkPosition;
            float time = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

            if (PlayerState == State.Waiting)
                _speed.X = 0;

            if (PlayerState == State.Running)
            {
                SoundManager.GetSound("PlayerClimbingLadder").Pause();
                SoundManager.GetSound("Running").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("Running").Play(false, true);

                if (_speed.X != MaxSpeed.X * _directionIndicator)
                    _speed.X = MaxSpeed.X * _directionIndicator;
            }
            else if (PlayerState == State.Climbing)
            {
                SoundManager.GetSound("Running").Pause();
                SoundManager.GetSound("PlayerClimbingLadder").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("PlayerClimbingLadder").Play(false, true);
                if (PlayerYDirection == YDirection.None)
                    SoundManager.GetSound("PlayerClimbingLadder").Pause();
            }
            else
            {
                SoundManager.GetSound("Running").Pause();
                if (PlayerYDirection != YDirection.None)
                    SoundManager.GetSound("PlayerClimbingLadder").Pause();
            }

            if (PlayerState == State.Jumping)
            {
                if (PlayerXDirection != XDirection.None)
                {
                    if (Speed.X != MaxSpeed.X * _directionIndicator)
                        _speed.X = MaxSpeed.X * _directionIndicator;
                }
                else
                    _speed.X = 0f;

                if (_jump == true && _endJump == false)
                {
                    _jump = false;
                    _speed.Y = 7f;
                    _checkCollision = false;
                }
                else
                {
                    _model.BSphere = ((DrawableModel)_model).GetBoundingSphere();

                    checkPosition = _model.BSphere.Center;
                    checkPosition.Y += -_model.BSphere.Radius - 0.1f;
                    node = _scene.MainLayer.SearchNode(checkPosition);
                    if (node == null)
                        _checkCollision = true;

                    checkPosition = _model.BSphere.Center;
                    if (_startStaircase || _climbStaircase || _finishStaircase)
                    {
                        checkPosition.Y += -_model.BSphere.Radius * 2f;
                    }
                    else
                        checkPosition.Y += -_model.BSphere.Radius * 3;

                    node = _scene.MainLayer.SearchNode(checkPosition);
                    if (node != null)// && node.ModelList[0].Key != NodeType.Ladder)
                    {
                        if (_startJump == false)
                        {
                            ((AnimatedModel)this.DModel).TimeSpeed = 0.7f;
                            ((AnimatedModel)this.DModel).Animation.StartClip("AirJump", true);

                            _startJump = true;
                        }
                    }
                }

                if (_speed.Y <= 0)
                {
                    checkPosition = _model.BSphere.Center;
                    checkPosition.Y += -_model.BSphere.Radius * 2;
                    node = _scene.MainLayer.SearchNode(checkPosition);
                    if (node != null && node.ModelList[0].Key != NodeType.Ladder)
                    {
                        if (_finishJump == false)
                        {
                            ((AnimatedModel)this.DModel).TimeSpeed = 1.0f;
                            ((AnimatedModel)this.DModel).Animation.StartClip("EndJump", true);

                            _finishJump = true;
                        }
                    }

                    if (_downCollision == true)
                    {
                        _jump = false;
                        _endJump = true;
                        _startJump = false;
                        _finishJump = false;
                        _speed.Y = 0;

                        if (_playerXDirection == XDirection.None)
                        {
                            EventManager.Trigger(new EventData_UnitStateChanged(this, State.Waiting));

                            if (_playerYDirection == Player.YDirection.Up)
                            {
                                ((AnimatedModel)this.DModel).TimeSpeed = 3.0f;
                                ((AnimatedModel)this.DModel).Animation.StartClip("ShootUp", true);
                            }
                            else
                            {
                                ((AnimatedModel)this.DModel).TimeSpeed = 1.0f;
                                ((AnimatedModel)this.DModel).Animation.StartClip("Waiting", true);
                            }
                        }
                        else
                        {
                            EventManager.Trigger(new EventData_UnitStateChanged(this, State.Running));

                            ((AnimatedModel)this.DModel).TimeSpeed = 3.0f;
                            if (_playerYDirection == Player.YDirection.Up)
                                ((AnimatedModel)this.DModel).Animation.StartClip("RunningD", true);
                            else
                                ((AnimatedModel)this.DModel).Animation.StartClip("RunningH", true);
                        }
                    }
                }
            }

            if(_gun != null)
                _gun.Update(gameTime);

            _lifeBar.Update(gameTime);
            
            base.Update(gameTime);

            //Console.WriteLine("PLAYER UPDATE: " + Position);
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

            OcTreeNode node;
            Vector3 checkPosition;
            float time = (float)(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
            
            if (_startLadder)
            {
                if (_playerState != State.Climbing)
                    EventManager.Trigger(new EventData_UnitStateChanged(this, Player.State.Climbing));
                
                _speed.X = 0;
                switch (_playerYDirection)
                {
                    case YDirection.None:
                        _speed.Y = 0;
                        break;
                    case YDirection.Down:
                        _speed.Y = -MaxSpeed.Y;
                        break;
                    case YDirection.Up:
                        _speed.Y = MaxSpeed.Y;
                        break;
                }

                _correctPosition = Position +
                    new Vector3(0, Speed.Y * time, 0);
                
                checkPosition = _model.BSphere.Center;
                checkPosition.Y += -_model.BSphere.Radius + 0.2f;
                node = _scene.MainLayer.SearchNode(checkPosition);
                if (node != null && node.ModelList[0].Key == NodeType.None)
                {
                    EventManager.Trigger(new EventData_ObjectsCollision(this, ref node,
                        EventData_ObjectsCollision.CollisionDirection.Down,
                        EventData_ObjectsCollision.CollisionSurface.Box));
                }
            }
            else if (_finishLadder)
            {
                if (_playerState != State.Climbing)
                    EventManager.Trigger(new EventData_UnitStateChanged(this, Player.State.Climbing));

                _speed.X = 0;
                switch (_playerYDirection)
                {
                    case YDirection.None:
                        _speed.Y = 0;
                        break;
                    case YDirection.Down:
                        _speed.Y = -MaxSpeed.Y;
                        break;
                    case YDirection.Up:
                        _speed.Y = MaxSpeed.Y;
                        break;
                }

                _correctPosition = Position + new Vector3(0, Speed.Y * time, 0);

                checkPosition = _model.BSphere.Center;
                checkPosition.X += _model.BSphere.Radius / 2f;
                node = _scene.MainLayer.SearchNode(checkPosition);

                Vector3 checkPosition2 = _model.BSphere.Center;
                checkPosition2.X += -_model.BSphere.Radius / 2f;
                OcTreeNode node2 = _scene.MainLayer.SearchNode(checkPosition2);

                if (node == null && node2 == null)
                {
                    _startLadder = false;
                    _climbLadder = false;
                    _finishLadder = false;

                    if (_playerXDirection == XDirection.None)
                    {
                        EventManager.Trigger(new EventData_UnitStateChanged(this, State.Waiting));

                        ((AnimatedModel)this.DModel).TimeSpeed = 1.0f;
                        ((AnimatedModel)this.DModel).Animation.StartClip("Waiting", true);
                    }
                    else
                    {
                        EventManager.Trigger(new EventData_UnitStateChanged(this, State.Running));

                        ((AnimatedModel)this.DModel).TimeSpeed = 3.0f;
                        if (_playerYDirection == Player.YDirection.Up)
                            ((AnimatedModel)this.DModel).Animation.StartClip("RunningD", true);
                        else
                            ((AnimatedModel)this.DModel).Animation.StartClip("RunningH", true);
                    }
                }
            }

            if (_climbLadder)
            {
                if (_playerState != State.Climbing)
                    EventManager.Trigger(new EventData_UnitStateChanged(this, Player.State.Climbing));

                _speed.X = 0;
                switch (_playerYDirection)
                {
                    case YDirection.None:
                        _speed.Y = 0;
                        break;
                    case YDirection.Down:
                        _speed.Y = -MaxSpeed.Y;
                        break;
                    case YDirection.Up:
                        _speed.Y = MaxSpeed.Y;
                        break;
                }

                _correctPosition = Position + new Vector3(0, (Speed.Y * time), 0);
            }

            if (_stopLadder)
            {
                if (this.PlayerState != State.Waiting && this.PlayerState != State.Running)
                {
                    if (_playerXDirection == XDirection.None)
                    {
                        EventManager.Trigger(new EventData_UnitStateChanged(this, State.Waiting));

                        ((AnimatedModel)this.DModel).TimeSpeed = 1.0f;
                        ((AnimatedModel)this.DModel).Animation.StartClip("Waiting", true);
                    }
                    else
                    {
                        EventManager.Trigger(new EventData_UnitStateChanged(this, State.Running));

                        ((AnimatedModel)this.DModel).TimeSpeed = 3.0f;
                        if (_playerYDirection == Player.YDirection.Up)
                            ((AnimatedModel)this.DModel).Animation.StartClip("RunningD", true);
                        else
                            ((AnimatedModel)this.DModel).Animation.StartClip("RunningH", true);
                    }
                }

                _stopLadder = false;
            }

            if (_doorDetected && !_doorActivated)
            {
                //foreach (KeyValuePair<string, KeyValuePair<Vector3, BoundingBox>> door in _scene.MainLayer.Doors)
                foreach(Door door in _scene.MainLayer.Doors)
                {
                    //if (door.Value.Value.Contains(_doorBox) != ContainmentType.Disjoint)
                    if (door.boundingBox.Contains(_doorBox) != ContainmentType.Disjoint)
                    {
                        if (CheckObject(door.neededObject))
                        {
                            _doorActivated = true;
                            EventManager.QueueEvent(new EventData_LevelChanged(door.nextLevel, this, door.nextPosition));
                        }
                        else
                        {
                            if (_playerXDirection == XDirection.Left ||
                                (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Left))
                            {
                                _speed.X = 0;
                                EventManager.Trigger(new EventData_ObjectsCollision(this, ref _door,
                                    EventData_ObjectsCollision.CollisionDirection.Left, EventData_ObjectsCollision.CollisionSurface.Box, 
                                    _model.BSphere.Radius / 6.0f));
                            }
                            else if (_playerXDirection == XDirection.Right ||
                                (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Right))
                            {
                                _speed.X = 0;
                                EventManager.Trigger(new EventData_ObjectsCollision(this, ref _door,
                                    EventData_ObjectsCollision.CollisionDirection.Right, EventData_ObjectsCollision.CollisionSurface.Box,
                                    -(_model.BSphere.Radius / 6.0f) - 0.1f));
                            }
                        }
                    }
                }
            }
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

            Vector3 point1, point2, point3;
            OcTreeNode node1, node2, node3;

            if (!_startLadder)
            {
                // Detect left start ladder
                if (_playerXDirection == XDirection.Left ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Left))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += _model.BSphere.Radius - 0.5f;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.Y += -_model.BSphere.Radius - 0.1f;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    point3 = _model.BSphere.Center;
                    point3.X += _model.BSphere.Radius + 0.5f;
                    node3 = _scene.MainLayer.SearchNode(point3);

                    if (node1 != null && node1.ModelList[0].Key == NodeType.Ladder &&
                        node2 != null && node2.ModelList[0].Key == NodeType.None &&
                        node3 == null)
                    {
                        _startLadder = true;
                        _climbLadder = false;
                        _finishLadder = false;

                        Vector3 rotation = _model.Rotation;
                        _model.Rotation = new Vector3(MathHelper.ToRadians(270), rotation.Y, rotation.Z);

                        return;
                    }
                    else
                    {
                        if (node1 != null && node1.ModelList[0].Key == NodeType.Ladder && node2 == null && node3 == null)
                        {
                            _startLadder = true;
                            _climbLadder = false;
                            _finishLadder = false;

                            Vector3 rotation = _model.Rotation;
                            _model.Rotation = new Vector3(MathHelper.ToRadians(270), rotation.Y, rotation.Z);

                            return;
                        }
                        else
                            _startLadder = false;
                    }
                }

                // Detect right start ladder
                if (_playerXDirection == XDirection.Right ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Right))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += -_model.BSphere.Radius + 0.5f;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.Y += -_model.BSphere.Radius - 0.1f;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    point3 = _model.BSphere.Center;
                    point3.X += -_model.BSphere.Radius - 0.5f;
                    node3 = _scene.MainLayer.SearchNode(point3);

                    if (node1 != null && node1.ModelList[0].Key == NodeType.Ladder &&
                        node2 != null && node2.ModelList[0].Key == NodeType.None &&
                        node3 == null)
                    {
                        _startLadder = true;
                        _climbLadder = false;
                        _finishLadder = false;

                        Vector3 rotation = _model.Rotation;
                        _model.Rotation = new Vector3(MathHelper.ToRadians(90), rotation.Y, rotation.Z);

                        return;
                    }
                    else
                    {
                        if (node1 != null && node1.ModelList[0].Key == NodeType.Ladder && node2 == null && node3 == null)
                        {
                            _startLadder = true;
                            _climbLadder = false;
                            _finishLadder = false;

                            Vector3 rotation = _model.Rotation;
                            _model.Rotation = new Vector3(MathHelper.ToRadians(90), rotation.Y, rotation.Z);

                            return;
                        }
                        else
                            _startLadder = false;
                    }
                }
            }

            if (!_climbLadder)
            {
                // Detect left climb ladder
                if (_playerXDirection == XDirection.Left ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Left))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += _model.BSphere.Radius - 0.5f;
                    point1.Y += -_model.BSphere.Radius;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += _model.BSphere.Radius - 0.5f;
                    point2.Y += _model.BSphere.Radius;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    point3 = _model.BSphere.Center;
                    point3.X += _model.BSphere.Radius + 0.5f;
                    node3 = _scene.MainLayer.SearchNode(point3);

                    if (node1 != null && node1.ModelList[0].Key == NodeType.Ladder &&
                        node2 != null && node2.ModelList[0].Key == NodeType.Ladder &&
                        node3 == null)
                    {
                        _startLadder = false;
                        _climbLadder = true;
                        _finishLadder = false;

                        return;
                    }
                    else
                        _climbLadder = false;
                }

                // Detect right climb ladder
                if (_playerXDirection == XDirection.Right ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Right))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += -_model.BSphere.Radius + 0.5f;
                    point1.Y += -_model.BSphere.Radius;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += -_model.BSphere.Radius + 0.5f;
                    point2.Y += _model.BSphere.Radius;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    point3 = _model.BSphere.Center;
                    point3.X += -_model.BSphere.Radius - 0.5f;
                    node3 = _scene.MainLayer.SearchNode(point3);

                    if (node1 != null && node1.ModelList[0].Key == NodeType.Ladder &&
                        node2 != null && node2.ModelList[0].Key == NodeType.Ladder &&
                        node3 == null)
                    {
                        _startLadder = false;
                        _climbLadder = true;
                        _finishLadder = false;

                        return;
                    }
                    else
                        _climbLadder = false;
                }
            }

            if (!_finishLadder && _climbLadder)
            {
                // Detect left finish ladder
                if (_playerXDirection == XDirection.Left ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Left))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += _model.BSphere.Radius - 0.5f;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.Y += _model.BSphere.Radius;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    point3 = _model.BSphere.Center;
                    point3.X += _model.BSphere.Radius + 1f;
                    node3 = _scene.MainLayer.SearchNode(point3);
                    
                    if (node1 != null && node1.ModelList[0].Key == NodeType.Ladder && node2 == null && node3 == null)
                    {
                        _startLadder = false;
                        _climbLadder = false;
                        _finishLadder = true;

                        return;
                    }
                    else
                        _finishLadder = false;
                }

                // Detect right finish ladder
                if (_playerXDirection == XDirection.Right ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Right))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += -_model.BSphere.Radius + 0.5f;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.Y += _model.BSphere.Radius;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    point3 = _model.BSphere.Center;
                    point3.X += -_model.BSphere.Radius - 1f;
                    node3 = _scene.MainLayer.SearchNode(point3);
                    
                    if (node1 != null && node1.ModelList[0].Key == NodeType.Ladder && node2 == null && node3 == null)
                    {
                        _startLadder = false;
                        _climbLadder = false;
                        _finishLadder = true;

                        return;
                    }
                    else
                        _finishLadder = false;
                }
            }

            if (_startLadder || _climbLadder || _finishLadder)
            {
                // Detect left stop ladder
                if (_playerXDirection == XDirection.Left ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Left))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += _model.BSphere.Radius - 0.5f;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += -_model.BSphere.Radius - 0.5f;
                    point2.Y += _model.BSphere.Radius / 2f;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    if (node1 == null && node2 == null)
                    {
                        _startLadder = false;
                        _climbLadder = false;
                        _finishLadder = false;
                        _stopLadder = true;
                    }
                    else
                        _stopLadder = false;
                }

                // Detect right stop ladder
                if (_playerXDirection == XDirection.Right ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Right))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += -_model.BSphere.Radius + 0.5f;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    point2 = _model.BSphere.Center;
                    point2.X += _model.BSphere.Radius + 0.5f;
                    point2.Y += _model.BSphere.Radius / 2f;
                    node2 = _scene.MainLayer.SearchNode(point2);

                    if (node1 == null && node2 == null)
                    {
                        _startLadder = false;
                        _climbLadder = false;
                        _finishLadder = false;
                        _stopLadder = true;
                    }
                    else
                        _stopLadder = false;
                }
            }

            //if (!_doorDetected)
            //{
                // Detect left door
                if (_playerXDirection == XDirection.Left ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Left))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += -_model.BSphere.Radius / 6.0f;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    if (node1 != null && node1.ModelList[0].Key == NodeType.Door)
                    {
                        _doorDetected = true;
                        _door = node1;
                        _doorBox = node1.BoundingBox;
                    }
                    else
                        _doorDetected = false;
                }

                // Detect right door
                if (_playerXDirection == XDirection.Right ||
                   (_playerXDirection == XDirection.None && _lastPlayerXDirection == XDirection.Right))
                {
                    point1 = _model.BSphere.Center;
                    point1.X += _model.BSphere.Radius / 6.0f;
                    node1 = _scene.MainLayer.SearchNode(point1);

                    if (node1 != null && node1.ModelList[0].Key == NodeType.Door)
                    {
                        _doorDetected = true;
                        _door = node1;
                        _doorBox = node1.BoundingBox;
                    }
                    else
                        _doorDetected = false;
                }
            //}
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>Player</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            if(_gun != null)
                _gun.Draw(gameTime);

            _lifeBar.Draw(gameTime);

            base.Draw(gameTime);
        }

        #endregion
    }
}
