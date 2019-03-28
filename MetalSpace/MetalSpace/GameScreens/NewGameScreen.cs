using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Models;
using MetalSpace.Cameras;
using MetalSpace.Events;
using MetalSpace.Managers;
using MetalSpace.Settings;
using MetalSpace.Animations;
using MetalSpace.GameComponents;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>NewGameScreen</c> class represents the screen that appears
    /// when the user want to start a new game. In this screen, the user
    /// can select 3 possible difficults: easy, normal and hard.
    /// </summary>
    class NewGameScreen : GameScreen
    {
        #region Fields

        /// <summary>
        /// Separation between the difficult entries.
        /// </summary>
        private int _menuEntriesSeparation;

        /// <summary>
        /// Current position of the drawing.
        /// </summary>
        private Vector2 _currentPosition;

        /// <summary>
        /// Initial position for the drawing.
        /// </summary>
        private Vector2 _initialPosition;

        /// <summary>
        /// Size of each difficult entry.
        /// </summary>
        private Vector2 _menuEntrySize;

        /// <summary>
        /// Store for the SelectedOption property.
        /// </summary>
        private bool _selectedOption;

        /// <summary>
        /// Store for the SelectedDifficult property.
        /// </summary>
        private int _selectedDifficult;

        /// <summary>
        /// Store for the SelectedEnd property.
        /// </summary>
        private bool _selectedEnd;

        private DrawableModel _model;
        private ObjectAnimation _animation;

        #endregion

        #region Properties

        /// <summary>
        /// SelectedOption property
        /// </summary>
        /// <value>
        /// Selected difficult option.
        /// </value>
        public bool SelectedOption
        {
            get { return _selectedOption; }
            set { _selectedOption = value; }
        }

        /// <summary>
        /// SelectedDifficult property
        /// </summary>
        /// <value>
        /// Selected difficult option.
        /// </value>
        public int SelectedDifficult
        {
            get { return _selectedDifficult; }
            set { _selectedDifficult = value; }
        }

        /// <summary>
        /// SelectedEnd property
        /// </summary>
        /// <value>
        /// Selected end option (accept, cancel).
        /// </value>
        public bool SelectedEnd
        {
            get { return _selectedEnd; }
            set { _selectedEnd = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>NewGameScreen</c> class.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        public NewGameScreen(string name)
            : this(name, 25)
        {

        }

        /// <summary>
        /// Constructor of the <c>NewGameScreen</c> class.
        /// </summary>
        /// <param name="name">Name of the screen.</param>
        /// <param name="menuEntriesSeparation">Separation between the menu entries.</param>
        public NewGameScreen(string name, int menuEntriesSeparation)
        {
            GameGraphicsDevice = EngineManager.GameGraphicsDevice;

            Name = name;

            TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            TransitionOffTime = TimeSpan.FromSeconds(0.5f);

            _menuEntriesSeparation = menuEntriesSeparation;
            _menuEntrySize = ScreenManager.FontEntries.MeasureString("Example");
            _initialPosition = new Vector2(50f, (GameSettings.DefaultInstance.ResolutionHeight / 2) -
                (((_menuEntrySize.Y * 2) + TextureManager.GetTexture("EasyDifficult").BaseTexture.Height + 
                  (_menuEntriesSeparation * 2)) / 2));
            _currentPosition = _initialPosition;

            _selectedEnd = false;
            _selectedDifficult = 0;
            _selectedOption = false;

            _model = new DrawableModel((GameModel)ModelManager.GetModel("AnimatedMonster"),
                new Vector3(12.5f, 10, 0), new Vector3(0, MathHelper.ToRadians(180), 0), Vector3.One * 10f, 0);
            
            _animation = new ObjectAnimation(Vector3.Zero, Vector3.Zero,
                _model.Rotation, _model.Rotation + new Vector3(MathHelper.TwoPi, 0, 0),
                TimeSpan.FromSeconds(5f), true);
        }

        #endregion

        #region Handle Input

        /// <summary>
        /// Handle the input of the user relative to the <c>NewGameScreen</c> screen.
        /// </summary>
        /// <param name="input">Current state of the player input.</param>
        /// <param name="gameTime">Current time of the game.</param>
        public override void HandleInput(Input input, GameTime gameTime)
        {
            if (input.Up || input.Down)
            {
                _selectedOption = !_selectedOption;
                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);
            }

            if (input.Left)
            {
                if (!_selectedOption)
                    _selectedDifficult = _selectedDifficult - 1 < 0 ? 2 : _selectedDifficult - 1;
                else
                    _selectedEnd = !_selectedEnd;

                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);
            }

            if (input.Right)
            {
                if (!_selectedOption)
                    _selectedDifficult = _selectedDifficult + 1 > 2 ? 0 : _selectedDifficult + 1;
                else
                    _selectedEnd = !_selectedEnd;

                SoundManager.GetSound("MenuSelect").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuSelect").Play(true);
            }

            if (input.Start || input.Action)
            {
                EventManager.Trigger(new EventData_LogMessage("VALORES: " + _selectedOption + " - " + _selectedEnd + " - " + _selectedDifficult));
                SoundManager.GetSound("MenuAccept").Volume = GameSettings.DefaultInstance.SoundVolume;
                SoundManager.GetSound("MenuAccept").Play(true);
                if (_selectedOption && _selectedEnd)
                {
                    OnCancel();
                }
                else if ((_selectedOption && !_selectedEnd) || (!_selectedOption || _selectedEnd))
                {
                    switch (_selectedDifficult)
                    {
                        case 0:
                            GameSettings.DefaultInstance.Difficult = Difficult.Easy;
                            break;
                        case 1:
                            GameSettings.DefaultInstance.Difficult = Difficult.Normal;
                            break;
                        case 2:
                            GameSettings.DefaultInstance.Difficult = Difficult.Hard;
                            break;
                    }

                    GameSettings.Save();

                    ScreenManager.RemoveScreen("Background");
                    ScreenManager.RemoveScreen("MainMenu");
                    ScreenManager.AddScreen("LoadingScreen", new LoadingGame("Map_1_3.txt", true));
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

            // Set the camera
            CameraManager.AddCamera(CameraManager.DefaultCamera,
                new TargetCamera(new Vector3(0, 0, 70), new Vector3(0, 0, 0)));
            CameraManager.SetActiveCamera(CameraManager.DefaultCamera);
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the necessary elements of the new game screen.
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

            _animation.Update(gameTime.ElapsedGameTime);
            _model.Rotation = new Vector3(_animation.Rotation.X, _model.Rotation.Y, _model.Rotation.Z);
        }

        #endregion

        #region Draw Method

        private Dictionary<string, Matrix> _individualTransformations =
            new Dictionary<string,Matrix>();

        /// <summary>
        /// Draw the current state of the <c>NewGameScreen</c> screen.
        /// </summary>
        /// <param name="gameTime">Current time of the game.</param>
        public override void Draw(GameTime gameTime)
        {
            if (!TextureManager.GetTexture("EasyDifficult").ReadyToRender ||
                !TextureManager.GetTexture("NormalDifficult").ReadyToRender ||
                !TextureManager.GetTexture("HardDifficult").ReadyToRender)
                return;

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise, null, CanvasManager.ScaleMatrix);

            GameGraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GameGraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            GameGraphicsDevice.BlendState = BlendState.AlphaBlend;
            GameGraphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Draw the easy difficult
            float xPosition = _currentPosition.X;
            ScreenManager.SpriteBatch.Draw(
                TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                new Rectangle((int)xPosition, (int)_currentPosition.Y,
                              (int)_menuEntrySize.X, (int)_menuEntrySize.Y),
                !_selectedOption && _selectedDifficult == 0 ? Color.White : Color.SlateGray);

            ScreenManager.SpriteBatch.DrawString(
                ScreenManager.FontEntriesSelected,
                StringHelper.DefaultInstance.Get("difficult_easy"),
                new Vector2(xPosition, _currentPosition.Y),
                !_selectedOption && _selectedDifficult == 0 ? Color.White : Color.SlateGray);

            // Draw the normal difficult
            xPosition += (((GameSettings.DefaultInstance.ResolutionWidth / 2) - 30f) / 3);
            ScreenManager.SpriteBatch.Draw(
                TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                new Rectangle((int)xPosition, (int)_currentPosition.Y,
                              (int)_menuEntrySize.X, (int)_menuEntrySize.Y),
                !_selectedOption && _selectedDifficult == 1 ? Color.White : Color.SlateGray);

            ScreenManager.SpriteBatch.DrawString(
                ScreenManager.FontEntriesSelected,
                StringHelper.DefaultInstance.Get("difficult_normal"),
                new Vector2(xPosition, _currentPosition.Y),
                !_selectedOption && _selectedDifficult == 1 ? Color.White : Color.SlateGray);

            // Draw the hard difficult
            xPosition += (((GameSettings.DefaultInstance.ResolutionWidth / 2) - 30f) / 3);
            ScreenManager.SpriteBatch.Draw(
                TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                new Rectangle((int)xPosition, (int)_currentPosition.Y,
                              (int)_menuEntrySize.X, (int)_menuEntrySize.Y),
                !_selectedOption && _selectedDifficult == 2 ? Color.White : Color.SlateGray);

            ScreenManager.SpriteBatch.DrawString(
                ScreenManager.FontEntriesSelected,
                StringHelper.DefaultInstance.Get("difficult_hard"),
                new Vector2(xPosition, _currentPosition.Y),
                !_selectedOption && _selectedDifficult == 2 ? Color.White : Color.SlateGray);

            _currentPosition.Y += _menuEntrySize.Y +
                TextureManager.GetTexture("HardDifficult").BaseTexture.Height + _menuEntriesSeparation *2;

            // Draw the accept and cancel buttons
            if (_selectedOption && !_selectedEnd)
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle((int)_currentPosition.X, (int) _currentPosition.Y,
                                  (int)(GameSettings.DefaultInstance.ResolutionWidth / 4), (int)_menuEntrySize.Y),
                    Color.White);

            ScreenManager.SpriteBatch.DrawString(
                ScreenManager.FontEntriesSelected,
                StringHelper.DefaultInstance.Get("accept"),
                new Vector2(_currentPosition.X, _currentPosition.Y),
                _selectedOption && !_selectedEnd ? Color.White : Color.SlateGray);

            if (_selectedOption && _selectedEnd)
                ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("DummyTexture15T").BaseTexture,
                    new Rectangle((int)GameSettings.DefaultInstance.ResolutionWidth / 3, (int)_currentPosition.Y,
                                  (int)(GameSettings.DefaultInstance.ResolutionWidth / 4), (int)_menuEntrySize.Y),
                    Color.White);

            ScreenManager.SpriteBatch.DrawString(
                ScreenManager.FontEntriesSelected,
                StringHelper.DefaultInstance.Get("cancel"),
                new Vector2(GameSettings.DefaultInstance.ResolutionWidth / 3, (int)_currentPosition.Y),
                _selectedOption && _selectedEnd ? Color.White : Color.SlateGray);

            _model.Draw(CameraManager.ActiveCamera.View, CameraManager.ActiveCamera.Projection,
                DrawingMethod.NoInstancing, _individualTransformations, null);

            ScreenManager.SpriteBatch.End();

            _currentPosition = _initialPosition;
        }

        #endregion
    }
}
