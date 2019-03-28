using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Settings;
using MetalSpace.Interfaces;
using MetalSpace.Events;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>NumericOptionEntry</c> class represents an option that can have
    /// a list of numeric values. It contains an instance of the <c>OptionBar</c> class
    /// that represents that numeric value.
    /// </summary>
    public class NumericOptionEntry : IOptionEntry
    {
        #region Fields

        /// <summary>
        /// Store for the Text property.
        /// </summary>
        private string _text;

        /// <summary>
        /// Store for the MinimumValue property.
        /// </summary>
        private float _minimumValue;

        /// <summary>
        /// Store for the MaximumValue property.
        /// </summary>
        private float _maximumValue;

        /// <summary>
        /// Store for the CurrentValue property.
        /// </summary>
        private float _currentValue;

        /// <summary>
        /// Refence to a <c>OptionBar</c> that represents the values.
        /// </summary>
        private OptionBar _bar;

        #endregion

        #region Properties

        /// <summary>
        /// Text property
        /// </summary>
        /// <value>
        /// Text that represents the current menu option.
        /// </value>
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// MinimumValue property
        /// </summary>
        /// <value>
        /// Minimum value of the list of values.
        /// </value>
        public float MinimumValue
        {
            get { return _minimumValue; }
            set { _minimumValue = value; }
        }

        /// <summary>
        /// MaximumValue property
        /// </summary>
        /// <value>
        /// Maximum value of the list of values.
        /// </value>
        public float MaximumValue
        {
            get { return _maximumValue; }
            set { _maximumValue = value; }
        }

        /// <summary>
        /// CurrentValue property
        /// </summary>
        /// <value>
        /// Current value in the list of values.
        /// </value>
        public float CurrentValue
        {
            get { return _currentValue; }
            set { _currentValue = value; }
        }

        #endregion

        public event EventHandler<EventArgs> Selected;

        public void OnSelectEntry(int direction)
        {
            float percentToDraw = _maximumValue / 10;
            if (direction == 0)
                _currentValue = _currentValue - percentToDraw < 0 ? _minimumValue : _currentValue - percentToDraw;
            else
                _currentValue = _currentValue + percentToDraw > _maximumValue ? _maximumValue : _currentValue + percentToDraw;
        }

        #region Constructor

        /// <summary>
        /// Constructor of the <c>NumericOptionEntry</c> class.
        /// </summary>
        /// <param name="text">Text that represents the current menu option.</param>
        /// <param name="minimumValue">Minimum value of the numeric list value.</param>
        /// <param name="maximumValue">Maximum value of the numeric list value.</param>
        /// <param name="currentValue">Current value of the numeric list value.</param>
        public NumericOptionEntry(string text, float minimumValue, 
            float maximumValue, float currentValue)
        {
            _text = text;
            _minimumValue = minimumValue;
            _currentValue = currentValue;
            _maximumValue = maximumValue;
            _bar = new OptionBar(_minimumValue, _maximumValue);
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the numeric option entry.
        /// </summary>
        /// <param name="screen">Reference to the screen that contains the option.</param>
        /// <param name="isSelected">true if the current option is selected, false otherwise.</param>
        /// <param name="gametime">Global time of the game.</param>
        public void Update(OptionsMenu screen, bool isSelected, GameTime gametime)
        {
            _bar.Update(this, isSelected, gametime);
            /*float fadeSpeed = (float)gametime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                SelectionFade = Math.Min(SelectionFade + fadeSpeed, 1);
            else
                SelectionFade = Math.Max(SelectionFade - fadeSpeed, 0);*/
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the numeric option entry.
        /// </summary>
        /// <param name="screen">Reference to the screen that contains the option.</param>
        /// <param name="position">Position where the option is drawed.</param>
        /// <param name="isSelected">true if the current option is selected, false otherwise.</param>
        /// <param name="gametime">Global time of the game.</param>
        public void Draw(OptionsMenu screen, Vector2 position, bool isSelected, GameTime gameTime)
        {
            Color color = isSelected ? Color.White : Color.SlateGray;
            color = new Color(color.R, color.G, color.B, screen.TransitionAlpha);

            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected, _text,
                position, color, 0, new Vector2(0.5f, 0.5f), 0.75f, SpriteEffects.None, 0);

            int xPosition = GameSettings.DefaultInstance.ResolutionWidth / 4;
            _bar.Draw(this, new Vector2(xPosition, position.Y), _currentValue, isSelected, gameTime);
        }

        #endregion
    }
}
