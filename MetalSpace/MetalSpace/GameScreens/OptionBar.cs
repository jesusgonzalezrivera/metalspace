using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Managers;
using MetalSpace.Settings;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>OptionBar</c> class represents a bar that show the
    /// current value between a minimum and maximum values.
    /// </summary>
    class OptionBar
    {
        #region Fields

        /// <summary>
        /// Store for the MinimumValue property.
        /// </summary>
        private float _minimumValue;

        /// <summary>
        /// Store for the MaximumValue property.
        /// </summary>
        private float _maximumValue;

        #endregion

        #region Properties

        /// <summary>
        /// MinimumValue property
        /// </summary>
        /// <value>
        /// Minimum value of the bar.
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
        /// Maximum value of the bar.
        /// </value>
        public float MaximumValue
        {
            get { return _maximumValue; }
            set { _maximumValue = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>OptionBar</c> class.
        /// </summary>
        /// <param name="minimumValue">Minimum value of the bar.</param>
        /// <param name="maximumValue">Maximum value of the bar.</param>
        public OptionBar(float minimumValue, float maximumValue)
        {
            _minimumValue = minimumValue;
            _maximumValue = maximumValue;
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the bar.
        /// </summary>
        /// <param name="screen">Reference to the screen that contains the bar.</param>
        /// <param name="isSelected">true if the <c>OptionBar</c> is selected, false otherwise.</param>
        /// <param name="gametime">Global time of the game.</param>
        public virtual void Update(NumericOptionEntry screen, bool isSelected,
                                   GameTime gametime)
        {
            
        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state ofthe game.
        /// </summary>
        /// <param name="screen">Reference to the screen that contains the bar.</param>
        /// <param name="position">Position where the <c>OptionBar</c> is drawed.</param>
        /// <param name="currentValue">Current value of the <c>OptionBar</c>.</param>
        /// <param name="isSelected">true if the <c>OptionBar</c> is selected, false otherwise.</param>
        /// <param name="gametime">Global time of the game.</param>
        public virtual void Draw(NumericOptionEntry screen, Vector2 position, float currentValue,
                                 bool isSelected, GameTime gameTime)
        {
            Texture2D borderTexture = TextureManager.GetTexture("BarTexture").BaseTexture;

            double percentToDraw = currentValue / _maximumValue;
            double eachPercentWidth = borderTexture.Width / _maximumValue;

            int xPosition = GameSettings.DefaultInstance.ResolutionWidth / 3;
            ScreenManager.SpriteBatch.Draw(
                TextureManager.GetTexture("DummyTexture").BaseTexture,
                new Rectangle(xPosition, (int)position.Y+8, 
                              (int)((percentToDraw*100) * eachPercentWidth), borderTexture.Height),
                Color.Red);

            ScreenManager.SpriteBatch.Draw(
                borderTexture,
                new Rectangle(xPosition, (int)position.Y+8, borderTexture.Width, borderTexture.Height),
                Color.White);
        }

        #endregion
    }
}
