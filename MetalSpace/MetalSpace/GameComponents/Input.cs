using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MetalSpace.Settings;

namespace MetalSpace.GameComponents
{
    public class Input : GameComponent
    {
        #region Fields

        /// <summary>
        /// false if the user needs to umpress the key to detect another
        /// or true otherwise.
        /// </summary>
        public static bool Continuous = false;

        /// <summary>
        /// Store for the LastKeyboardState property.
        /// </summary>
        private static KeyboardState _lastKeyboardState;

        /// <summary>
        /// Store for the CurrentKeyboardState property.
        /// </summary>
        private static KeyboardState _currentKeyboardState;

        private static MouseState _lastMouseState;
        
        private static MouseState _currentMouseState;
        
        private Point _lastMouseLocation;

        private Vector2 _mouseMoved;
        
        #endregion

        #region Properties

        /// <summary>
        /// LastKeyboardState property
        /// </summary>
        /// <value>
        /// Get the last keyboard state.
        /// </value>
        public static KeyboardState LastKeyboardState
        {
            get { return _lastKeyboardState; }
            set { _lastKeyboardState = value; }
        }

        /// <summary>
        /// CurrentKeyboardState property
        /// </summary>
        /// <value>
        /// Get the current keyboard state.
        /// </value>
        public static KeyboardState CurrentKeyboardState
        {
            get { return _currentKeyboardState; }
            set { _currentKeyboardState = value; }
        }

        /// <summary>
        /// True if the Start key is pressed, false otherwise.
        /// </summary>
        public bool Start
        {
            get { return IsNewKeyPressed(GameSettings.DefaultInstance.StartKey); }
        }

        /// <summary>
        /// True if the Select key is pressed, false otherwise.
        /// </summary>
        public bool Select
        {
            get { return IsNewKeyPressed(GameSettings.DefaultInstance.SelectKey); }
        }

        /// <summary>
        /// True if the Up key is pressed, false otherwise.
        /// </summary>
        public bool Up
        {
            get { return IsNewKeyPressed(GameSettings.DefaultInstance.UpKey); }
        }

        /// <summary>
        /// True if the Down key is pressed, false otherwise.
        /// </summary>
        public bool Down
        {
            get { return IsNewKeyPressed(GameSettings.DefaultInstance.DownKey); }
        }

        /// <summary>
        /// True if the Left key is pressed, false otherwise.
        /// </summary>
        public bool Left
        {
            get { return IsNewKeyPressed(GameSettings.DefaultInstance.LeftKey); }
        }

        /// <summary>
        /// True if the Right key is pressed, false otherwise.
        /// </summary>
        public bool Right
        {
            get { return IsNewKeyPressed(GameSettings.DefaultInstance.RightKey); }
        }

        /// <summary>
        /// True if the Action key is pressed, false otherwise.
        /// </summary>
        public bool Action
        {
            get { return IsNewKeyPressed(GameSettings.DefaultInstance.ActionKey); }
        }

        /// <summary>
        /// True if the Jump key is pressed, false otherwise.
        /// </summary>
        public bool Jump
        {
            get { return IsNewKeyPressed(GameSettings.DefaultInstance.JumpKey); }
        }

        /// <summary>
        /// True if the Attack key is pressed, false otherwise.
        /// </summary>
        public bool Attack
        {
            get { return IsNewKeyPressed(GameSettings.DefaultInstance.AttackKey); }
        }

        /// <summary>
        /// True if the SAttack key is pressed, false otherwise.
        /// </summary>
        public bool SAttack
        {
            get { return IsNewKeyPressed(GameSettings.DefaultInstance.SAttackKey); }
        }

        /// <summary>
        /// True if the Inventory key is pressed, false otherwise.
        /// </summary>
        public bool Inventory
        {
            get { return IsNewKeyPressed(GameSettings.DefaultInstance.InventoryKey); }
        }

        /// <summary>
        /// True if the Cancel key is pressed, false otherwise.
        /// </summary>
        public bool Cancel
        {
            get { return IsNewKeyPressed(GameSettings.DefaultInstance.CancelKey); }
        }

        public static MouseState LastMouseState
        {
            get { return _lastMouseState; }
            set { _lastMouseState = value; }
        }

        public static MouseState CurrentMouseState
        {
            get { return _currentMouseState; }
            set { _currentMouseState = value; }
        }

        public Vector2 MouseMoved
        {
            get { return _mouseMoved; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>Input</c> class.
        /// </summary>
        /// <param name="game">Reference to the main game.</param>
        public Input(Game game) : base(game)
        {
            Enabled = true;

            GameSettings.Initialize();
        }

        #endregion

        #region IsNewKeyPressed Method

        /// <summary>
        /// Indicates if a new key is pressed.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if the key is pressed and false otherwise.</returns>
        bool IsNewKeyPressed(Keys key)
        {
            if (Continuous == true)
                return _currentKeyboardState.IsKeyDown(key);
            else
                return (_currentKeyboardState.IsKeyDown(key) &&
                        _lastKeyboardState.IsKeyUp(key));
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the state of the current and last keyboard state.
        /// </summary>
        public void Update()
        {
            _lastKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _lastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            _mouseMoved = new Vector2(_lastMouseState.X - _currentMouseState.X,
                _lastMouseState.Y - _currentMouseState.Y);
            _lastMouseLocation = new Point(_currentMouseState.X, _currentMouseState.Y);
        }

        #endregion
    }
}
