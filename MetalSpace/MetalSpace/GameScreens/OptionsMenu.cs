using System;
using System.Collections.Generic;
using System.Text;
using MetalSpace.Events;
using MetalSpace.Managers;
using MetalSpace.Interfaces;
using MetalSpace.GameScreens;
using MetalSpace.Settings;
using MetalSpace.GameComponents;
using MetalSpace.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>OptionsMenu</c> class represents the list of options that
    /// the user can modify, organised in three categories (video, sound and controls).
    /// </summary>
    public class OptionsMenu : GameScreen
    {
        #region Fields

        /// <summary>
        /// Current position of the drawing.
        /// </summary>
        private Vector2 _currentPosition;

        /// <summary>
        /// Initial position for the drawing.
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
        private float _menuEntriesSeparation;

        /// <summary>
        /// Store for the SelectedEnd property.
        /// </summary>
        private bool _selectedEnd;

        /// <summary>
        /// Store for the SelectedOption property.
        /// </summary>
        private int _selectedOption;

        /// <summary>
        /// Store for the SelectedCategory property.
        /// </summary>
        private string _selectedCategory;

        /// <summary>
        /// Store for the Categories property.
        /// </summary>
        private List<string> _categories;

        /// <summary>
        /// Store for the MenuEntries property.
        /// </summary>
        private Dictionary<string, List<IOptionEntry>> _menuEntries;

        #endregion

        #region Properties

        /// <summary>
        /// Categories property
        /// </summary>
        /// <value>
        /// List of categories in the option menu (video, sound, pad).
        /// </value>
        public List<string> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        /// <summary>
        /// MenuEntries property
        /// </summary>
        /// <value>
        /// List of entries in each option category.
        /// </value>
        public Dictionary<string, List<IOptionEntry>> MenuEntries
        {
            get { return _menuEntries; }
            set { _menuEntries = value; }
        }

        /// <summary>
        /// SelectedCategory property
        /// </summary>
        /// <value>
        /// Selected category of the options menu.
        /// </value>
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; }
        }

        /// <summary>
        /// SelectedOption property
        /// </summary>
        /// <value>
        /// Selected option in the current category.
        /// </value>
        public int SelectedOption
        {
            get { return _selectedOption; }
            set { _selectedOption = value; }
        }

        /// <summary>
        /// SelectedEnd property
        /// </summary>
        /// <value>
        /// Selected end property (accept, cancel).
        /// </value>
        public bool SelectedEnd
        {
            get { return _selectedEnd; }
            set { _selectedEnd = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>OptionsMenu</c> class.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        /// <param name="menuEntriesSeparation">Separation between the entries in the option menu.</param>
        public OptionsMenu(string name, float menuEntriesSeparation = 25f)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            Name = name;

            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);

            _selectedOption = 0;
            _selectedEnd = false;
            _selectedCategory = StringHelper.DefaultInstance.Get("menu_option_categories_video");
            
            _menuEntries = new Dictionary<string, List<IOptionEntry>>();

            // Fix the position parameters
            _menuEntriesSeparation = menuEntriesSeparation;
            _menuEntrySize = ScreenManager.FontEntries.MeasureString("Example");
            _menuCenter = new Vector2(30f, GameSettings.DefaultInstance.ResolutionHeight / 2);
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load and calculate all the necessary content of the game.
        /// </summary>
        public override void Load()
        {
            base.Load();

            // Set video category
            List<IOptionEntry> videoCategory = new List<IOptionEntry>();

            List<string> resolutions = new List<string>();
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                resolutions.Add(mode.Width + "x" + mode.Height);

            videoCategory.Add(new ListOptionMenu(
                StringHelper.DefaultInstance.Get("menu_option_category_video_resolution"),
                resolutions,
                GameSettings.DefaultInstance.WindowsResolutionWidth + "x" +
                GameSettings.DefaultInstance.WindowsResolutionHeight));

            List<string> booleanOption = new List<string>();
            booleanOption.Add(StringHelper.DefaultInstance.Get("activated"));
            booleanOption.Add(StringHelper.DefaultInstance.Get("non_activated"));
            
            videoCategory.Add(new ListOptionMenu(
                StringHelper.DefaultInstance.Get("menu_option_category_video_fullscreen"),
                booleanOption, GameSettings.DefaultInstance.FullScreen ?
                StringHelper.DefaultInstance.Get("activated") :
                StringHelper.DefaultInstance.Get("non_activated")));

            videoCategory.Add(
                new ListOptionMenu(StringHelper.DefaultInstance.Get("menu_option_category_video_vertical_syncronization"),
                                   booleanOption, GameSettings.DefaultInstance.VerticalSyncronization ?
                                   StringHelper.DefaultInstance.Get("activated") :
                                   StringHelper.DefaultInstance.Get("non_activated")));


            List<string> qualityOption = new List<string>();
            qualityOption.Add(StringHelper.DefaultInstance.Get("quality_high"));
            qualityOption.Add(StringHelper.DefaultInstance.Get("quality_low"));
            videoCategory.Add(
                new ListOptionMenu(StringHelper.DefaultInstance.Get("menu_option_category_video_quality"),
                                   qualityOption, GameSettings.DefaultInstance.HighDetail ?
                                   StringHelper.DefaultInstance.Get("quality_high") :
                                   StringHelper.DefaultInstance.Get("quality_low")));

            _menuEntries.Add(StringHelper.DefaultInstance.Get("menu_option_categories_video"), videoCategory);

            // Set sound category
            List<IOptionEntry> soundCategory = new List<IOptionEntry>();

            soundCategory.Add(new NumericOptionEntry(
                StringHelper.DefaultInstance.Get("menu_option_category_sound_music_volume"), 0, 100,
                GameSettings.DefaultInstance.MusicVolume * 100));

            soundCategory.Add(new NumericOptionEntry(
                StringHelper.DefaultInstance.Get("menu_option_category_sound_effects_volume"), 0, 100,
                GameSettings.DefaultInstance.SoundVolume * 100));

            _menuEntries.Add(StringHelper.DefaultInstance.Get("menu_option_categories_sound"), soundCategory);

            // Set control category
            List<IOptionEntry> controlCategory = new List<IOptionEntry>();
            List<string> keys = new List<string>();
            keys.Add("A"); keys.Add("B"); keys.Add("C"); keys.Add("D"); keys.Add("E"); keys.Add("F");
            keys.Add("G"); keys.Add("H"); keys.Add("I"); keys.Add("J"); keys.Add("K"); keys.Add("L");
            keys.Add("M"); keys.Add("N"); keys.Add("O"); keys.Add("P"); keys.Add("Q"); keys.Add("R");
            keys.Add("S"); keys.Add("T"); keys.Add("U"); keys.Add("V"); keys.Add("W"); keys.Add("X");
            keys.Add("Y"); keys.Add("Z"); keys.Add("ENTER"); keys.Add("BACK"); keys.Add("ESC");
            keys.Add("SHIFT"); keys.Add("SPACE");
            controlCategory.Add(new ListOptionMenu(StringHelper.DefaultInstance.Get("menu_option_category_pad_up"),
                keys, GameSettings.DefaultInstance.UpKey.ToString()));
            controlCategory.Add(new ListOptionMenu(StringHelper.DefaultInstance.Get("menu_option_category_pad_down"),
                keys, GameSettings.DefaultInstance.DownKey.ToString()));
            controlCategory.Add(new ListOptionMenu(StringHelper.DefaultInstance.Get("menu_option_category_pad_left"),
                keys, GameSettings.DefaultInstance.LeftKey.ToString()));
            controlCategory.Add(new ListOptionMenu(StringHelper.DefaultInstance.Get("menu_option_category_pad_right"),
                keys, GameSettings.DefaultInstance.RightKey.ToString()));
            controlCategory.Add(new ListOptionMenu(StringHelper.DefaultInstance.Get("menu_option_category_pad_start"),
                keys, GameSettings.DefaultInstance.StartKey.ToString()));
            controlCategory.Add(new ListOptionMenu(StringHelper.DefaultInstance.Get("menu_option_category_pad_select"),
                keys, GameSettings.DefaultInstance.SelectKey.ToString()));
            controlCategory.Add(new ListOptionMenu(StringHelper.DefaultInstance.Get("menu_option_category_pad_action"),
                keys, GameSettings.DefaultInstance.ActionKey.ToString()));
            controlCategory.Add(new ListOptionMenu(StringHelper.DefaultInstance.Get("menu_option_category_pad_jump"),
                keys, GameSettings.DefaultInstance.JumpKey.ToString()));
            controlCategory.Add(new ListOptionMenu(StringHelper.DefaultInstance.Get("menu_option_category_pad_attack"),
                keys, GameSettings.DefaultInstance.AttackKey.ToString()));
            controlCategory.Add(new ListOptionMenu(StringHelper.DefaultInstance.Get("menu_option_category_pad_sattack"),
                keys, GameSettings.DefaultInstance.SAttackKey.ToString()));

            _menuEntries.Add(StringHelper.DefaultInstance.Get("menu_option_categories_pad"), controlCategory);

            _initialPosition = new Vector2(30f, _menuCenter.Y - ((((_menuEntrySize.Y - 5) *
                _menuEntries[StringHelper.DefaultInstance.Get("menu_option_categories_pad")].Count) -
                _menuEntriesSeparation) / 2f));
            _currentPosition = _initialPosition;
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the necessary elements of the options screen.
        /// </summary>
        public override void Unload()
        {
            base.Unload();
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Handle the input of the user relative to the <c>OptionsMenu</c> screen.
        /// </summary>
        /// <param name="input">Current state of the player input.</param>
        /// <param name="gameTime">Current time of the game.</param>
        public override void HandleInput(Input input, GameTime gameTime)
        {
            if (input.Up)
            {
                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);
                _selectedOption = (_selectedOption - 1) < -1 ?
                    _menuEntries[_selectedCategory].Count : _selectedOption - 1;
            }

            if (input.Down)
            {
                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);
                _selectedOption = (_selectedOption + 1) > _menuEntries[_selectedCategory].Count ?
                    -1 : _selectedOption + 1;
            }

            if (input.Left)
            {
                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);

                if (_selectedOption == -1)
                {
                    if (_selectedCategory == StringHelper.DefaultInstance.Get("menu_option_categories_sound"))
                        _selectedCategory = StringHelper.DefaultInstance.Get("menu_option_categories_video");
                    else if (_selectedCategory == StringHelper.DefaultInstance.Get("menu_option_categories_pad"))
                        _selectedCategory = StringHelper.DefaultInstance.Get("menu_option_categories_sound");
                }
                else if (_selectedOption == _menuEntries[_selectedCategory].Count)
                    _selectedEnd = !_selectedEnd;
                else
                    OnSelectEntry(_selectedOption, 0);
            }

            if (input.Right)
            {
                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);

                if (_selectedOption == -1)
                {
                    if (_selectedCategory == StringHelper.DefaultInstance.Get("menu_option_categories_video"))
                        _selectedCategory = StringHelper.DefaultInstance.Get("menu_option_categories_sound");
                    else if (_selectedCategory == StringHelper.DefaultInstance.Get("menu_option_categories_sound"))
                        _selectedCategory = StringHelper.DefaultInstance.Get("menu_option_categories_pad");
                }
                else if (_selectedOption == _menuEntries[_selectedCategory].Count)
                    _selectedEnd = !_selectedEnd;
                else
                    OnSelectEntry(_selectedOption, 1);
            }
            
            if (input.Start || input.Action)
            {
                if (_selectedOption == _menuEntries[_selectedCategory].Count)
                {
                    SoundManager.GetSound("MenuAccept").Volume = GameSettings.DefaultInstance.SoundVolume;
                    SoundManager.GetSound("MenuAccept").Play(true);

                    if (!_selectedEnd)
                        GameSettings.Save();
                    else
                        GameSettings.Load();

                    ScreenManager.RemoveScreen("OptionsMenu");
                }
            }
        }

        protected virtual void OnSelectEntry(int optionIndex, int direction)
        {
            _menuEntries[_selectedCategory][optionIndex].OnSelectEntry(direction);
        }

        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        protected void OnCancel(Object sender, EventArgs e)
        {
            OnCancel();
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
            for(int i=0; i < _menuEntries[_selectedCategory].Count; i++)
            {
                bool isSelected = IsActive && i == _selectedOption;
                _menuEntries[_selectedCategory][i].Update(this, isSelected, gameTime);
            }

            // Check video changes in the settings
            List<IOptionEntry> videoCategory = _menuEntries[
                StringHelper.DefaultInstance.Get("menu_option_categories_video")];

            if (((ListOptionMenu)videoCategory[0]).CurrentValue !=
                GameSettings.DefaultInstance.WindowsResolutionWidth + "x" +
                GameSettings.DefaultInstance.WindowsResolutionHeight)
            {
                GameSettings.DefaultInstance.WindowsResolutionWidth =
                    Convert.ToInt32(((ListOptionMenu)videoCategory[0]).CurrentValue.Split('x')[0]);
                GameSettings.DefaultInstance.WindowsResolutionHeight =
                    Convert.ToInt32(((ListOptionMenu)videoCategory[0]).CurrentValue.Split('x')[1]);
            }

            if (((ListOptionMenu)videoCategory[1]).CurrentValue !=
                (GameSettings.DefaultInstance.FullScreen ? 
                StringHelper.DefaultInstance.Get("activated") :
                StringHelper.DefaultInstance.Get("non_activated") ) )
            {
                GameSettings.DefaultInstance.FullScreen =
                    ((ListOptionMenu)videoCategory[1]).CurrentValue == 
                    StringHelper.DefaultInstance.Get("activated") ?
                    true : false;
            }

            if (((ListOptionMenu)videoCategory[2]).CurrentValue !=
                (GameSettings.DefaultInstance.VerticalSyncronization ?
                StringHelper.DefaultInstance.Get("activated") :
                StringHelper.DefaultInstance.Get("non_activated")))
            {
                GameSettings.DefaultInstance.VerticalSyncronization =
                    ((ListOptionMenu)videoCategory[2]).CurrentValue ==
                    StringHelper.DefaultInstance.Get("activated") ?
                    true : false;
            }

            if (((ListOptionMenu)videoCategory[3]).CurrentValue !=
                (GameSettings.DefaultInstance.HighDetail ?
                StringHelper.DefaultInstance.Get("quality_high") :
                StringHelper.DefaultInstance.Get("quality_low")))
            {
                GameSettings.DefaultInstance.HighDetail =
                    ((ListOptionMenu)videoCategory[3]).CurrentValue ==
                    StringHelper.DefaultInstance.Get("quality_high") ?
                    true : false;
            }

            // Check sound changes in the settings
            List<IOptionEntry> soundCategory = _menuEntries[
                StringHelper.DefaultInstance.Get("menu_option_categories_sound")];

            if (((NumericOptionEntry)soundCategory[0]).CurrentValue / 100f !=
                GameSettings.DefaultInstance.MusicVolume)
            {
                GameSettings.DefaultInstance.MusicVolume =
                    ((NumericOptionEntry)soundCategory[0]).CurrentValue / 100f;
            }

            if (((NumericOptionEntry)soundCategory[1]).CurrentValue / 100f !=
                GameSettings.DefaultInstance.SoundVolume)
            {
                GameSettings.DefaultInstance.SoundVolume =
                    ((NumericOptionEntry)soundCategory[1]).CurrentValue / 100f;
            }

            // Check pad changes in the settings
            List<IOptionEntry> padCategory = _menuEntries[
                StringHelper.DefaultInstance.Get("menu_option_categories_pad")];

            if (((ListOptionMenu)padCategory[0]).CurrentValue !=
                GameSettings.DefaultInstance.UpKey.ToString())
            {
                GameSettings.DefaultInstance.UpKey = (Keys)
                    Enum.Parse(typeof(Keys), ((ListOptionMenu)padCategory[0]).CurrentValue);
            }

            if (((ListOptionMenu)padCategory[1]).CurrentValue !=
                GameSettings.DefaultInstance.DownKey.ToString())
            {
                GameSettings.DefaultInstance.DownKey = (Keys)
                    Enum.Parse(typeof(Keys), ((ListOptionMenu)padCategory[1]).CurrentValue);
            }

            if (((ListOptionMenu)padCategory[2]).CurrentValue !=
                GameSettings.DefaultInstance.LeftKey.ToString())
            {
                GameSettings.DefaultInstance.LeftKey = (Keys)
                    Enum.Parse(typeof(Keys), ((ListOptionMenu)padCategory[2]).CurrentValue);
            }

            if (((ListOptionMenu)padCategory[3]).CurrentValue !=
                GameSettings.DefaultInstance.RightKey.ToString())
            {
                GameSettings.DefaultInstance.RightKey = (Keys)
                    Enum.Parse(typeof(Keys), ((ListOptionMenu)padCategory[3]).CurrentValue);
            }

            if (((ListOptionMenu)padCategory[4]).CurrentValue !=
                GameSettings.DefaultInstance.StartKey.ToString())
            {
                GameSettings.DefaultInstance.StartKey = (Keys)
                    Enum.Parse(typeof(Keys), ((ListOptionMenu)padCategory[4]).CurrentValue);
            }

            if (((ListOptionMenu)padCategory[5]).CurrentValue !=
                GameSettings.DefaultInstance.SelectKey.ToString())
            {
                GameSettings.DefaultInstance.SelectKey = (Keys)
                    Enum.Parse(typeof(Keys), ((ListOptionMenu)padCategory[5]).CurrentValue);
            }

            if (((ListOptionMenu)padCategory[6]).CurrentValue !=
                GameSettings.DefaultInstance.ActionKey.ToString())
            {
                GameSettings.DefaultInstance.ActionKey = (Keys) 
                    Enum.Parse(typeof(Keys), ((ListOptionMenu)padCategory[6]).CurrentValue);
            }

            if (((ListOptionMenu)padCategory[7]).CurrentValue !=
                GameSettings.DefaultInstance.JumpKey.ToString())
            {
                GameSettings.DefaultInstance.JumpKey = (Keys)
                    Enum.Parse(typeof(Keys), ((ListOptionMenu)padCategory[7]).CurrentValue);
            }

            if (((ListOptionMenu)padCategory[8]).CurrentValue !=
                GameSettings.DefaultInstance.AttackKey.ToString())
            {
                GameSettings.DefaultInstance.AttackKey = (Keys)
                    Enum.Parse(typeof(Keys), ((ListOptionMenu)padCategory[8]).CurrentValue);
            }

            if (((ListOptionMenu)padCategory[9]).CurrentValue !=
                GameSettings.DefaultInstance.SAttackKey.ToString())
            {
                GameSettings.DefaultInstance.SAttackKey = (Keys)
                    Enum.Parse(typeof(Keys), ((ListOptionMenu)padCategory[9]).CurrentValue);
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>OptionsMenu</c> screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            if (!TextureManager.GetTexture("DummyTexture").ReadyToRender ||
                !TextureManager.GetTexture("DummyTexture15T").ReadyToRender ||
                !TextureManager.GetTexture("BarTexture").ReadyToRender)
                return;

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, CanvasManager.ScaleMatrix);
            
            // Draw the categories
            float xPosition = _currentPosition.X;
            foreach (string category in _menuEntries.Keys)
            {
                if(_selectedOption == -1 && category == _selectedCategory)
                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                        new Rectangle((int)xPosition, (int)_currentPosition.Y,
                                      (int)(((GameSettings.DefaultInstance.ResolutionWidth / 2) - 30f) / 3), (int)_menuEntrySize.Y),
                        Color.LightBlue);

                ScreenManager.SpriteBatch.DrawString(
                    _selectedCategory == category ? ScreenManager.FontEntriesSelected : ScreenManager.FontEntries, 
                    category, new Vector2(xPosition, _currentPosition.Y), 
                    _selectedCategory == category ? Color.White : Color.SlateGray);
                
                xPosition += (((GameSettings.DefaultInstance.ResolutionWidth / 2) - 30f) / 3);
            }
            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SeparationBar").BaseTexture,
                new Rectangle((int)_currentPosition.X, (int)(_currentPosition.Y + _menuEntrySize.Y),
                    (int)((GameSettings.DefaultInstance.ResolutionWidth / 2)), 3), Color.White);

            _currentPosition.Y += _menuEntrySize.Y + _menuEntriesSeparation;

            // Draw menu entries
            for (int i = 0; i < _menuEntries[_selectedCategory].Count; i++)
            {
                IOptionEntry menuEntry = _menuEntries[_selectedCategory][i];

                bool isSelected = IsActive && (i == _selectedOption);

                // Draw the background of the menu entry
                if (isSelected)
                    ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                        new Rectangle((int)_currentPosition.X, (int)_currentPosition.Y,
                            (int)(GameSettings.DefaultInstance.ResolutionWidth / 2), (int)_menuEntrySize.Y),
                        Color.LightBlue);

                // Draw the menu entry
                menuEntry.Draw(this, _currentPosition, isSelected, gameTime);

                if (_selectedCategory == StringHelper.DefaultInstance.Get("menu_option_categories_pad"))
                {
                    if (i == _menuEntries[_selectedCategory].Count - 1)
                        _currentPosition.Y += _menuEntrySize.Y + _menuEntriesSeparation;
                    else
                        _currentPosition.Y += _menuEntrySize.Y - 5f;
                }
                else
                    _currentPosition.Y += _menuEntrySize.Y + _menuEntriesSeparation;
            }
            _currentPosition.Y = 650f;
            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("SeparationBar").BaseTexture,
                new Rectangle((int)_currentPosition.X, (int)(_currentPosition.Y - _menuEntriesSeparation),
                    (int)((GameSettings.DefaultInstance.ResolutionWidth / 2)), 3), Color.White);

            // Draw accept and cancel buttons
            xPosition = _currentPosition.X;
            if (_selectedOption == _menuEntries[_selectedCategory].Count && !_selectedEnd)
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle((int)xPosition, (int)_currentPosition.Y,
                                  (int)(GameSettings.DefaultInstance.ResolutionWidth / 4), (int)_menuEntrySize.Y),
                    Color.LightBlue);

            ScreenManager.SpriteBatch.DrawString(
                !_selectedEnd ? ScreenManager.FontEntriesSelected : ScreenManager.FontEntries, 
                StringHelper.DefaultInstance.Get("accept"),
                new Vector2(xPosition, _currentPosition.Y), !_selectedEnd ? Color.White : Color.SlateGray);
            
            xPosition = GameSettings.DefaultInstance.ResolutionWidth / 3;
            if (_selectedOption == _menuEntries[_selectedCategory].Count && _selectedEnd)
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle((int)xPosition, (int)_currentPosition.Y,
                                  (int)(GameSettings.DefaultInstance.ResolutionWidth / 4), (int)_menuEntrySize.Y),
                    Color.LightBlue);

            ScreenManager.SpriteBatch.DrawString(
                _selectedEnd ? ScreenManager.FontEntriesSelected : ScreenManager.FontEntries,
                StringHelper.DefaultInstance.Get("cancel"),
                new Vector2(xPosition, _currentPosition.Y), _selectedEnd ? Color.White : Color.SlateGray);

            ScreenManager.SpriteBatch.End();

            _currentPosition = _initialPosition;
        }

        #endregion
    }
}
