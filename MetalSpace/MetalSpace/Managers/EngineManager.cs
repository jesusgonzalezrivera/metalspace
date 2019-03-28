using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MetalSpace.Managers
{
    /// <summary>
    /// The <c>EngineManager</c> class represents the class where the usar
    /// can access to the current instance of the game and their internal 
    /// graphics tools (device manager...).
    /// </summary>
    class EngineManager : Engine
    {
        #region Fields

        /// <summary>
        /// Store for the Game property.
        /// </summary>
        private static Game _game;

        #endregion

        #region Game property

        /// <summary>
        /// Game property
        /// </summary>
        /// <value>
        /// Reference to the main game.
        /// </value>
        public static Game Game
        {
            get { return _game; }
            set { _game = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>EngineManager</c> class.
        /// </summary>
        public EngineManager() : base("MetalSpace") { }

        /// <summary>
        /// Constructor of the <c>EngineManager</c> class.
        /// </summary>
        /// <param name="name">Name of the game (the title appears in the menu and the title of the windows).</param>
        public EngineManager(string name) : base(name) { }

        #endregion

        #region Draw Method

        /// <summary>
        /// Draw the current state of the game.
        /// </summary>
        /// <param name="gametime">Global time of the game.</param>
        protected override void Draw(GameTime gametime)
        {
            base.Draw(gametime);
        }

        #endregion
    }
}
