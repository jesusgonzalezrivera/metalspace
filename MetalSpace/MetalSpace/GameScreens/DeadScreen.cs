using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Events;
using MetalSpace.Textures;
using MetalSpace.Managers;
using MetalSpace.Settings;
using MetalSpace.GameComponents;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>DeadScreen</c> class represents the screen that appears when
    /// the player dies. If the user want to continue, the last saved game is
    /// loaded. Else the user goes to the main menu screen.
    /// </summary>
    class DeadScreen : GameScreen
    {
        #region Fields

        /// <summary>
        /// Const string that contains the name of the background texture.
        /// </summary>
        private const string texture = "DeadBackground";

        /// <summary>
        /// Reference of the game screen where the user interacts.
        /// </summary>
        private MainGameScreen _gameScreen;

        /// <summary>
        /// Seletec entry in the <c>DeadScreen</c> screen.
        /// </summary>
        private int _selectedEntry;

        /// <summary>
        /// List of entries used in the <c>DeadScreen</c> (continue and exit).
        /// </summary>
        private List<MenuEntry> _menuEntries = new List<MenuEntry>();

        /// <summary>
        /// Size of the title.
        /// </summary>
        private Vector2 _titleSize;

        /// <summary>
        /// Y Size of the entry menu.
        /// </summary>
        private Vector2 _menuEntrySizeY;

        /// <summary>
        /// X Size of the entry menu.
        /// </summary>
        private Vector2 _menuEntrySizeN;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>DeadScreen</c> class.
        /// </summary>
        /// <param name="gameScreen">Reference to the screen where the user interacts.</param>
        /// <param name="name">Name of the screen.</param>
        public DeadScreen(MainGameScreen gameScreen, string name)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            Name = name;

            _gameScreen = gameScreen;

            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);

            Input.Continuous = false;

            IsPopup = true;

            _selectedEntry = 0;

            _titleSize = ScreenManager.FontEntries.MeasureString(
                StringHelper.DefaultInstance.Get("dead_screen_message"));
            _menuEntrySizeY = ScreenManager.FontEntries.MeasureString(
                StringHelper.DefaultInstance.Get("dead_screen_yes"));
            _menuEntrySizeN = ScreenManager.FontEntries.MeasureString(
                StringHelper.DefaultInstance.Get("dead_screen_no"));
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load and calculate all the necessary content of the game.
        /// </summary>
        public override void Load()
        {
            base.Load();
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the necessary elements of the background
        /// </summary>
        public override void Unload()
        {
            base.Unload();

            Input.Continuous = true;
        }

        #endregion

        #region Handle Input Method

        /// <summary>
        /// Handle the input of the user relative to the <c>DeadScreen</c> screen.
        /// </summary>
        /// <param name="input">Current state of the player input.</param>
        /// <param name="gameTime">Current time of the game.</param>
        public override void HandleInput(GameComponents.Input input, GameTime gameTime)
        {
            base.HandleInput(input, gameTime);

            if (input.Left)
                _selectedEntry = (_selectedEntry - 1) == -1 ? 0 : (_selectedEntry - 1);

            if (input.Right)
                _selectedEntry = (_selectedEntry + 1) == 2 ? 1 : (_selectedEntry + 1);

            if (input.Action)
            {
                switch (_selectedEntry)
                {
                    case 0:
                        ScreenManager.RemoveScreen("DeadScreen");
                        int index = 0;
                        string[] files = FileHelper.GetFilesInDirectory();
                        if (files.Length == 0)
                        {
                            ScreenManager.RemoveScreen("DeadScreen");
                        }
                        else
                        {
                            bool finish = false;
                            for (int i = 0; i < files.Length && !finish; i++)
                            {
                                if (files[i].Contains("GameSetting"))
                                    continue;

                                string extension = files[i].Split(' ')[2].Split('.')[1];
                                if (extension != "png")
                                {
                                    index = i;
                                    finish = true;
                                }
                            }

                            for (int i = 1; i < files.Length; i++)
                            {
                                if (files[i].Contains("GameSetting"))
                                    continue;

                                string extension = files[i].Split(' ')[2].Split('.')[1];
                                if (extension != "png")
                                {
                                    int date = Convert.ToInt32(files[i].Split(' ')[1]);
                                    int hour = Convert.ToInt32(files[i].Split(' ')[2].Split('.')[0]);
                                    if (date > Convert.ToInt32(files[index].Split(' ')[1]))
                                        index = i;
                                    else if (date == Convert.ToInt32(files[index].Split(' ')[1]) &&
                                        hour > Convert.ToInt32(files[index].Split(' ')[2].Split('.')[0]))
                                        index = i;
                                }
                            }

                            EventManager.Trigger(new EventData_LoadSavedGame(files[index]));
                        }
                        break;
                    case 1:
                        ScreenManager.RemoveScreen("DeadScreen");
                        ScreenManager.AddScreen("LoadingScreen", new LoadingGame(
                            LevelManager.GetLevel(_gameScreen.MapName).MapInformation, false));
                        ScreenManager.RemoveScreen("ContinueGame");
                        break;
                }
            }
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

            // Update each menu entry
            for (int i = 0; i < _menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == _selectedEntry);
                _menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>DeadScreen</c> screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            // Fade the popup alpha during transitions.
            Color color = new Color(255, 255, 255, TransitionAlpha);

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, CanvasManager.ScaleMatrix);

            // Draw the background rectangle.
            Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth,
                GameSettings.DefaultInstance.ResolutionHeight);

            Rectangle backgroundRectangle = new Rectangle(
                0, 0, (int)viewportSize.X, (int)viewportSize.Y);

            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture(texture).BaseTexture as Texture2D, 
                backgroundRectangle, color);

            // Draw the strings
            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                StringHelper.DefaultInstance.Get("dead_screen_message"),
                new Vector2((viewportSize.X / 2f) - (_titleSize.X / 2f),
                    (viewportSize.Y / 3f) - (_titleSize.Y / 2f)),
                color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                StringHelper.DefaultInstance.Get("dead_screen_yes"),
                new Vector2(viewportSize.X * 2f / 6f, viewportSize.Y / 2f),
                _selectedEntry == 0 ? Color.White : Color.SlateGray, 
                0, Vector2.Zero, 1, SpriteEffects.None, 0);

            if (_selectedEntry == 0)
            {
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle((int)(viewportSize.X * 2f / 6f), (int)(viewportSize.Y / 2f),
                        (int)(_menuEntrySizeY.X + 10), (int)(_menuEntrySizeY.Y)),
                    Color.White);
            }

            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                StringHelper.DefaultInstance.Get("dead_screen_no"),
                new Vector2(viewportSize.X * 4f / 6f, viewportSize.Y / 2f),
                _selectedEntry == 1 ? Color.White : Color.SlateGray, 
                0, Vector2.Zero, 1, SpriteEffects.None, 0);

            if (_selectedEntry == 1)
            {
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle((int)(viewportSize.X * 4f / 6f), (int)(viewportSize.Y / 2f),
                        (int)(_menuEntrySizeN.X + 10), (int)(_menuEntrySizeN.Y)),
                    Color.White);
            }

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
