using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Sound;
using MetalSpace.Models;
using MetalSpace.Events;
using MetalSpace.Managers;
using MetalSpace.Textures;
using MetalSpace.Settings;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>LoadGameScreen</c> class represent the screen that appears to the
    /// user load or start a game. Before show the game screen, it load all the
    /// resources needed for the level.
    /// </summary>
    class LoadingGame : GameScreen
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
        /// Information of the map (layers, doors...).
        /// </summary>
        private Dictionary<string, string> _mapInformation;

        /// <summary>
        /// Store for the Resources property.
        /// </summary>
        private List<string> _resources;
        private List<string> _loadedResources;

        /// <summary>
        /// true if the resources should be loaded, false otherwise.
        /// </summary>
        private bool _create;

        /// <summary>
        /// Total resources that should be loaded.
        /// </summary>
        private int _totalResources;

        /// <summary>
        /// Current number of elements to be loaded.
        /// </summary>
        private int _numberOfLoadedResources;

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

        #region Properties

        /// <summary>
        /// Resources property
        /// </summary>
        /// <value>
        /// List of resources that should be loaded.
        /// </value>
        public List<string> Resources
        {
            get { return _resources; }
            set { _resources = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>LoadingGame</c> class.
        /// </summary>
        /// <param name="mapName">Name of the map to be loaded.</param>
        /// <param name="create">true if the resources should be loaded, false otherwise.</param>
        /// <param name="savedGame">Saved game that should be loaded.</param>
        public LoadingGame(string mapName, bool create = true, SavedGame savedGame = null)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            _create = create;
            _savedGame = savedGame;
            _mapName = mapName;
            _mapInformation = FileHelper.readMapInformation(mapName);
            
            _resources = new List<string>(_mapInformation["Elements"].Split(' '));
            _resources.Add("Map_1_1.txt");
            _resources.Add("Map_1_2.txt");
            _resources.Add("Map_1_3.txt");
            _resources.Add("Map_1_4.txt");
            _resources.Add("Map_1_5.txt");

            _loadedResources = new List<string>();

            _safeTime = 0;
            _numberOfLoadedResources = 0;
            _totalResources = _resources.Count;
        }

        /// <summary>
        /// Constructor of the <c>LoadingGame</c> class.
        /// </summary>
        /// <param name="mapName">Name of the map to be loaded.</param>
        /// <param name="create">true if the resources should be loaded, false otherwise.</param>
        public LoadingGame(Dictionary<string, string> mapInformation, bool create = false)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            _create = create;

            _resources = new List<string>(mapInformation["Elements"].Split(' '));
            _resources.Add("Map_1_1.txt");
            _resources.Add("Map_1_2.txt");
            _resources.Add("Map_1_3.txt");
            _resources.Add("Map_1_4.txt");
            _resources.Add("Map_1_5.txt");

            _loadedResources = new List<string>();

            _numberOfLoadedResources = 0;
            _totalResources = _resources.Count;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load and calculate all the necessary content of the game.
        /// </summary>
        public override void Load()
        {
            _positionBar = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth/2f - TextureManager.GetTexture("BarTexture").BaseTexture.Width / 2f,
                GameSettings.DefaultInstance.ResolutionHeight/3f - TextureManager.GetTexture("BarTexture").BaseTexture.Height / 2f);

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
        /// Unload the necessary elements of the loading game screen.
        /// </summary>
        public override void Unload()
        {
            base.Unload();
        }

        #endregion

        #region HandleInput Method

        /// <summary>
        /// Handle the input of the user relative to the <c>LoadingGame</c> screen.
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

            if (_resources.Count != 0)
            {
                string resource = _resources[0];
                string[] information = resource.Split('_');
                if (information[1] == "Texture")
                {
                    if (_create)
                    {
                        ThreadStart threadStarter = delegate
                        {
                            TextureManager.AddTexture(information[0], new GameTexture("Content/Textures/" + information[0]));
                            _numberOfLoadedResources++;
                        };

                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }
                    else
                    {
                        ThreadStart threadStarter = delegate
                        {
                            TextureManager.RemoveTexture(information[0]);
                            _numberOfLoadedResources++;
                        };

                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }

                    _loadedResources.Add(resource);
                    _resources.Remove(resource);
                }
                else if (information[1] == "Model")
                {
                    if (_create)
                    {
                        ThreadStart threadStarter = delegate
                        {
                            ModelManager.AddModel(information[0], new GameModel("Content/Models/" + information[0]));
                            _numberOfLoadedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }
                    else
                    {
                        ThreadStart threadStarter = delegate
                        {
                            ModelManager.RemoveModel(information[0]);
                            _numberOfLoadedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }

                    _loadedResources.Add(resource);
                    _resources.Remove(resource);
                }
                else if (information[1] == "Sound")
                {
                    if (_create)
                    {
                        ThreadStart threadStarter = delegate
                        {
                            SoundManager.AddSound(information[0], new GameSound("Content/Sounds/" + information[0]));
                            _numberOfLoadedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }
                    else
                    {
                        ThreadStart threadStarter = delegate
                        {
                            SoundManager.RemoveSound(information[0]);
                            _numberOfLoadedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }

                    _loadedResources.Add(resource);
                    _resources.Remove(resource);
                }
                else
                {
                    if (_create)
                    {
                        foreach (string loadedResource in _loadedResources)
                        {
                            try
                            {
                                string[] loadedInformation = loadedResource.Split('_');
                                if (loadedInformation[1] == "Model" && ModelManager.GetModel(loadedInformation[0]).Model.Meshes == null)
                                    throw new Exception();
                                else if (loadedInformation[1] == "Texture" && TextureManager.GetTexture(loadedInformation[0]).BaseTexture.IsDisposed)
                                    throw new Exception();
                                else if (loadedInformation[1] == "Sound" && SoundManager.GetSound(loadedInformation[0]).GameSoundEffect.IsDisposed)
                                    throw new Exception();
                            }
                            catch (Exception e)
                            {
                                return;
                            }
                        }

                        ThreadStart threadStarter = delegate
                        {
                            LevelManager.AddLevel(resource, new Scene.SceneRenderer(
                                FileHelper.readMapInformation(resource)));
                            _numberOfLoadedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }
                    else
                    {
                        ThreadStart threadStarter = delegate
                        {
                            LevelManager.RemoveLevel(resource);
                            _numberOfLoadedResources++;
                        };
                        Thread addResourceThread = new Thread(threadStarter);
                        addResourceThread.Start();
                    }

                    _loadedResources.Add(resource);
                    _resources.Remove(resource);
                }
            }
            
            if (_numberOfLoadedResources >= _totalResources - 1)
            {
                EngineManager.Game.IsFixedTimeStep = false;
                if (_create)
                {
                    ScreenManager.RemoveScreen("LoadingScreen");
                    ScreenManager.AddScreen("ContinueGame", new MainGameScreen(
                        "ContinueGame", _mapName, _mapInformation, _savedGame));
                }
                else if (_safeTime > totalSafeTime)
                {
                    ScreenManager.RemoveScreen("LoadingScreen");
                    EngineManager.Loader.Unload();
                    ScreenManager.RemoveScreen("LoadingScreen");
                    ScreenManager.AddScreen("Background", new BackgroundScreen("Background"));
                    ScreenManager.AddScreen("MainMenu", new MainMenuScreen("MainMenu"));
                }
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>LoadingGame</c> screen.
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
            
            double eachPercentWidth = (double)borderTexture.Width / (double)_totalResources;
            int xPosition = GameSettings.DefaultInstance.ResolutionWidth / 3;
            ScreenManager.SpriteBatch.Draw(
                TextureManager.GetTexture("DummyTexture").BaseTexture as Texture2D,
                new Rectangle((int)_positionBar.X, (int)_positionBar.Y,
                    (int)(_numberOfLoadedResources * eachPercentWidth), (int)(borderTexture.Height * 0.75f)),
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
