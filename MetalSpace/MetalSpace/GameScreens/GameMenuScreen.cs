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
    /// The <c>GameMenuScreen</c> represents the internal menu that the user can
    /// display when he/she uses the Start key. There are four possible options:
    /// continue, save game, load last saved game and exit.
    /// </summary>
    class GameMenuScreen : GameScreen
    {
        #region Fields

        /// <summary>
        /// Const string that contains the name of the background texture.
        /// </summary>
        private const string texture = "DialogBackground";

        /// <summary>
        /// Reference to the main screen of the game.
        /// </summary>
        private MainGameScreen _gameScreen;

        /// <summary>
        /// Distance between the menu entries.
        /// </summary>
        private float _menuEntriesSeparation;

        /// <summary>
        /// Seletec menu entry.
        /// </summary>
        private int _selectedEntry;

        /// <summary>
        /// List of entries in the menu.
        /// </summary>
        private List<MenuEntry> _menuEntries = new List<MenuEntry>();

        /// <summary>
        /// Size of each menu entry.
        /// </summary>
        private Vector2 _menuEntrySize;
        
        /// <summary>
        /// Initial position of the menu drawing.
        /// </summary>
        private Vector2 _initialPosition;
        
        /// <summary>
        /// Current position in the menu drawing.
        /// </summary>
        private Vector2 _currentPosition;
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>GameMenuScreen</c> class.
        /// </summary>
        /// <param name="gameScreen">Reference to the main game screen.</param>
        /// <param name="name">Name of screen.</param>
        /// <param name="menuEntriesSeparation">Separation between the entries.</param>
        public GameMenuScreen(MainGameScreen gameScreen, string name, float menuEntriesSeparation)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            Name = name;

            _gameScreen = gameScreen;

            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);

            Input.Continuous = false;

            IsPopup = true;

            _selectedEntry = 0;

            _menuEntries = new List<MenuEntry>();
            _menuEntries.Add(new MenuEntry(StringHelper.DefaultInstance.Get("menu_ingame_continue")));
            _menuEntries.Add(new MenuEntry(StringHelper.DefaultInstance.Get("menu_ingame_save")));
            _menuEntries.Add(new MenuEntry(StringHelper.DefaultInstance.Get("menu_ingame_lastsave")));
            _menuEntries.Add(new MenuEntry(StringHelper.DefaultInstance.Get("menu_ingame_exit")));

            Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth, 
                GameSettings.DefaultInstance.ResolutionHeight);

            _menuEntrySize = ScreenManager.FontEntries.MeasureString(
                StringHelper.DefaultInstance.Get("menu_ingame_lastsave"));

            _menuEntriesSeparation = _menuEntrySize.Y * 0.9f;

            _initialPosition.X = (viewportSize.X - _menuEntrySize.X) / 2;
            _initialPosition.Y = (viewportSize.Y / 2f) - ((_menuEntrySize.Y * 4f + _menuEntriesSeparation * 5f) / 2f);
            _currentPosition = _initialPosition;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load the necessary elements of the game menu screen.
        /// </summary>
        public override void Load()
        {
            base.Load();
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the necessary elements of the game menu screen.
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

            if (input.Up)
                _selectedEntry = (_selectedEntry - 1) == -1 ? _menuEntries.Count - 1 : (_selectedEntry - 1);

            if (input.Down)
                _selectedEntry = (_selectedEntry + 1) == _menuEntries.Count ? 0 : (_selectedEntry + 1);

            if (input.Action)
            {
                switch (_selectedEntry)
                {
                    case 0:
                        ScreenManager.RemoveScreen("AGameMenu");
                        break;

                    case 1:
                        ScreenManager.RemoveScreen("AGameMenu");
                        EventManager.Trigger(new EventData_SaveGame(_gameScreen.MapName, 
                            _gameScreen.MainPlayer, _gameScreen.Enemies));
                        break;

                    case 2:
                        ScreenManager.RemoveScreen("AGameMenu");

                        int index = 0;
                        string[] files = FileHelper.GetFilesInDirectory();
                        if (files.Length == 0)
                        {
                            ScreenManager.RemoveScreen("AGameMenu");
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

                    case 3:
                        ScreenManager.RemoveScreen("AGameMenu");
                        ScreenManager.AddScreen("LoadingScreen", new LoadingGame(
                            LevelManager.GetLevel(_gameScreen.MapName).MapInformation, false));
                        ScreenManager.RemoveScreen("ContinueGame");
                        
                        break;
                }
            }

            /*if (input.Select)
            {
                ScreenManager.RemoveScreen("InGameMenu");
            }*/
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the state of the screen.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        /// <param name="otherScreenHasFocus">true if other screen has the focus, false otherwise.</param>
        /// <param name="coveredByOtherScreen">true if other screen cover this screen, false otherwise.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update the transition
            float transitionOffset = (float)Math.Pow(TransitionInstant, 2);
            if (ScreenState == ScreenState.TransitionOn)
                _currentPosition.X -= transitionOffset * 256;
            else
                _currentPosition.X += transitionOffset * 512;

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

            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture(texture).BaseTexture as Texture2D, 
                new Rectangle(
                    (int)(viewportSize.X / 4f), (int)(viewportSize.Y / 4f),
                    (int)(viewportSize.X * 2f / 4f), (int)(viewportSize.Y * 2f / 4f)), 
                color);

            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected, "Menu", 
                new Vector2(_currentPosition.X + _menuEntrySize.X / 3f, _currentPosition.Y), 
                color, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);

            _currentPosition.Y += _menuEntrySize.Y + _menuEntriesSeparation;

            // Draw menu entries
            for (int i = 0; i < _menuEntries.Count; i++)
            {
                MenuEntry menuEntry = _menuEntries[i];

                bool isSelected = IsActive && (i == _selectedEntry);

                menuEntry.Draw(this, _currentPosition, isSelected, gameTime);

                _currentPosition.Y += _menuEntrySize.Y + _menuEntriesSeparation;
            }

            _currentPosition = _initialPosition;

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}