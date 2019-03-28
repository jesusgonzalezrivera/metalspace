using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Objects;
using MetalSpace.Scene;
using MetalSpace.Models;
using MetalSpace.Managers;
using MetalSpace.Cameras;
using MetalSpace.Events;
using MetalSpace.Effects;
using MetalSpace.Settings;
using MetalSpace.GameComponents;
using MetalSpace.Interfaces;
using MetalSpace.ArtificialIntelligence;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>MainGameScreen</c> class represent the game where the player,
    /// the enemies and the map are created, destroyed, updated and drawed.
    /// </summary>
    class MainGameScreen : GameScreen
    {
        #region Fields

        /// <summary>
        /// Store for the MapName property.
        /// </summary>
        private string _mapName;

        /// <summary>
        /// Store for the MapInformation property.
        /// </summary>
        //private Dictionary<string, string> _mapInformation;

        /// <summary>
        /// Saved game that contains the data of the player and the enemies.
        /// </summary>
        private SavedGame _savedGame;

        /// <summary>
        /// Scene where the player and the enemies move.
        /// </summary>
        //private SceneRenderer _scene;

        /// <summary>
        /// Store for the MainPlayer property.
        /// </summary>
        private Player _player;

        /// <summary>
        /// New position where the player change the level.
        /// </summary>
        private Vector3 _newPosition;

        /// <summary>
        /// Amount time that the enemies use for change the damaged state.
        /// </summary>
        private float[] _enemiesAmount;

        /// <summary>
        /// Store for the Enemies property.
        /// </summary>
        private Enemy[] _enemies;

        /// <summary>
        /// Store for the ShotManager property.
        /// </summary>
        private ShotManager _shotManager;

        /// <summary>
        /// Store for the ThrownObjectsManager property.
        /// </summary>
        private ThrownObjectManager _thrownObjectsManager;
        
        #endregion

        #region Properties

        /// <summary>
        /// MapName property
        /// </summary>
        /// <value>
        /// Name of the map where the player moves.
        /// </value>
        public string MapName
        {
            get { return _mapName; }
            set { _mapName = value; }
        }

        /// <summary>
        /// MapInformation property
        /// </summary>
        /// <value>
        /// Information of the map.
        /// </value>
        //public Dictionary<string, string> MapInformation
        //{
        //    get { return _mapInformation; }
        //    set { _mapInformation = value; }
        //}

        /// <summary>
        /// MainPlayer property
        /// </summary>
        /// <value>
        /// Main player that the user controls.
        /// </value>
        public Player MainPlayer
        {
            get { return _player; }
            set { _player = value; }
        }

        /// <summary>
        /// Enemies property
        /// </summary>
        /// <value>
        /// List of enemies that exists in the level.
        /// </value>
        public Enemy[] Enemies
        {
            get { return _enemies; }
            set { _enemies = value; }
        }

        /// <summary>
        /// ShotManager property
        /// </summary>
        /// <value>
        /// Manager for the shot that the player shoots.
        /// </value>
        public ShotManager ShotManager
        {
            get { return _shotManager; }
        }

        /// <summary>
        /// ThrownObjectsManager property
        /// </summary>
        /// <value>
        /// Manager for the thrown objects that the enemies throws away.
        /// </value>
        public ThrownObjectManager ThrownObjectsManager
        {
            get { return _thrownObjectsManager; }
        }

        //public SceneRenderer Scene
        //{
        //    get { return _scene; }
        //    set { _scene = value; }
        //}

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>MainGameScreen</c> class.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        /// <param name="mapName">Name of the represented map.</param>
        /// <param name="mapInformation">Information of the level and the enemies.</param>
        public MainGameScreen(string name, string mapName, Dictionary<string, string> mapInformation)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            Name = name;

            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);

            Input.Continuous = true;

            _mapName = mapName;

            //_mapInformation = new Dictionary<string, string>(mapInformation);

            //if(_scene == null)
            //    _scene = new SceneRenderer(mapInformation);

            //SoundManager.GetSound("InGame").Volume = 0.5f;
            //SoundManager.GetSound("InGame").Play(false, true);
        }

        /// <summary>
        /// Constructor of the <c>MainGameScreen</c> class.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        /// <param name="mapName">Name of the represented map.</param>
        /// <param name="mapInformation">Information of the level and the enemies.</param>
        /// <param name="savedGame">Saved game that contains the current state of the player.</param>
        public MainGameScreen(string name, string mapName, Dictionary<string, string> mapInformation, 
            SavedGame savedGame) :
            this(name, mapName, mapInformation)
        {
            _savedGame = savedGame;
        }

        /// <summary>
        /// Constructor of the <c>MainGameScreen</c> class.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        /// <param name="mapName">Name of the represented map.</param>
        /// <param name="mapInformation">Information of the level and the enemies.</param>
        /// <param name="player">Reference to an existing instance of a player.</param>
        /// /// <param name="newPosition">New position of the player in the level.</param>
        public MainGameScreen(string name, string mapName, Dictionary<string, string> mapInformation, 
            Player player, Vector3 newPosition) :
            this(name, mapName, mapInformation)
        {
            _player = player;
            _newPosition = newPosition;
        }

        private bool _sceneLoaded = false;
        public MainGameScreen(string name, string mapName, Dictionary<string, string> mapInformation,
            Player player, Vector3 newPosition, SceneRenderer scene) :
            this(name, mapName, mapInformation)
        {
            //_scene = scene;
            _sceneLoaded = true;

            _player = player;
            _newPosition = newPosition;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load and calculate all the necessary content of the game.
        /// </summary>
        public override void Load()
        {
            base.Load();

            //if(!_sceneLoaded)
            //    _scene.Load();

            if(_player != null)
            {
                CameraManager.ActiveCamera = new ChaseCamera(
                    new Vector3(_newPosition.X, _newPosition.Y + 1f, 17f),
                    new Vector3(_newPosition.X, _newPosition.Y + 1f, 5f),
                    new Vector3(-1.5f, -1.5f, 12),
                    LevelManager.GetLevel(_mapName).Layers2D[0].Rows + 2, LevelManager.GetLevel(_mapName).Layers2D[0].Cols + 2, 12);
                //CameraManager.ActiveCamera = new FreeCamera(new Vector3(0, 5, 10), 0, 0);

                _player = new Player(LevelManager.GetLevel(_mapName), _player.DModel.Model.FileName.Split('/')[2], _newPosition,
                    _player.Rotation, _player.Scale, _player.MaxSpeed, _player.MaxLife, _player.Life,
                    _player.Attack, _player.Debolio, _player.Aerogel, _player.Fulereno, _player.TotalPoints,
                    _player.Objects);
                _player.Load();
                
                foreach (Objects.Object pObject in _player.Objects)
                {
                    if (pObject.IsEquipped && pObject is Objects.Weapon)
                        _player.PlayerGun = new Gun(pObject.Name, ((Objects.Weapon)pObject).Type,
                            _player, ((Objects.Weapon)pObject).Power, ((Objects.Weapon)pObject).TotalAmmo);
                    else if (pObject.IsEquipped && pObject is Objects.Armature)
                        ((AnimatedModel)_player.DModel).MeshesToDraw.Add(pObject.Name);
                }
            }
            else if (_savedGame != null)
            {
                CameraManager.ActiveCamera = new ChaseCamera(
                    new Vector3(_savedGame.PlayerPosition.X, _savedGame.PlayerPosition.Y + 1f, 17f),
                    new Vector3(_savedGame.PlayerPosition.X, _savedGame.PlayerPosition.Y + 1f, 5f),
                    new Vector3(-1.5f, -1.5f, 12),
                    LevelManager.GetLevel(_mapName).Layers2D[0].Rows + 2, LevelManager.GetLevel(_mapName).Layers2D[0].Cols + 2, 12);
                //CameraManager.ActiveCamera = new FreeCamera(new Vector3(0, 5, 10), 0, 0);

                _player = new Player(LevelManager.GetLevel(_mapName), _savedGame.PlayerName.Split('/')[2], _savedGame.PlayerPosition,
                    _savedGame.PlayerRotation, _savedGame.PlayerScale, _savedGame.PlayerMaxSpeed,
                    _savedGame.PlayerMaxLife, _savedGame.PlayerCurrentLife, 10);

                _player.Load();

                int ammoIndex = 0;
                int weaponIndex = 0;
                int armatureIndex = 0;
                for (int i = 0; i < _savedGame.PlayerNumberOfObjects; i++)
                {
                    if (_savedGame.PlayerObjectType[i] == "Armature")
                    {
                        _player.Objects.Add(new Armature(_savedGame.PlayerObjectName[i],
                            _savedGame.PlayerObjectTextureName[i], _savedGame.PlayerObjectPosition[i],
                            _savedGame.PlayerObjectEquipped[i], _savedGame.PlayerObjectArmatureDefense[armatureIndex],
                            _savedGame.PlayerObjectArmatureSkill[armatureIndex], _savedGame.PlayerObjectArmatureType[armatureIndex]));

                        if (_savedGame.PlayerObjectEquipped[i] == true)
                            ((AnimatedModel)_player.DModel).MeshesToDraw.Add(_savedGame.PlayerObjectName[i]);

                        armatureIndex++;
                    }
                    else if (_savedGame.PlayerObjectType[i] == "Weapon")
                    {
                        _player.Objects.Add(new Weapon(_savedGame.PlayerObjectName[i],
                            _savedGame.PlayerObjectTextureName[i], _savedGame.PlayerObjectPosition[i],
                            _savedGame.PlayerObjectEquipped[i], _savedGame.PlayerObjectWeaponType[weaponIndex],
                            _savedGame.PlayerObjectWeaponPower[weaponIndex], _savedGame.PlayerObjectWeaponTotalAmmo[weaponIndex]));

                        if (_savedGame.PlayerObjectEquipped[i] == true)
                        {
                            _player.PlayerGun = new Gun(_savedGame.PlayerObjectName[i], Gun.ShotType.Laser,
                                _player, _savedGame.PlayerObjectWeaponPower[weaponIndex],
                                _savedGame.PlayerObjectWeaponTotalAmmo[weaponIndex]);
                        }

                        weaponIndex++;
                    }
                    else if (_savedGame.PlayerObjectType[i] == "Ammo")
                    {
                        _player.Objects.Add(new Ammo(_savedGame.PlayerObjectName[i],
                            _savedGame.PlayerObjectTextureName[i], _savedGame.PlayerObjectPosition[i],
                            _savedGame.PlayerObjectEquipped[i], _savedGame.PlayerObjectAmmoAmount[ammoIndex],
                            _savedGame.PlayerObjectAmmoType[ammoIndex]));
                        ammoIndex++;
                    }
                    else
                    {
                        _player.Objects.Add(new Objects.Object(_savedGame.PlayerObjectName[i],
                            _savedGame.PlayerObjectTextureName[i], _savedGame.PlayerObjectPosition[i],
                            _savedGame.PlayerObjectEquipped[i]));
                    }
                }
            }
            else
            {
                CameraManager.ActiveCamera = new ChaseCamera(new Vector3(10f, 7.5f, 17f),
                    new Vector3(10f, 7.5f, 5f), new Vector3(-1.5f, -1.5f, 12),
                    LevelManager.GetLevel(_mapName).Layers2D[0].Rows + 2, LevelManager.GetLevel(_mapName).Layers2D[0].Cols + 2, 12);
                //CameraManager.ActiveCamera = new FreeCamera(new Vector3(0, 5, 10), 0, 0);

                int life = 0;
                int attack = 0;
                Vector2 Speed = Vector2.One;
                switch (GameSettings.DefaultInstance.Difficult)
                {
                    case Difficult.Easy:
                        life = 200;
                        attack = 10;
                        Speed = new Vector2(6f, 6f);
                        break;

                    case Difficult.Normal:
                        life = 100;
                        attack = 10;
                        Speed = new Vector2(5f, 5f);
                        break;

                    case Difficult.Hard:
                        life = 50;
                        attack = 10;
                        Speed = new Vector2(4f, 4f);
                        break;
                }

                _player = new Player(LevelManager.GetLevel(_mapName), "AnimatedPlayer",
                    new Vector3(10, 6.5f, 5f),
                    new Vector3(MathHelper.ToRadians(90), 0, 0),
                    Vector3.One,
                    Speed, life, life, attack);

                _player.Load();
            }

            if (_savedGame == null)
            {
                int life = 0;
                int attack = 0;
                Vector2 Speed = Vector2.One;
                switch (GameSettings.DefaultInstance.Difficult)
                {
                    case Difficult.Easy:
                        life = 50;
                        attack = 5;
                        Speed = new Vector2(2f, 2f);
                        break;

                    case Difficult.Normal:
                        life = 75;
                        attack = 10;
                        Speed = new Vector2(3f, 3f);
                        break;

                    case Difficult.Hard:
                        life = 100;
                        attack = 20;
                        Speed = new Vector2(4f, 4f);
                        break;
                }

                _enemiesAmount = new float[Convert.ToInt32(LevelManager.GetLevel(_mapName).MapInformation["NumberOfEnemies"])];
                _enemies = new Enemy[Convert.ToInt32(LevelManager.GetLevel(_mapName).MapInformation["NumberOfEnemies"])];
                for (int i = 0; i < Convert.ToInt32(LevelManager.GetLevel(_mapName).MapInformation["NumberOfEnemies"]); i++)
                {
                    string[] information = LevelManager.GetLevel(_mapName).MapInformation["Enemy" + i].Split(' ');

                    List<string> objects = new List<string>();
                    for (int j = 10; j < information.Length; j++)
                        objects.Add(information[j]);

                    _enemies[i] = new Enemy(
                        LevelManager.GetLevel(_mapName), information[0],
                        new Vector3((float)Convert.ToDouble(information[1]), (float)Convert.ToDouble(information[2]), (float)Convert.ToDouble(information[3])),
                        new Vector3(MathHelper.ToRadians((float)Convert.ToDouble(information[4])), MathHelper.ToRadians((float)Convert.ToDouble(information[5])), MathHelper.ToRadians((float)Convert.ToDouble(information[6]))),
                        new Vector3((float)Convert.ToDouble(information[7]), (float)Convert.ToDouble(information[8]), (float)Convert.ToDouble(information[9])),
                        Speed, life, attack, objects);

                    _enemies[i].Load();

                    _enemies[i].Context = new EnemyContext(LevelManager.GetLevel(_mapName).MainLayer, _player, _enemies[i]);
                    _enemies[i].Context.ChangeState(Inactive.Instance);
                }
            }
            else
            {
                _enemies = new Enemy[_savedGame.NumberOfEnemies];
                _enemiesAmount = new float[_savedGame.NumberOfEnemies];
                for (int i = 0; i < _savedGame.NumberOfEnemies; i++)
                {
                    _enemies[i] = new Enemy(LevelManager.GetLevel(_mapName), _savedGame.EnemyName[i].Split('/')[2], 
                        _savedGame.EnemyPosition[i], _savedGame.EnemyRotation[i], _savedGame.EnemyScale[i],
                        _savedGame.EnemyMaxSpeed[i], _savedGame.EnemyMaxLife[i], _savedGame.EnemyAttack[i]);

                    _enemies[i].Load();

                    _enemies[i].Context = new EnemyContext(LevelManager.GetLevel(_mapName).MainLayer, _player, _enemies[i]);
                    _enemies[i].Context.ChangeState(Inactive.Instance);
                }
            }

            _shotManager = new ShotManager(LevelManager.GetLevel(_mapName), _player, _enemies);
            _shotManager.Load();

            _thrownObjectsManager = new ThrownObjectManager(LevelManager.GetLevel(_mapName), _player);
            _thrownObjectsManager.Load();

            int numberOfObjects = Convert.ToInt32(LevelManager.GetLevel(_mapName).MapInformation["NumberOfObjects"]);
            for(int i=0; i < numberOfObjects; i++)
            {
                string[] information = LevelManager.GetLevel(_mapName).MapInformation["Object" + i].Split(' ');

                Vector3 position = new Vector3((float)Convert.ToDouble(information[1]), (float)Convert.ToDouble(information[2]), (float)Convert.ToDouble(information[3]));
                Vector3 rotation = new Vector3((float)Convert.ToDouble(information[4]), (float)Convert.ToDouble(information[5]), (float)Convert.ToDouble(information[6]));
                Vector3 scale = new Vector3((float)Convert.ToDouble(information[7]), (float)Convert.ToDouble(information[8]), (float)Convert.ToDouble(information[9]));
                ThrownObjectsManager.AddObject(new MoveableObject(LevelManager.GetLevel(_mapName), new DrawableModel(
                    (GameModel)ModelManager.GetModel(information[0] + "Object"), position, rotation, scale, 0), Vector2.Zero, true, true));
            }
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the necessary elements of the main game screen.
        /// </summary>
        public override void Unload()
        {
            base.Unload();

            //_player.Unload();
            //_player = null;

            foreach (Enemy enemy in _enemies)
                enemy.Unload();
            _enemies = null;

            //_scene.Unload();
            //_scene = null;

            //_mapInformation.Clear();
            //_mapInformation = null;
        }

        #endregion

        #region HandleInput method

        private float _amount = 0.0f;

        /// <summary>
        /// Handle the input of the user relative to the <c>MainGameScreen</c> screen.
        /// </summary>
        /// <param name="input">Current state of the player input.</param>
        /// <param name="gameTime">Current time of the game.</param>
        public override void HandleInput(GameComponents.Input input, GameTime gameTime)
        {
            if (input.Up)
            {
                if (_player.PlayerYDirection != Player.YDirection.Up)
                    EventManager.QueueEvent(new EventData_UnitDirectionYChanged(ref _player, Player.YDirection.Up));
            }

            if (input.Down)
            {
                if (_player.PlayerYDirection != Player.YDirection.Down)
                    EventManager.QueueEvent(new EventData_UnitDirectionYChanged(ref _player, Player.YDirection.Down));
            }

            if (!input.Up && !input.Down)
            {
                if (_player.PlayerYDirection != Player.YDirection.None)
                    EventManager.QueueEvent(new EventData_UnitDirectionYChanged(ref _player, Player.YDirection.None));
            }

            if (input.Left)
            {
                if (_player.PlayerXDirection != Player.XDirection.Left)
                    EventManager.QueueEvent(new EventData_UnitDirectionXChanged(ref _player, Player.XDirection.Left));

                if (_player.PlayerState == Player.State.Waiting)
                    EventManager.QueueEvent(new EventData_UnitStateChanged(_player, Player.State.Running));
            }

            if (input.Right)
            {
                if (_player.PlayerXDirection != Player.XDirection.Right)
                    EventManager.QueueEvent(new EventData_UnitDirectionXChanged(ref _player, Player.XDirection.Right));

                if (_player.PlayerState == Player.State.Waiting)
                    EventManager.QueueEvent(new EventData_UnitStateChanged(_player, Player.State.Running));
            }

            if (!input.Left && !input.Right)
            {
                if (_player.PlayerXDirection != Player.XDirection.None)
                    EventManager.QueueEvent(new EventData_UnitDirectionXChanged(ref _player, Player.XDirection.None));

                if (_player.PlayerState == Player.State.Running)// || _player.PlayerState == GameUnit.State.Jumping)
                    EventManager.QueueEvent(new EventData_UnitStateChanged(_player, Player.State.Waiting));
            }

            if (input.Attack)
            {
                _amount += (float)(gameTime.ElapsedGameTime.Milliseconds / 1000f);
                if (_player.PlayerGun != null && _amount > _player.ShotSpeed)
                {
                    Vector3 origin = Vector3.Zero;
                    Vector3 up = Vector3.Zero;
                    Vector3 speed = Vector3.Zero;

                    if (_player.PlayerState == Player.State.Jumping)
                    {
                        if (_player.PlayerXDirection == Character.XDirection.Left ||
                            (_player.PlayerXDirection == Character.XDirection.None &&
                            _player.LastPlayerXDirection == Character.XDirection.Left))
                        {
                            origin = new Vector3(
                                _player.DModel.BSphere.Center.X - _player.DModel.BSphere.Radius / 1f,
                                _player.DModel.BSphere.Center.Y + _player.DModel.BSphere.Radius / 2.5f,
                                _player.DModel.BSphere.Center.Z);
                            up = -Vector3.Up;
                            speed = new Vector3(-10f, 0, 0);
                        }
                        else if (_player.PlayerXDirection == Character.XDirection.Right ||
                            (_player.PlayerXDirection == Character.XDirection.None &&
                            _player.LastPlayerXDirection == Character.XDirection.Right))
                        {
                            origin = new Vector3(
                                _player.DModel.BSphere.Center.X + _player.DModel.BSphere.Radius / 1.2f,
                                _player.DModel.BSphere.Center.Y + _player.DModel.BSphere.Radius / 4.5f,
                                _player.DModel.BSphere.Center.Z);
                            up = Vector3.Up;
                            speed = new Vector3(10f, 0, 0);
                        }
                    }
                    else if (_player.PlayerXDirection == Character.XDirection.None &&
                        _player.PlayerYDirection == Character.YDirection.Up)
                    {
                        origin = new Vector3(
                            _player.DModel.BSphere.Center.X,
                            _player.DModel.BSphere.Center.Y + _player.DModel.BSphere.Radius * 1.5f,
                            _player.DModel.BSphere.Center.Z);
                        up = new Vector3(-1f, 0, 0);
                        speed = new Vector3(0, 10f, 0);
                    }
                    else if (_player.PlayerXDirection == Character.XDirection.Left ||
                        (_player.PlayerXDirection == Character.XDirection.None && _player.LastPlayerXDirection == Character.XDirection.Left))
                    {
                        if (_player.PlayerYDirection == Character.YDirection.Up)
                        {
                            origin = new Vector3(
                                _player.DModel.BSphere.Center.X - _player.DModel.BSphere.Radius / 1.3f,
                                _player.DModel.BSphere.Center.Y + _player.DModel.BSphere.Radius / 0.85f,
                                _player.DModel.BSphere.Center.Z);
                            up = new Vector3(-0.75f, -0.75f, 0);
                            speed = new Vector3(-10f, 10f, 0);
                        }
                        else
                        {
                            origin = new Vector3(
                                _player.DModel.BSphere.Center.X - _player.DModel.BSphere.Radius / 0.9f,
                                _player.DModel.BSphere.Center.Y + _player.DModel.BSphere.Radius / 4.5f,
                                _player.DModel.BSphere.Center.Z);
                            up = Vector3.Up;
                            speed = new Vector3(-10f, 0, 0);
                        }
                    }
                    else if (_player.PlayerXDirection == Character.XDirection.Right ||
                        (_player.PlayerXDirection == Character.XDirection.None && _player.LastPlayerXDirection == Character.XDirection.Right))
                    {
                        if (_player.PlayerYDirection == Character.YDirection.Up)
                        {
                            origin = new Vector3(
                                _player.DModel.BSphere.Center.X + _player.DModel.BSphere.Radius / 1.3f,
                                _player.DModel.BSphere.Center.Y + _player.DModel.BSphere.Radius / 0.9f,
                                _player.DModel.BSphere.Center.Z);
                            up = new Vector3(-0.75f, 0.75f, 0);
                            speed = new Vector3(10f, 10f, 0);
                        }
                        else
                        {
                            origin = new Vector3(
                                _player.DModel.BSphere.Center.X + _player.DModel.BSphere.Radius / 1.2f,
                                _player.DModel.BSphere.Center.Y + _player.DModel.BSphere.Radius / 4.5f,
                                _player.DModel.BSphere.Center.Z);
                            up = Vector3.Up;
                            speed = new Vector3(10f, 0, 0);
                        }
                    }

                    if (_player.PlayerState != Player.State.Climbing)
                    {
                        if (_player.PlayerGun.CurrentAmmo != 0)
                        {
                            if (_player.PlayerGun.GunType == Gun.ShotType.Laser)
                            {
                                SoundManager.GetSound("LaserShot").Volume = GameSettings.DefaultInstance.SoundVolume;
                                SoundManager.GetSound("LaserShot").Play(true, false);
                            }
                            else
                            {
                                SoundManager.GetSound("MetalShot").Volume = GameSettings.DefaultInstance.SoundVolume;
                                SoundManager.GetSound("MetalShot").Play(true, false);
                            }

                            _shotManager.AddShot(new Shot(_player.PlayerGun.GunType, origin, up, speed));

                            _player.PlayerGun.CurrentAmmo -= 1;
                        }
                        else
                        {
                            foreach (Objects.Object playerObject in _player.Objects)
                                if (playerObject is Objects.Ammo &&
                                    ((Objects.Ammo)playerObject).Type == _player.PlayerGun.GunType)
                                {
                                    EventManager.Trigger(new EventData_ReloadGun(_player, playerObject));
                                    ((AnimatedModel)_player.DModel).Animation.StartClip("Reload", false);
                                }
                        }
                    }

                    _amount = 0.0f;
                }
            }

            if (input.Jump)
            {
                EventManager.QueueEvent(new EventData_UnitStateChanged(_player, Player.State.Jumping));
            }

            if (input.Select)
            {
                Input.Continuous = false;
                ScreenManager.AddScreen("AGameMenu", new GameMenuScreen(this, "AGameMenu", 25f));
            }

            if (input.Start)
            {
                ((ChaseCamera)CameraManager.ActiveCamera).Position = new Vector3(10f, 7.5f, 18f);
                ((ChaseCamera)CameraManager.ActiveCamera).Target = new Vector3(10f, 7.5f, 5f);

                _player.Position = new Vector3(10, 6.5f, 5f);
            }

            if (input.Inventory)
            {
                SoundManager.GetSound("Running").Pause();
                ScreenManager.AddScreen("Inventory", new InventoryScreen(
                    this, "Inventory", "InventoryBackground", _player));
            }

            /*((FreeCamera)CameraManager.ActiveCamera).Rotate(input.MouseMoved.X * 0.01f, input.MouseMoved.Y * 0.01f);

            Vector3 moveVector = Vector3.Zero;
            if (input.Up)    moveVector += Vector3.Forward;
            if (input.Down)  moveVector += Vector3.Backward;
            if (input.Right) moveVector += Vector3.Left;
            if (input.Left)  moveVector += Vector3.Right;
            moveVector *= ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f) * 30.0f;
            
            ((FreeCamera)CameraManager.ActiveCamera).Move(moveVector);*/
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the state of the screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        /// <param name="otherScreenHasFocus">true if other screen has the focus, false otherwise.</param>
        /// <param name="coveredByOtherScreen">true if other screen cover this screen, false otherwise.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (!base.IsActive)
                return;

            LevelManager.GetLevel(_mapName).Update(gameTime);

            Vector3 playerPosition = _player.Position;
            _player.Update(gameTime);

            for(int i=0; i < _enemies.Length; i++)
            {
                _enemies[i].Update(gameTime);
                if (_enemies[i].AttackMade)
                {
                    _enemies[i].AttackMade = false;
                    EventManager.Trigger(new EventData_CharactersAttack(_enemies[i], _player));
                    _enemiesAmount[i] = 0.0f;
                }
                else
                {
                    _enemiesAmount[i] += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                    if (_enemiesAmount[i] >= 1.0f)
                    {
                        _enemiesAmount[i] = 0.0f;
                        ((AnimatedModel)_player.DModel).Damaged = false;
                    }
                }
            }

            ShotManager.Update(gameTime);
            _thrownObjectsManager.Update(gameTime);

            ((ChaseCamera)CameraManager.ActiveCamera).Move(_player, _player.Position - playerPosition);
            ((ChaseCamera)CameraManager.ActiveCamera).Update(gameTime);
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>MainGameScreen</c> screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            GameGraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GameGraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GameGraphicsDevice.BlendState = BlendState.Opaque;

            GameGraphicsDevice.BlendState = BlendState.AlphaBlend;
            GameGraphicsDevice.DepthStencilState = DepthStencilState.Default;

            ScreenManager.SpriteBatch.Begin();

            LevelManager.GetLevel(_mapName).Draw(gameTime, null);
            
            _player.Draw(gameTime);

            foreach (Enemy enemy in _enemies)
                enemy.Draw(gameTime);

            ShotManager.Draw(gameTime);
            _thrownObjectsManager.Draw(gameTime);

            // Set the World, View, and Projection Matrices for the Particle Systems before Drawing them
            EngineManager.ParticleManager.ParticleSystem.SetWorldViewProjectionMatricesForAllParticleSystems(
                Matrix.Identity, CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection);

            // Let all of the particle systems know of the Camera's current position
            EngineManager.ParticleManager.ParticleSystem.SetCameraPositionForAllParticleSystems(
                CameraManager.ActiveCamera.Position);

            // Update all of the Particle Systems
            EngineManager.ParticleManager.ParticleSystem.UpdateAllParticleSystems(
                (float)gameTime.ElapsedGameTime.TotalSeconds);

            ScreenManager.SpriteBatch.End();
            
            base.Draw(gameTime);
        }

        #endregion
    }
}
