using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Settings;
using MetalSpace.Interfaces;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>ListOptionMenu</c> class represents an option that can have
    /// a value in a list of possible values.
    /// </summary>
    class ListOptionMenu : IOptionEntry
    {
        #region Fields

        /// <summary>
        /// Store for the Text property.
        /// </summary>
        private string _text;

        /// <summary>
        /// Store for the AllowedValues property.
        /// </summary>
        private List<string> _allowedValues;

        /// <summary>
        /// Store for the CurrentValue property.
        /// </summary>
        private string _currentValue;

        #endregion

        #region Properties

        /// <summary>
        /// Text property
        /// </summary>
        /// <value>
        /// Text that represent the <c>ListOptionMenu</c>.
        /// </value>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        
        /// <summary>
        /// AllowedValues property
        /// </summary>
        /// <value>
        /// Allowed value for the <c>ListOptionMenu</c>.
        /// </value>
        public List<string> AllowedValues
        {
            get { return _allowedValues; }
            set { _allowedValues = value; }
        }
        
        /// <summary>
        /// CurrentValue property
        /// </summary>
        /// <value>
        /// Current value of the <c>ListOptionMenu</c>.
        /// </value>
        public string CurrentValue
        {
            get { return _currentValue; }
            set { _currentValue = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>ListOptionMenu</c> class.
        /// </summary>
        /// <param name="text">Text that represent the option.</param>
        /// <param name="allowedValues">Allowed values of the option.</param>
        /// <param name="currentValue">Current value of the option.</param>
        public ListOptionMenu(string text, List<string> allowedValues, string currentValue)
        {
            _text = text;
            _allowedValues = allowedValues;
            _currentValue = currentValue;
        }

        #endregion

        public event EventHandler<EventArgs> Selected;

        public void OnSelectEntry(int direction)
        {
            int index = _allowedValues.FindIndex(s => s == _currentValue);
            if (direction == 0)
                _currentValue = _allowedValues[index - 1 < 0 ? _allowedValues.Count - 1 : index - 1];
            else
                _currentValue = _allowedValues[index + 1 == _allowedValues.Count ? 0 : index + 1];
        }

        #region Update Method

        /// <summary>
        /// Update the state of the option.
        /// </summary>
        /// <param name="screen">Reference to the <c>OptionsMenu</c> class that cotains this option.</param>
        /// <param name="isSelected">true if the option is selected, false otherwise.</param>
        /// <param name="gametime">Global time of the game.</param>
        public void Update(OptionsMenu screen, bool isSelected, GameTime gametime)
        {
            
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the option.
        /// </summary>
        /// <param name="screen">Reference to the <c>OptionsMenu</c> class that cotains this option.</param>
        /// <param name="position">Position where the option should be drawed.</param>
        /// <param name="isSelected">true if the option is selected, false otherwise.</param>
        /// <param name="gametime">Global time of the game.</param>
        public void Draw(OptionsMenu screen, Vector2 position, bool isSelected, GameTime gameTime)
        {
            Color color = isSelected ? Color.White : Color.SlateGray;
            color = new Color(color.R, color.G, color.B, screen.TransitionAlpha);

            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected, _text, position, color, 0,
                Vector2.Zero, 0.75f, SpriteEffects.None, 0);

            int xPosition = GameSettings.DefaultInstance.ResolutionWidth / 3;
            string textEntry = "<  " + _currentValue + "  >";
            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected, textEntry,
                new Vector2(xPosition, position.Y), color, 0, Vector2.Zero, 0.75f, SpriteEffects.None, 0);
        }

        #endregion
    }
}
