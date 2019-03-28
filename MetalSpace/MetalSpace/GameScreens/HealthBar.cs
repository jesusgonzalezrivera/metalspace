using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MetalSpace.Settings;
using MetalSpace.Managers;

namespace MetalSpace.GameScreens
{
    /// <summary>
    /// The <c>HealthBar</c> class represents a simple bar that indicates the
    /// amount of life of the player.
    /// </summary>
    public class HealthBar
    {
        #region Fields

        /// <summary>
        /// Store for the TotalLife property.
        /// </summary>
        private float _totalLife;

        /// <summary>
        /// Store for the CurrentLife property.
        /// </summary>
        private float _currentLife;

        /// <summary>
        /// Region be drawn.
        /// </summary>
        private Rectangle _drawRegion;

        /// <summary>
        /// Percent of the bar to be drawn.
        /// </summary>
        private double _percentToDraw;

        /// <summary>
        /// Width of each percent.
        /// </summary>
        private double _eachPercentWidth;

        #endregion

        #region Properties

        /// <summary>
        /// TotalLife property
        /// </summary>
        /// <value>
        /// Total life of the player (without any damage).
        /// </value>
        public float TotalLife
        {
            get { return _totalLife; }
            set { _totalLife = value; }
        }

        /// <summary>
        /// CurrentLife property
        /// </summary>
        /// <value>
        /// Current life of the player.
        /// </value>
        public float CurrentLife
        {
            get { return _currentLife; }
            set 
            { 
                _currentLife = value;
                _percentToDraw = _currentLife / _totalLife;
                _eachPercentWidth = _drawRegion.Width / _totalLife;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>HealthBar</c> class.
        /// </summary>
        /// <param name="totalLife">Total life of the player.</param>
        /// <param name="currentLife">Current life of the player.</param>
        public HealthBar(int totalLife, int currentLife)
        {
            _totalLife = totalLife;
            _currentLife = currentLife;
        }

        #endregion

        #region Load Method

        /// <summary>
        /// Load and calculate all the necessary content of the game.
        /// </summary>
        public void Load()
        {
            Vector2 viewportSize = new Vector2(
                GameSettings.DefaultInstance.ResolutionWidth,
                GameSettings.DefaultInstance.ResolutionHeight);

            _drawRegion = new Rectangle(
                (int)(viewportSize.X / 16f), (int)(viewportSize.Y / 16f),
                (int)(viewportSize.X * 2f / 8f), (int)(viewportSize.Y / 32f));

            _percentToDraw = _currentLife / _totalLife;
            _eachPercentWidth = _drawRegion.Width / _totalLife;
        }

        #endregion

        #region Unload Method

        /// <summary>
        /// Unload the necessary elements of the background
        /// </summary>
        public void Unload()
        {

        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the current state of the bar.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Update(GameTime gameTime)
        {

        }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the bar.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.DrawString(ScreenManager.FontEntriesSelected,
                (_percentToDraw * 100).ToString(),
                new Vector2(_drawRegion.X + (_drawRegion.Width / 2.5f), _drawRegion.Y - _drawRegion.Height * 1.25f),
                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            ScreenManager.SpriteBatch.Draw(
                TextureManager.GetTexture("DummyTexture").BaseTexture,
                new Rectangle(_drawRegion.X + 10, _drawRegion.Y + 3,
                              (int)((_percentToDraw * 100) * _eachPercentWidth) - 20, _drawRegion.Height - 6),
                Color.Red);

            ScreenManager.SpriteBatch.Draw(TextureManager.GetTexture("LifeBar").BaseTexture,
                _drawRegion, Color.White);
        }

        #endregion
    }
}
