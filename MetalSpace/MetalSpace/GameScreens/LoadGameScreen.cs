using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Events;
using MetalSpace.Managers;
using MetalSpace.Textures;
using MetalSpace.GameScreens;
using MetalSpace.GameComponents;
using MetalSpace.Settings;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>SavedGame</c> class represents the necessary elements that
    /// represent a saved game.
    /// </summary>
    [Serializable]
    public class SavedGame
    {
        public string Date;
        public string MapName;
        public float Percent;
        public Texture2D CapturedImage;
        public string PlayerName;
        public Vector3 PlayerPosition, PlayerRotation, PlayerScale;
        public Vector2 PlayerMaxSpeed;
        public int PlayerCurrentLife, PlayerMaxLife;
        public int Debolio, Aerogel, Fulereno;
        public int TotalPoints;

        public int PlayerNumberOfObjects;
        public List<string> PlayerObjectType;
        public List<string> PlayerObjectName;
        public List<string> PlayerObjectTextureName;
        public List<int> PlayerObjectPosition;
        public List<bool> PlayerObjectEquipped;
        public List<int> PlayerObjectWeaponCurrentAmmo;
        public List<int> PlayerObjectWeaponTotalAmmo;
        public List<int> PlayerObjectWeaponPower;
        public List<Objects.Gun.ShotType> PlayerObjectWeaponType;
        public List<int> PlayerObjectArmatureSkill;
        public List<int> PlayerObjectArmatureDefense;
        public List<Objects.Armature.ArmatureType> PlayerObjectArmatureType;
        public List<int> PlayerObjectAmmoAmount;
        public List<Objects.Gun.ShotType> PlayerObjectAmmoType;

        public int NumberOfEnemies;
        public List<string> EnemyName;
        public List<Vector3> EnemyPosition;
        public List<Vector3> EnemyRotation;
        public List<Vector3> EnemyScale;
        public List<Vector2> EnemyMaxSpeed;
        public List<int> EnemyLife, EnemyMaxLife;
        public List<int> EnemyAttack;
    };

    /// <summary>
    /// The <c>LoadGameScreen</c> class represent the list of all saved
    /// games that the user has saved.
    /// </summary>
    class LoadGameScreen : GameScreen
    {
        #region Fields

        /// <summary>
        /// Current position of the drawing.
        /// </summary>
        private Vector2 _currentPosition;

        /// <summary>
        /// Initial position where start the drawing.
        /// </summary>
        private Vector2 _initialPosition;

        /// <summary>
        /// Size of each menu entry.
        /// </summary>
        private Vector2 _menuEntrySize;

        /// <summary>
        /// Center of the menu.
        /// </summary>
        private Vector2 _menuCenter;

        /// <summary>
        /// Separation of each menu entry.
        /// </summary>
        private float _menuEntriesSeparation;

        /// <summary>
        /// Store for the SavedGames property.
        /// </summary>
        private List<SavedGameEntry> _savedGames;

        /// <summary>
        /// Last page to be selected for the user.
        /// </summary>
        private int _lastPage;

        /// <summary>
        /// Store for the SelectedPage property.
        /// </summary>
        private int _selectedPage;

        /// <summary>
        /// Store for the SelectedGame property.
        /// </summary>
        private int _selectedGame;

        /// <summary>
        /// Store for the SelectedEnd property.
        /// </summary>
        private bool _selectedEnd;

        #endregion

        #region Properties

        /// <summary>
        /// SavedGames property
        /// </summary>
        /// <value>
        /// List of saved saved games.
        /// </value>
        public List<SavedGameEntry> SavedGames
        {
            get { return _savedGames; }
            set { _savedGames = value; }
        }

        /// <summary>
        /// SelectedPage property
        /// </summary>
        /// <value>
        /// Current selected page of screen.
        /// </value>
        public int SelectedPage
        {
            get { return _selectedPage; }
            set { _selectedPage = value; }
        }

        /// <summary>
        /// SelectedGame property
        /// </summary>
        /// <value>
        /// Current selected saved game of screen.
        /// </value>
        public int SelectedGame
        {
            get { return _selectedGame; }
            set { _selectedGame = value; }
        }

        /// <summary>
        /// SelectedEnd property
        /// </summary>
        /// <value>
        /// Current selected end exit option of screen.
        /// </value>
        public bool SelectedEnd
        {
            get { return _selectedEnd; }
            set { _selectedEnd = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of <c>LoadGameScreen</c> class.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        public LoadGameScreen(string name)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            Name = name;

            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);

            LoadGames();

            _selectedEnd = false;
            _selectedPage = 0;
            _selectedGame = 0;
            _lastPage = _savedGames.Count / 3;
            _lastPage += _savedGames.Count % 3 == 0 ? 0 : 1;

            Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth,
                GameSettings.DefaultInstance.ResolutionHeight);

            _menuEntrySize = ScreenManager.FontEntries.MeasureString("X / X");
            _menuCenter = new Vector2(viewportSize.X / 10f, viewportSize.Y / 2);

            _menuEntriesSeparation = _menuEntrySize.Y * 0.5f;

            _initialPosition = new Vector2(viewportSize.X / 10f, 
                _menuCenter.Y - ((_menuEntrySize.Y * 9) + ((5 * 4) * 3) + (_menuEntriesSeparation * 2) +
                (_menuEntrySize.Y * 2)) / 2);
            _currentPosition = _initialPosition;
        }

        #endregion

        #region LoadGames Method

        /// <summary>
        /// Load the saved games by the user.
        /// </summary>
        public void LoadGames()
        {
            int counter = 0;
            string[] files = FileHelper.GetFilesInDirectory();
            List<SavedGame> listSavedGames = new List<SavedGame>();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Contains("GameSetting"))
                    continue;

                string extension = files[i].Split(' ')[2].Split('.')[1];
                if (extension == "xml")
                {
                    listSavedGames.Add(FileHelper.ReadSavedGame(files[i]));
                    
                    Stream imageFile = FileHelper.LoadGameContentFile(files[i].Split('.')[0] + ".png");
                    listSavedGames[counter].CapturedImage = Texture2D.FromStream(GameGraphicsDevice, imageFile);
                    imageFile.Close();

                    counter++;
                }
            }

            _savedGames = new List<SavedGameEntry>();
            foreach (SavedGame savedGame in listSavedGames)
                _savedGames.Add(new SavedGameEntry(savedGame));
        }

        #endregion

        #region HandleInput

        /// <summary>
        /// Handle the input of the user relative to the <c>LoadGameScreen</c> screen.
        /// </summary>
        /// <param name="input">Current state of the player input.</param>
        /// <param name="gameTime">Current time of the game.</param>
        public override void HandleInput(Input input, GameTime gameTime)
        {
            int lastIndex = (_selectedPage == _lastPage - 1) ? _savedGames.Count % 3 : 3;
            lastIndex = lastIndex == 0 ? 3 : lastIndex;
            if (input.Up)
            {
                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);
                _selectedGame = (_selectedGame - 1) < -1 ? lastIndex : _selectedGame - 1;
            }

            if (input.Down)
            {
                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);
                _selectedGame = (_selectedGame + 1) > lastIndex ? -1 : _selectedGame + 1;
            }

            if (input.Left)
            {
                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);

                _selectedPage = _selectedPage - 1 < 0 ? _lastPage - 1 : _selectedPage - 1;
                lastIndex = (_selectedPage == _lastPage - 1) ? _savedGames.Count % 3 : 3;
                lastIndex = lastIndex == 0 ? 3 : lastIndex;
                if (_lastPage != 1 && _selectedPage == _lastPage - 1 && _selectedGame > lastIndex - 1)
                    _selectedGame = 0;
            }

            if (input.Right)
            {
                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);

                _selectedPage = _selectedPage + 1 > _lastPage - 1 ? 0 : _selectedPage + 1;
                lastIndex = (_selectedPage == _lastPage - 1) ? _savedGames.Count % 3 : 3;
                lastIndex = lastIndex == 0 ? 3 : lastIndex;
                if (_lastPage != 1 && _selectedPage == _lastPage - 1 && _selectedGame > lastIndex - 1)
                    _selectedGame = 0;
            }

            if (input.Start || input.Action)
            {
                SoundManager.GetSound("MenuAccept").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuAccept").Play(true);
                if (_selectedGame == lastIndex)
                {
                    OnCancel();
                }
                else if (_selectedGame != -1 && _selectedGame < lastIndex)
                {
                    List<string> newFiles = new List<string>();
                    List<string> files = new List<string>(FileHelper.GetFilesInDirectory());
                    for (int i = 0; i < files.Count; i++)
                    {
                        if (!files[i].Contains("GameSetting") && files[i].Split(' ')[2].Split('.')[1] != "png")
                            newFiles.Add(files[i]);
                    }

                    EventManager.Trigger(new EventData_LoadSavedGame(newFiles[_selectedPage * 3 + _selectedGame]));
                }
            }
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
        /// Unload the necessary elements of the load game screen.
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

            // Check if thre is some saved game selected
            int lastIndex = (_selectedPage == _lastPage - 1) ? _savedGames.Count % 3 : 3;
            lastIndex = lastIndex == 0 ? 3 : lastIndex;
            if (_selectedGame == -1 || _selectedGame == lastIndex)
                return;

            // Update each menu entry
            for (int i = 0; i < lastIndex; i++)
            {
                bool isSelected = IsActive && i == _selectedGame;
                _savedGames[_selectedPage * 3 + _selectedGame].Update(this, isSelected, gameTime);
            }
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the <c>LoadGameScreen</c> screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            if (!TextureManager.GetTexture("DummyTexture15T").ReadyToRender ||
                !TextureManager.GetTexture("SeparationBar").ReadyToRender)
                return;

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, CanvasManager.ScaleMatrix);

            // Draw the current pages
            ScreenManager.SpriteBatch.Draw(
                TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                new Rectangle(GameSettings.DefaultInstance.ResolutionWidth / 3, (int)_currentPosition.Y,
                              (int)_menuEntrySize.X, (int)_menuEntrySize.Y),
                _selectedGame == -1 ? Color.White : Color.Transparent);

            ScreenManager.SpriteBatch.DrawString(
                ScreenManager.FontEntriesSelected,
                _selectedPage + " / " + (_lastPage - 1),
                new Vector2(GameSettings.DefaultInstance.ResolutionWidth / 3, _currentPosition.Y),
                _selectedGame == -1 ? Color.White : Color.SlateGray);

            _currentPosition.Y += _menuEntrySize.Y + _menuEntriesSeparation;

            int lastIndex = (_selectedPage == _lastPage - 1) ? _savedGames.Count % 3 : 3;
            lastIndex = lastIndex == 0 ? 3 : lastIndex;

            // Draw the saved games in the current page
            int ySize = (int)((_menuEntrySize.Y * 3) + 20);
            for (int i = 0; i < lastIndex; i++)
            {
                bool isSelected = IsActive && i == _selectedGame;

                if (_selectedGame != -1 && _selectedGame != 3)
                    ScreenManager.SpriteBatch.Draw(
                        TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                        new Rectangle(
                            (int)_currentPosition.X, (int)_currentPosition.Y,
                            (int)(GameSettings.DefaultInstance.ResolutionWidth / 2.15f), ySize),
                        isSelected ? Color.White : Color.Transparent);

                _savedGames[_selectedPage * 3 + i].Draw(this, _currentPosition, isSelected, gameTime);

                _currentPosition.Y += ySize + _menuEntriesSeparation;
            }

            // Draw cancel buttons
            int yPosition = (int)
                (ScreenManager.FontTitle.MeasureString("Prueba").Y +
                 (_menuEntrySize.Y * 9) + ((5 * 4) * 3) + (_menuEntriesSeparation * 3) +
                 (_menuEntrySize.Y * 2) + (_menuEntriesSeparation * 2));
            
            if (_selectedGame == lastIndex)
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle((int)GameSettings.DefaultInstance.ResolutionWidth / 4, yPosition,
                                  (int)(GameSettings.DefaultInstance.ResolutionWidth / 7), (int)_menuEntrySize.Y),
                    Color.White);

            ScreenManager.SpriteBatch.DrawString(
                ScreenManager.FontEntriesSelected,
                StringHelper.DefaultInstance.Get("cancel"),
                new Vector2(GameSettings.DefaultInstance.ResolutionWidth / 4, yPosition),
                _selectedGame == lastIndex ? Color.White : Color.SlateGray);

            ScreenManager.SpriteBatch.End();

            _currentPosition = _initialPosition;
        }

        #endregion
    }
}
