using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Sound;
using MetalSpace.Models;
using MetalSpace.Objects;
using MetalSpace.Textures;
using MetalSpace.Managers;
using MetalSpace.Events;
using MetalSpace.Settings;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>ChangingGame</c> class represents the screen that appears to the
    /// user when he/she change the current level. Unlike <c>LoadGameScreen</c>,
    /// in this case it is only necessary to dispose the elements not used in
    /// the next level and load the not loaded resources.
    /// </summary>
    class ChangingGame : GameScreen
    {
        #region Fields

        /// <summary>
        /// Const that contains a threshold of time to ensure that the elements
        /// are loaded and disposed.
        /// </summary>
        public const int totalSafeTime = 1500;

        /// <summary>
        /// Const string that contains the name of the background texture.
        /// </summary>
        private const string backgroundTexture = "LoadingBackground";

        /// <summary>
        /// Title that appears in the top of the screen.
        /// </summary>
        private const string menuTitle = "MetalSpace";

        /// <summary>
        /// Time since the start of this screen.
        /// </summary>
        public int _safeTime;

        /// <summary>
        /// Name of the map where the player goes
        /// </summary>
        private string _mapName;

        /// <summary>
        /// Saved game that should have the new map.
        /// </summary>
        private SavedGame _savedGame = null;

        /// <summary>
        /// Reference to an existing player instance.
        /// </summary>
        private Player _player = null;

        /// <summary>
        /// Start position of the player in the new level.
        /// </summary>
        private Vector3 _newPosition = Vector3.Zero;

        /// <summary>
        /// Information of the map (layers, doors...).
        /// </summary>
        private Dictionary<string, string> _mapInformation;

        /// <summary>
        /// Resources that should be loaded.
        /// </summary>
        private List<string> _loadableResources;

        /// <summary>
        /// Resources that should be diposed.
        /// </summary>
        private List<string> _removableResources;

        /// <summary>
        /// true if it is necessary to dispose resources and false otherwise.
        /// </summary>
        private bool _state;

        /// <summary>
        /// Total number of elements to be released.
        /// </summary>
        private int _totalReleasablesResources;

        /// <summary>
        /// Number of elements released.
        /// </summary>
        private int _releasedResources;

        /// <summary>
        /// Total number of elements to be loaded.
        /// </summary>
        private int _totalLoadableResources;

        /// <summary>
        /// Number of elements loaded.
        /// </summary>
        private int _loadedResources;

        /// <summary>
        /// Position of the bar that indicates the progress.
        /// </summary>
        private Vector2 _positionBar;

        /// <summary>
        /// Position of the bar that indicates the progress.
        /// </summary>
        private Vector2 _positionLoading;

        /// <summary>
        /// The position of the title in the screen.
        /// </summary>
        private Vector2 _positionTitle;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>ChangingGame</c> class.
        /// </summary>
        /// <param name="mapName">Name of the map where the player goes.</param>
        public ChangingGame(string mapName)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            _state = false;

            _mapName = mapName;

            _loadableResources = new List<string>();
            _removableResources = new List<string>();

            _mapInformation = FileHelper.readMapInformation(mapName);
            List<string> resources = new List<string>(_mapInformation["Elements"].Split(' '));

            List<string> loadedModels = ModelManager.GetKeys();
            List<string> loadedTextures = TextureManager.GetKeys();
            List<string> loadedSounds = SoundManager.GetKeys();
            foreach(string resource in resources)
            {
                if (resource.Split('_')[1] == "Texture")
                {
                    if (!loadedTextures.Contains(resource.Split('_')[0]))
                        _loadableResources.Add(resource);
                }
                else if (resource.Split('_')[1] == "Model")
                {
                    if (!loadedModels.Contains(resource.Split('_')[0]))
                        _loadableResources.Add(resource);
                }
                else if (resource.Split('_')[1] == "Sound")
                {
                    if (!loadedSounds.Contains(resource.Split('_')[0]))
                        _loadableResources.Add(resource);
                }
            }

            foreach (string resource in loadedModels)
                if (!resources.Contains(resource + "_Model"))
                    _removableResources.Add(resource + "_Model");

            foreach (string resource in loadedTextures)
                if (!resources.Contains(resource + "_Texture"))
                    _removableResources.Add(resource + "_Texture");

            foreach (string resource in loadedSounds)
                if (!resources.Contains(resource + "_Sound"))
                    _removableResources.Add(resource + "_Sound");

            _removableResources.Remove("SeparationBar_Texture");
            _removableResources.Remove("SelectedMenuEntry_Texture");
            _removableResources.Remove("DummyTexture15T_Texture");
            _removableResources.Remove("DummyTexture_Texture");
            _removableResources.Remove("BarTexture_Texture");
            _removableResources.Remove("DialogBackground_Texture");
            _removableResources.Remove("DeadBackground_Texture");
            _removableResources.Remove("LoadingBackground_Texture");
            _removableResources.Remove("MenuBackground_Texture");
            _removableResources.Remove("Menu_Sound");
            _removableResources.Remove("MenuSelect_Sound");
            _removableResources.Remove("MenuAccept_Sound");
            
            _loadedResources = 0;
            _totalLoadableResources = _loadableResources.Count;
            _releasedResources = 0;
            _totalReleasablesResources = _removableResources.Count;
        }

        /// <summary>
        /// Constructor of the <c>ChangingGame</c> class.
        /// </summary>
        /// <param name="mapName">Name of the map where the player goes.</param>
        /// <param name="savedGame">Saved game to be used in the level.</param>
        public ChangingGame(string mapName, SavedGame savedGame) : 
            this(mapName)
        {
            _savedGame = savedGame;
        }

        
        /// <summary>
        /// Constructor of the <c>ChangingGame</c> class.
        /// </summary>
        /// <param name="mapName">Name of the map where the player goes.</param>
        /// <param name="player">Reference to an existing instance of a player.</param>
        /// <param name="newPosition">Position of the player in the level.</param>
        public ChangingGame(string mapName, Player player, Vector3 newPosition) :
            this(mapName)
        {
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

            _positionBar = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth / 2f - TextureManager.GetTexture("BarTexture").BaseTexture.Width / 2f,
                GameSettings.DefaultInstance.ResolutionHeight / 3f - TextureManager.GetTexture("BarTexture").BaseTexture.Height / 2f);

            Vector2 titleSize = ScreenManager.FontTitle.MeasureString(menuTitle);
            Vector2 titleCenter = new Vector2(GameSettings.DefaultInstance.ResolutionWidth / 2.5f, 75f);
            _positionTitle = new Vector2(50f, titleCenter.Y - (titleSize.Y / 2));

            titleSize = ScreenManager.FontTitle.MeasureString("Loading ...");
            _positionLoading = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth / 2f - titleSize.X / 5f,
                GameSettings.DefaultInstance.ResolutionHeight / 3f - titleSize.Y / 2f);

            SoundManager.StopAllSounds();
            EngineManager.Game.IsFixedTimeStep = true;

            base.Load();
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the necessary elements of the changing game screen.
        /// </summary>
        public override void Unload()
        {
            base.Unload();
        }

        #endregion

        #region HandleInput Method

        /// <summary>
        /// Handle the input of the user relative to the <c>ChangingGame</c> screen.
        /// </summary>
        /// <param name="input">Current state of the player input.</param>
        /// <param name="gameTime">Current time of the game.</param>
        public override void HandleInput(GameComponents.Input input, GameTime gameTime)
        {
            base.HandleInput(input, gameTime);
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
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            _safeTime += gameTime.ElapsedGameTime.Milliseconds;

            if (_state == false)
            {
                if (_removableResources.Count != 0)
                {
                    string resource = _removableResources[0];
                    string[] information = resource.Split('_');
                    if (information[1] == "Texture")
                    {
                        ThreadStart threadStarter = delegate
                        {
                            TextureManager.RemoveTexture(information[0]);
                            _releasedResources++;
                        };

                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }
                    else if (information[1] == "Model")
                    {
                        ThreadStart threadStarter = delegate
                        {
                            ModelManager.RemoveModel(information[0]);
                            _releasedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }
                    else if (information[1] == "Sound")
                    {
                        ThreadStart threadStarter = delegate
                        {
                            SoundManager.RemoveSound(information[0]);
                            _releasedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }

                    _removableResources.Remove(resource);
                }
                else
                    _state = true;
            }
            else
            {
                if (_loadableResources.Count != 0)
                {
                    string resource = _loadableResources[0];
                    string[] information = resource.Split('_');
                    if (information[1] == "Texture")
                    {
                        ThreadStart threadStarter = delegate
                        {
                            TextureManager.AddTexture(information[0], new GameTexture("Content/Textures/" + information[0]));
                            _loadedResources++;
                        };

                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }
                    else if (information[1] == "Model")
                    {
                        ThreadStart threadStarter = delegate
                        {
                            ModelManager.AddModel(information[0], new GameModel("Content/Models/" + information[0]));
                            _loadedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }
                    else if (information[1] == "Sound")
                    {
                        ThreadStart threadStarter = delegate
                        {
                            SoundManager.AddSound(information[0], new GameSound("Content/Sounds/" + information[0]));
                            _loadedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }

                    _loadableResources.Remove(resource);
                }
            }

            if (_releasedResources >= _totalReleasablesResources - 1 && 
                _loadedResources >= _totalLoadableResources - 1 &&
                _safeTime > totalSafeTime)
            {
                EngineManager.Game.IsFixedTimeStep = false;
                ScreenManager.RemoveScreen("LoadingScreen");
                if (_savedGame != null)
                {
                    ScreenManager.AddScreen("ContinueGame", new MainGameScreen(
                        "ContinueGame", _mapName, _mapInformation, _savedGame));
                }
                else if (_player != null)
                {
                    ScreenManager.AddScreen("ContinueGame", new MainGameScreen(
                        "ContinueGame", _mapName, _mapInformation, _player, _newPosition));
                }
                else
                {
                    ScreenManager.AddScreen("ContinueGame", new MainGameScreen(
                        "ContinueGame", _mapName, _mapInformation));
                }
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>ChangingGame</c> screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            Texture2D borderTexture = TextureManager.GetTexture("BarTexture").BaseTexture as Texture2D;

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, CanvasManager.ScaleMatrix);

            // Draw the background
            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture(backgroundTexture).BaseTexture as Texture2D,
                new Rectangle(0, 0, GameSettings.DefaultInstance.ResolutionWidth, GameSettings.DefaultInstance.ResolutionHeight),
                new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            // Draw the title
            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontTitle, menuTitle, _positionTitle, Color.White);

            // Draw the loading bar
            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected, "Loading...",
                _positionLoading, Color.White);

            double eachPercentWidth = (double)borderTexture.Width / (double)(_totalReleasablesResources + _totalLoadableResources);
            int xPosition = GameSettings.DefaultInstance.ResolutionWidth / 3;
            ScreenManager.SpriteBatch.Draw(
                TextureManager.GetTexture("DummyTexture").BaseTexture as Texture2D,
                new Rectangle((int)_positionBar.X, (int)_positionBar.Y,
                    (int)((_loadedResources + _releasedResources) * eachPercentWidth), (int)(borderTexture.Height * 0.75f)),
                Color.CadetBlue);

            ScreenManager.SpriteBatch.Draw(
                borderTexture,
                new Rectangle((int)_positionBar.X, (int)_positionBar.Y, borderTexture.Width, (int)(borderTexture.Height * 0.75f)),
                Color.White);

            ScreenManager.SpriteBatch.End();
        }

        #endregion
    }
}
