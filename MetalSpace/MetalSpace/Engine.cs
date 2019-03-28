using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using DPSF;
using MetalSpace.Events;
using MetalSpace.Effects;
using MetalSpace.Cameras;
using MetalSpace.Settings;
using MetalSpace.Managers;
using MetalSpace.GameScreens;
using MetalSpace.GameComponents;

namespace MetalSpace
{
    class Engine : Game
    {
        #region Fields 

        private static bool _isActive;

        private static string _windowTitle;

        private static bool _applyDeviceChanges;
        
        #endregion

        #region GameComponents

        private static GraphicsDeviceManager _graphicsDeviceManager = null;

        private static ContentManager _loader = null;
        private static ContentManager _contentManager = null;
        
        private static FpsCounter _fpsCounter = null;
        private static ScreenManager _screenManager = null;
        private static CameraManager _cameraManager = null;

        private static TextureManager _textureManager = null;
        private static ModelManager _modelManager = null;
        private static SoundManager _soundManager = null;
        
        private static Input _input = null;
        
        private static EventManager _eventManager = null;

        private static ParticleManager _particleManager = null;

        private static LevelManager _levelManager = null;

        #endregion

        # region Properties

        public static bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public static string WindowTitle
        {
            get { return _windowTitle; }
            set { _windowTitle = value; }
        }

        public static bool ApplyDeviceChanges
        {
            get { return _applyDeviceChanges; }
            set { _applyDeviceChanges = value; }
        }
        
        public static ContentManager ContentManager
        {
            get { return _contentManager; }
        }

        public static ContentManager Loader
        {
            get { return _loader; }
            set { _loader = value; }
        }
        
        public static GraphicsDevice GameGraphicsDevice
        {
            get { return _graphicsDeviceManager.GraphicsDevice; }
        }

        public static FpsCounter FpsCounter
        {
            get { return _fpsCounter; }
        }

        public static TextureManager TextureManager
        {
            get { return _textureManager; }
        }

        public static ModelManager ModelManager
        {
            get { return _modelManager; }
        }

        public static SoundManager SoundManager
        {
            get { return _soundManager; }
        }

        public static Input Input
        {
            get { return _input; }
        }

        public static ScreenManager ScreenManager
        {
            get { return _screenManager; }
        }

        public static CameraManager CameraManager
        {
            get { return _cameraManager; }
        }

        public static EventManager EventManager
        {
            get { return _eventManager; }
            set { _eventManager = value; }
        }

        public static ParticleManager ParticleManager
        {
            get { return _particleManager; }
            set { _particleManager = value; }
        }

        public static LevelManager LevelManager
        {
            get { return _levelManager; }
            set { _levelManager = value; }
        }

        #endregion

        # region Constructor

        public Engine() : this("MetalSpace") { }

        public Engine(string windowTitle)
        {   
            _isActive = true;
            _windowTitle = windowTitle;
            _applyDeviceChanges = false;
            
            Content.RootDirectory = "Content";

            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            _graphicsDeviceManager.PreparingDeviceSettings += new EventHandler
                <PreparingDeviceSettingsEventArgs>(GraphicsDeviceManager_PreparingDeviceSetting);

            EngineManager.Game = this;

            _loader = new ContentManager(Services);
            _contentManager = new ContentManager(Services);

            FileHelper.Initialize();
            GameSettings.Initialize();
            StringHelper.Initialize();

            _fpsCounter = new FpsCounter(this);
            Components.Add(_fpsCounter);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            _cameraManager = new CameraManager(this);
            Components.Add(_cameraManager);

            _textureManager = new TextureManager(this);
            Components.Add(_textureManager);

            _modelManager = new ModelManager(this);
            Components.Add(_modelManager);

            _soundManager = new SoundManager(this);
            Components.Add(_soundManager);

            _input = new Input(this);
            Components.Add(_input);

            _eventManager = new Managers.EventManager();
            EventManager.AddListener(new EventListener_LogMessage("LogXNA-MetalSpace.txt"), EventManager.EventType_LogMessage);
            EventManager.AddListener(new EventListener_UnitCollision(), EventManager.EventType_UnitCollision);
            EventManager.AddListener(new EventListener_CharactersAttack(), EventManager.EventType_CharactersAttack);
            EventManager.AddListener(new EventListener_UnitStateChanged(), EventManager.EventType_UnitStateChanged);
            EventManager.AddListener(new EventListener_UnitDirectionChanged(), EventManager.EventType_UnitDirectionChanged);
            EventManager.AddListener(new EventListener_LevelChanged(), EventManager.EventType_LevelChanged);
            EventManager.AddListener(new EventListener_ChangePlayerObjects(), EventManager.EventType_ChangePlayerObjects);
            EventManager.AddListener(new EventListener_SavedGames(), EventManager.EventType_SavedGames);
            EventManager.AddListener(new EventListener_PickedObject(), EventManager.EventType_PickedObject);

            _particleManager = new ParticleManager(this);
            Components.Add(_particleManager);

            CanvasManager.Init(ref _graphicsDeviceManager);
            CanvasManager.SetVirtualResolution(1024, 768);
            CanvasManager.SetResolution(
                GameSettings.DefaultInstance.WindowsResolutionWidth,
                GameSettings.DefaultInstance.WindowsResolutionHeight,
                GameSettings.DefaultInstance.FullScreen);

            _levelManager = new Managers.LevelManager(this);
            Components.Add(_levelManager);

            _graphicsDeviceManager.SynchronizeWithVerticalRetrace = 
                GameSettings.DefaultInstance.VerticalSyncronization;
                
            //this.IsFixedTimeStep = true;
            this.IsFixedTimeStep = false;
        }
        
        #endregion

        #region Override Methods

        protected override void Initialize()
        {
            base.Initialize();
            
            _graphicsDeviceManager.DeviceReset +=
                new EventHandler<System.EventArgs>(GraphicsDeviceManager_DeviceReset);
            GraphicsDeviceManager_DeviceReset(null, EventArgs.Empty);
        }

        protected override void LoadContent()
        {
            ScreenManager.AddScreen("Background", new BackgroundScreen("Background"));
            ScreenManager.AddScreen("MainMenu", new MainMenuScreen("MainMenu"));
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            EventManager.Tick();

            this.Window.Title = WindowTitle + " - FPS: " + FpsCounter.FPS.ToString();

            if (_applyDeviceChanges)
            {
                _graphicsDeviceManager.ApplyChanges();
                _applyDeviceChanges = false;
            }
        }

        protected override void Draw(GameTime gametime)
        {
            base.Draw(gametime);
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);

            _isActive = true;
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);

            _isActive = false;
        }

        #endregion

        #region Event Handlers

        private void GraphicsDeviceManager_PreparingDeviceSetting(object sender, PreparingDeviceSettingsEventArgs e)
        {
            for (int i = 0; i < GraphicsAdapter.Adapters.Count; i++)
            {
                if (GraphicsAdapter.Adapters[i].IsProfileSupported(GraphicsProfile.Reach))
                {
                    e.GraphicsDeviceInformation.Adapter = GraphicsAdapter.Adapters[i];
                    e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.Reach;

                    return;
                }
                else
                {
                    e.GraphicsDeviceInformation.Adapter = GraphicsAdapter.Adapters[i];
                    e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;

                    return;
                }
            }
            //e.GraphicsDeviceInformation.Adapter = GraphicsAdapter.Adapters[0];
            //e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;

            return;
        }

        void GraphicsDeviceManager_DeviceReset(object sender, EventArgs e) { }

        #endregion
    }
}
