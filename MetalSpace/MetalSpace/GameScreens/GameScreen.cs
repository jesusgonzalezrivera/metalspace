using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MetalSpace.Managers;
using MetalSpace.GameComponents;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// Contains the possible states of a screen.
    /// </summary>
    public enum ScreenState
    {
        /// <summary>
        /// The screen is appearing.
        /// </summary>
        TransitionOn,
        /// <summary>
        /// The screen is active.
        /// </summary>
        Active,
        /// <summary>
        /// The screen is hiding.
        /// </summary>
        TransitionOff,
        /// <summary>
        /// The screen is hidden.
        /// </summary>
        Hidden
    }

    /// <summary>
    /// The <c>GameScreen</c> class represent an abstract screen with an empty
    /// appeance and behaviour. All the screens in the game uses this basic screen.
    /// </summary>
    public abstract class GameScreen
    {
        #region Fields

        /// <summary>
        /// Store for the Name property.
        /// </summary>
        private string _name;

        /// <summary>
        /// Store for the GameGraphicsDevice property.
        /// </summary>
        private GraphicsDevice _graphicsDevice = null;

        /// <summary>
        /// Store for the IsPopup property.
        /// </summary>
        private bool _isPopup = false;

        /// <summary>
        /// Store for the TransitionOnTime property.
        /// </summary>
        private TimeSpan _transitionOnTime = TimeSpan.Zero;

        /// <summary>
        /// Store for the TransitionOffTime property.
        /// </summary>
        private TimeSpan _transitionOffTime = TimeSpan.Zero;

        /// <summary>
        /// Store for the TransitionInstant property.
        /// </summary>
        private float _transitionInstant = 1;

        /// <summary>
        /// Store for the TransitionAlpha property.
        /// </summary>
        private byte _transitionAlpha;

        /// <summary>
        /// Store for the ScreenState property.
        /// </summary>
        private ScreenState _screenState = ScreenState.TransitionOn;

        /// <summary>
        /// Store for the IsExiting property.
        /// </summary>
        private bool _isExiting = false;

        /// <summary>
        /// Store for the OtherScreenHasFocus property.
        /// </summary>
        private bool _otherScreenHasFocus;

        #endregion

        #region Properties

        /// <summary>
        /// Name property
        /// </summary>
        /// <value>
        /// Name that identify the screen.
        /// </value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// GameGraphicsDevice property
        /// </summary>
        /// <value>
        /// Device that is used to render the screen.
        /// </value>
        public GraphicsDevice GameGraphicsDevice
        {
            get { return _graphicsDevice; }
            set { _graphicsDevice = value; }
        }

        /// <summary>
        /// IsPopup property
        /// </summary>
        /// <value>
        /// true if the screen is popup and false otherwise.
        /// </value>
        public bool IsPopup
        {
            get { return _isPopup; }
            set { _isPopup = value; }
        }

        /// <summary>
        /// TransitionOnTime property
        /// </summary>
        /// <value>
        /// Indicates how long the screen takes to transition on when it
        /// is activated
        /// </value>
        public TimeSpan TransitionOnTime
        {
            get { return _transitionOnTime; }
            set { _transitionOnTime = value; }
        }

        /// <summary>
        /// TransitionOffTime property
        /// </summary>
        /// <value>
        /// Indicates how long the screen takes to transition on when it 
        /// is deactivated.
        /// </value>
        public TimeSpan TransitionOffTime
        {
            get { return _transitionOffTime; }
            set { _transitionOffTime = value; }
        }

        /// <summary>
        /// TransitionInstant property
        /// </summary>
        /// <value>
        /// Gets the current instante of the animated transition, from 0 
        /// (fully active, no transition) to 1 (transitioned fully off to 
        /// nothing).
        /// </value>
        public float TransitionInstant
        {
            get { return _transitionInstant; }
            set { _transitionInstant = value; }
        }

        /// <summary>
        /// TransitionAlpha property
        /// </summary>
        /// <value>
        /// Value of the alpha channel to use in the <c>TransitionOn</c> and
        /// <c>TransitionOff</c> states.
        /// </value>
        public byte TransitionAlpha
        {
            get { return (byte)(255 - TransitionInstant * 255); }
        }

        /// <summary>
        /// ScreenState property
        /// </summary>
        /// <value>
        /// Current state of the screen.
        /// </value>
        public ScreenState ScreenState
        {
            get { return _screenState; }
            set { _screenState = value; }
        }

        /// <summary>
        /// IsExiting property
        /// </summary>
        /// <value>
        /// Indicates if the screen will remove itself as soon as the transition 
        /// finished.
        /// </value>
        public bool IsExiting
        {
            get { return _isExiting; }
            set { _isExiting = value; }
        }

        /// <summary>
        /// IsActive property
        /// </summary>
        /// <value>
        /// true if other screen has the focus and false otherwise.
        /// </value>
        public bool IsActive
        {
            get
            {
                return !_otherScreenHasFocus &&
                       (_screenState == ScreenState.TransitionOn ||
                        _screenState == ScreenState.Active);
            }
        }

        #endregion

        #region Virtual Functions
        
        /// <summary>
        /// Virtual function to load all the necessary elements of the screen.
        /// </summary>
        public virtual void Load() { }

        /// <summary>
        /// Virtual function to unload all the elements used by the screen.
        /// </summary>
        public virtual void Unload() { }

        /// <summary>
        /// Update the internal logic (states, alpha, times...) of the screen.
        /// Can be overwrited to implement the update of the interal logic.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        /// <param name="otherScreenHasFocus">true if other screen has the focus, false otherwise.</param>
        /// <param name="coveredByOtherScreen">true if other screen cover this screen, false otherwise.</param>
        public virtual void Update(GameTime gameTime,
            bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            _otherScreenHasFocus = otherScreenHasFocus;

            if (_isExiting)
            {
                // If the screen is exiting, it should transition off
                _screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, _transitionOffTime, 1))
                {
                    // The transition is finished
                    ScreenManager.RemoveScreen(_name);

                    _isExiting = false;
                }
            }
            else if (coveredByOtherScreen)
            {
                // If the screen is covered by other, it should transition off
                if (UpdateTransition(gameTime, _transitionOffTime, 1))
                    // Still busy transitioning
                    _screenState = ScreenState.TransitionOff;
                else
                    // Transition finished
                    _screenState = ScreenState.Hidden;
            }
            else
            {
                // The screen is become active
                if (UpdateTransition(gameTime, _transitionOnTime, -1))
                    // Still busy transitioning
                    _screenState = ScreenState.TransitionOn;
                else
                    // Transition finished
                    _screenState = ScreenState.Active;
            }
        }

        /// <summary>
        /// Updates the screen transition.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        /// <param name="time">Time of the transition.</param>
        /// <param name="direction">Direction of the transition.</param>
        /// <returns></returns>
        private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            // Calculate how much should the transition move
            float transitionDelta;
            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)
                    (gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds);

            // Update the screen transition
            _transitionInstant += transitionDelta * direction;

            // Check if the transition has finished
            if ((_transitionInstant <= 0) || (_transitionInstant >= 1))
            {
                _transitionInstant = MathHelper.Clamp(_transitionInstant, 0, 1);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Virtual function that allow the screen to handle user input.
        /// </summary>
        /// <param name="input">Input of the user.</param>
        /// <param name="gameTime">Global time of the game.</param>
        public virtual void HandleInput(Input input, GameTime gameTime) { }

        /// <summary>
        /// Virtual function that is called when the screen should draw itself.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public virtual void Draw(GameTime gameTime) { }

        /// <summary>
        /// Virtual function that is called when it is necessary to draw after the first drawing.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public virtual void PosUIDraw(GameTime gameTime) { }

        /// <summary>
        /// Close the entire game.
        /// </summary>
        public void ExitScreen()
        {
            if (_transitionOffTime == TimeSpan.Zero)
                ScreenManager.RemoveScreen(_name);
            else
                _isExiting = true;
        }

        #endregion
    }
}
