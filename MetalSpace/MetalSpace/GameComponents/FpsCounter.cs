using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MetalSpace.GameComponents
{
    /// <summary>
    /// The <c>FpsCounter</c> class represents a counter of the number
    /// of frames per second that the game is capable to render.
    /// </summary>
    class FpsCounter : GameComponent
    {
        #region Fields

        /// <summary>
        /// Store for the FPS property.
        /// </summary>
        private float _fps;

        /// <summary>
        /// Get the number of frames since the last update.
        /// </summary>
        private float _framecount;

        /// <summary>
        /// Get the interval between the each update.
        /// </summary>
        private float _updateInterval;

        /// <summary>
        /// Get the time since the last update proccess.
        /// </summary>
        private float _timeSinceLastUpdate;

        #endregion

        #region Properties

        /// <summary>
        /// FPS property
        /// </summary>
        /// <value>
        /// Get the number of frames per second.
        /// </value>
        public float FPS
        {
            get { return _fps; }
        }

        #endregion

        #region Construtor

        /// <summary>
        /// Constructor of the <c>FpsCounter</c> class.
        /// </summary>
        /// <param name="game">Reference to the main game.</param>
        public FpsCounter(Game game)
            : base(game)
        {
            Enabled = true;

            _fps = 0;
            _framecount = 0;
            _updateInterval = 1.0f;
            _timeSinceLastUpdate = 0.0f;
        }

        #endregion

        #region Update Method

        /// <summary>
        /// Update the FPS in each execution.
        /// </summary>
        /// <param name="gameTime">Global time of the game.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _framecount++;
            _timeSinceLastUpdate += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timeSinceLastUpdate > _updateInterval)
            {
                _fps = _framecount / _timeSinceLastUpdate;

                _framecount = 0;
                _timeSinceLastUpdate -= _updateInterval;
            }
        }

        #endregion
    }
}
