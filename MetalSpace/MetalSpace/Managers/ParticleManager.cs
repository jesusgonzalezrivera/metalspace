using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using DPSF;

namespace MetalSpace.Managers
{
    /// <summary>
    /// The <c>ParticleManager</c> class represents a reference that permits
    /// to use the particle manager anywhere in the application.
    /// </summary>
    class ParticleManager : GameComponent
    {
        #region Fields

        /// <summary>
        /// Store for the Initialized property.
        /// </summary>
        private static bool _initialized = false;

        /// <summary>
        /// Store for the ParticleManager property.
        /// </summary>
        private ParticleSystemManager _particleManager;

        #endregion

        #region Properties

        /// <summary>
        /// Initialized property
        /// </summary>
        /// <value>
        /// true if the <c>ParticleManager</c> was initialized, false otherwise.
        /// </value>
        public static bool Initialized
        {
            get { return _initialized; }
            set { _initialized = value; }
        }

        /// <summary>
        /// ParticleSystem property
        /// </summary>
        /// <value>
        /// Reference to the particle system manager.
        /// </value>
        public ParticleSystemManager ParticleSystem
        {
            get { return _particleManager; }
            set { _particleManager = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>ParticleManager</c> class.
        /// </summary>
        /// <param name="game">Reference to the main game.</param>
        public ParticleManager(Game game) : base(game)
        {
            _particleManager = new ParticleSystemManager();
        }

        #endregion

        #region Initialize Method

        /// <summary>
        /// Initialize the content of the <c>ParticleManager</c>.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _initialized = true;
        }

        #endregion
    }
}
