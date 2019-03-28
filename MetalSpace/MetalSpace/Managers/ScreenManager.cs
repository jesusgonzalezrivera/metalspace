using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Textures;
using MetalSpace.GameScreens;
using MetalSpace.Events;

namespace MetalSpace.Managers
{
    /// <summary>
    /// The <c>ScreenManager</c> class controls the state of the different screens
    /// that the user can display in each moment.
    /// </summary>
    class ScreenManager : DrawableGameComponent
    {
        #region Fields

        /// <summary>
        /// List of active screens.
        /// </summary>
        private static List<KeyValuePair<string, GameScreen>> _screens =
            new List<KeyValuePair<string, GameScreen>>();

        /// <summary>
        /// List of screens of update.
        /// </summary>
        private static List<GameScreen> _screensToUpdate = 
            new List<GameScreen>();

        /// <summary>
        /// Store for the Initialized property.
        /// </summary>
        private static bool _initialized = false;

        /// <summary>
        /// Store for the SpriteBatch property.
        /// </summary>
        private static SpriteBatch _spriteBatch;

        /// <summary>
        /// Store for the FontEntries property.
        /// </summary>
        private static SpriteFont _fontEntries;

        /// <summary>
        /// Store for the FontEntriesSelected property.
        /// </summary>
        private static SpriteFont _fontEntriesSelected;

        /// <summary>
        /// Store for the FontTitle property.
        /// </summary>
        private static SpriteFont _fontTitle;

        #endregion

        #region Properties

        /// <summary>
        /// Initialized property
        /// </summary>
        /// <value>
        /// true if the <c>ScreenManager</c> was initialized, false otherwise.
        /// </value>
        public static bool Initialized
        {
            get { return _initialized; }
        }

        /// <summary>
        /// SpriteBatch property
        /// </summary>
        /// <value>
        /// SpriteBatch shared by all screens and used to draw text.
        /// </value>
        public static SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        /// <summary>
        /// FontEntries property
        /// </summary>
        /// <value>
        /// Font used to draw normal entries.
        /// </value>
        public static SpriteFont FontEntries
        {
            get { return _fontEntries; }
        }

        /// <summary>
        /// FontEntriesSelected property
        /// </summary>
        /// <value>
        /// Font used to draw selected entries.
        /// </value>
        public static SpriteFont FontEntriesSelected
        {
            get { return _fontEntriesSelected; }
        }

        /// <summary>
        /// FontTitle property
        /// </summary>
        /// <value>
        /// Font used to draw titles.
        /// </value>
        public static SpriteFont FontTitle
        {
            get { return _fontTitle; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>ScreenManager</c> class.
        /// </summary>
        /// <param name="game">Reference to the main game.</param>
        public ScreenManager(Game game) : base(game)
        {
            Enabled = true;
        }

        #endregion

        #region Initialize Method

        /// <summary>
        /// Initialize the content of the <c>ScreenManager</c>.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _initialized = true;
        }

        #endregion

        #region LoadContent Method

        /// <summary>
        /// Load the necessary content of the <c>ScreenManager</c>.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(EngineManager.GameGraphicsDevice);
            _fontTitle = EngineManager.ContentManager.Load<SpriteFont>("Content/Fonts/FontTitle");
            _fontTitle.Spacing -= 18;
            _fontEntries = EngineManager.ContentManager.Load<SpriteFont>("Content/Fonts/FontNormal");
            _fontEntriesSelected = EngineManager.ContentManager.Load<SpriteFont>("Content/Fonts/FontNormalSelected");
            _fontEntriesSelected.Spacing -= 13;

            foreach (KeyValuePair<string,GameScreen> screen in _screens)
                screen.Value.Load();
        }

        #endregion 

        #region UnloadContent Method

        /// <summary>
        /// Unload the content used in the <c>ScreenManager</c>.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
            
            foreach (KeyValuePair<string, GameScreen> screen in _screens)
                screen.Value.Unload();
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the state of the screens added to the <c>ScreenManager</c>.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard input
            EngineManager.Input.Update();

            // Make a copy of the screen list to avoid confusion if the
            // process of updating one screen adds or removes others
            _screensToUpdate.Clear();

            foreach (KeyValuePair<string, GameScreen> screen in _screens)
                _screensToUpdate.Add(screen.Value);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;
            
            // Loop over the screens waiting to be updated
            while (_screensToUpdate.Count > 0)
            {
                // Pop the last screen in the list
                GameScreen screen = _screensToUpdate[_screensToUpdate.Count - 1];
                
                _screensToUpdate.RemoveAt(_screensToUpdate.Count - 1);

                // Update the screen
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
                
                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first screen we came across, give it a
                    // chance to handle input
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(EngineManager.Input, gameTime);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screen that they are covered by it
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the screens that are visible to the user.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            CanvasManager.BeginDraw();

            foreach (KeyValuePair<string, GameScreen> screen in _screens)
            {
                if (screen.Value.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Value.Draw(gameTime);
                screen.Value.PosUIDraw(gameTime);
            }
        }

        #endregion

        #region AddScreen Method

        /// <summary>
        /// Add a new screen to the <c>ScreenManager</c>.
        /// </summary>
        /// <param name="name">Name of the screen to be added.</param>
        /// <param name="screen">Reference to the screen to be added.</param>
        public static void AddScreen(string name, GameScreen screen)
        {
            _screens.Add(new KeyValuePair<string, GameScreen>(name, screen));
            if (_initialized)
                screen.Load();
        }

        #endregion

        #region RemoveScreen Method

        /// <summary>
        /// Remove an existing screen from the <c>ScreenManager</c>.
        /// </summary>
        /// <param name="name">Name of the screen to be removed.</param>
        public static void RemoveScreen(string name)
        {
            foreach (KeyValuePair<string, GameScreen> screen in _screens)
                if (screen.Key == name)
                {
                    if (_initialized)
                        screen.Value.Unload();

                    _screensToUpdate.Remove(screen.Value);
                    _screens.Remove(screen);

                    return;
                }
        }

        #endregion

        #region GetScreen Method

        /// <summary>
        /// Get the screen with the specified name.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        /// <returns>Screen with the specified name.</returns>
        public static GameScreen GetScreen(string name)
        {
            foreach (KeyValuePair<string, GameScreen> screen in _screens)
                if (screen.Key == name)
                    return screen.Value;

            return null;
        }

        #endregion
    }
}