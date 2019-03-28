using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Settings;
using MetalSpace.GameComponents;
using MetalSpace.Textures;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>MenuScreen</c> class is an abstract class that represents
    /// a basic menu for the user. By this way, the user only have extend
    /// this class and select the entries and implement the behaviour of
    /// each entry.
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        #region Fields

        /// <summary>
        /// Current position of the menu entries drawing.
        /// </summary>
        private Vector2 _currentPosition;

        /// <summary>
        /// Initial position of the menu entries.
        /// </summary>
        private Vector2 _initialPosition;

        /// <summary>
        /// Center of the menu.
        /// </summary>
        private Vector2 _menuCenter;

        /// <summary>
        /// Size of each menu entry.
        /// </summary>
        private Vector2 _menuEntrySize;

        /// <summary>
        /// Separation between the menu entries.
        /// </summary>
        private float   _menuEntriesSeparation;

        /// <summary>
        /// Store for the SelectedEntry property.
        /// </summary>
        private int _selectedEntry = 0;

        /// <summary>
        /// Store for the MenuEntries property.
        /// </summary>
        List<MenuEntry> _menuEntries = new List<MenuEntry>();
        
        #endregion

        #region Properties

        /// <summary>
        /// SelectedEntry property
        /// </summary>
        /// <value>
        /// Selected menu entry.
        /// </value>
        public int SelectedEntry
        {
            get { return _selectedEntry; }
            set { _selectedEntry = value; }
        }

        /// <summary>
        /// MenuEntries property
        /// </summary>
        /// <value>
        /// List of menu entries that appears on the screen.
        /// </value>
        protected List<MenuEntry> MenuEntries
        {
            get { return _menuEntries; }
            set { _menuEntries = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>MenuScreen</c> class.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        /// <param name="menuTitle">Title of the menu.</param>
        public MenuScreen(string name, string menuTitle)
            : this(name, menuTitle, 5, 25f)
        {

        }

        /// <summary>
        /// Constructor of the <c>MenuScreen</c> class.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        /// <param name="menuTitle">Title of the menu.</param>
        /// <param name="menuEntries">Number of menu entries.</param>
        /// <param name="menuEntriesSeparation">Separation between the menu entries.</param>
        public MenuScreen(string name, string menuTitle, int menuEntries, float menuEntriesSeparation)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            Name = name;

            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            
            _menuEntriesSeparation = menuEntriesSeparation;
            _menuEntrySize = ScreenManager.FontEntries.MeasureString("Example");
            _menuCenter = new Vector2(50f, GameSettings.DefaultInstance.ResolutionHeight / 2);
            _initialPosition = new Vector2(50f, _menuCenter.Y - ((((_menuEntriesSeparation + _menuEntrySize.Y) *
                menuEntries) - _menuEntriesSeparation) / 2f));
            _currentPosition = _initialPosition;
        }

        #endregion

        #region HandleInput Method

        /// <summary>
        /// Handle the input of the user relative to the <c>MenuScreen</c> screen.
        /// </summary>
        /// <param name="input">Current state of the player input.</param>
        /// <param name="gameTime">Current time of the game.</param>
        public override void HandleInput(Input input, GameTime gameTime)
        {
            if (input.Up)
            {
                _selectedEntry = (_selectedEntry - 1) < 0 ?
                    _menuEntries.Count - 1 : _selectedEntry - 1;

                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);
            }

            if (input.Down)
            {
                _selectedEntry = (_selectedEntry + 1) % _menuEntries.Count;

                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);
            }

            if (input.Start || input.Action)
            {
                SoundManager.GetSound("MenuAccept").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuAccept").Play(true);
                OnSelectEntry(_selectedEntry);
            }
            else if (input.Select)
                OnCancel();
        }

        #endregion

        /// <summary>
        /// Action to do when the entry is selected.
        /// </summary>
        /// <param name="entryIndex"></param>
        protected virtual void OnSelectEntry(int entryIndex)
        {
            _menuEntries[_selectedEntry].OnSelectEntry();
        }

        /// <summary>
        /// Action todo when whe want to exit from the screen.
        /// </summary>
        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        protected void OnCancel(Object sender, EventArgs e)
        {
            OnCancel();
        }

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
        /// Unload the necessary elements of the loading game screen.
        /// </summary>
        public override void Unload()
        {
            base.Unload();
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
        /// Draw the current state of the <c>LoadingGame</c> screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, CanvasManager.ScaleMatrix);

            // Draw menu entries
            for (int i=0; i < _menuEntries.Count; i++)
            {
                MenuEntry menuEntry = _menuEntries[i];

                bool isSelected = IsActive && (i == _selectedEntry);

                menuEntry.Draw(this, _currentPosition, isSelected, gameTime);

                _currentPosition.Y += _menuEntrySize.Y + _menuEntriesSeparation;
            }

            _currentPosition = _initialPosition;

            ScreenManager.SpriteBatch.End();
        }

        #endregion
    }
}
