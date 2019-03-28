using System;
using System.Collections.Generic;
using System.Text;
using MetalSpace.Animations;
using Microsoft.Xna.Framework;

namespace MetalSpace.Managers
{
    /// <summary>
    /// The <c>AnimationManager</c> class represents the manager that controls
    /// the <c>ObjectAnimation</c> of the models that are in the game. 
    /// </summary>
    class AnimationManager : GameComponent
    {
        #region Fields

        /// <summary>
        /// Dictionary of animations represented by a key.
        /// </summary>
        private static Dictionary<string, ObjectAnimation> _animations =
            new Dictionary<string, ObjectAnimation>();

        /// <summary>
        /// true if the <c>AnimationManager</c> is initialized, false otherwise.
        /// </summary>
        private static bool _initialized = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>AnimationManager</c> class.
        /// </summary>
        /// <param name="game">Global time of the game.</param>
        public AnimationManager(Game game) : base(game)
        {

        }

        #endregion

        #region Initialize Method

        /// <summary>
        /// Initialize the content of the <c>AnimationManager</c>.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _initialized = true;
        }

        #endregion

        #region AddAnimation Method

        /// <summary>
        /// Add a new <c>ObjectAnimation</c> to the manager.
        /// </summary>
        /// <param name="animationName">Name of the animation.</param>
        /// <param name="newAnimation">Instance of the <c>ObjectAnimation</c>.</param>
        public static void AddAnimation(string animationName, ObjectAnimation newAnimation)
        {
            _animations.Add(animationName, newAnimation);
        }

        #endregion

        #region RemoveAnimation Method

        /// <summary>
        /// Remove a existing <c>ObjectAnimation</c> of the manager.
        /// </summary>
        /// <param name="animationName">Name of the animation.</param>
        public static void RemoveAnimation(string animationName)
        {
            if (!String.IsNullOrEmpty(animationName) && _animations.ContainsKey(animationName))
                _animations.Remove(animationName);
        }

        #endregion

        #region GetAnimation Method

        /// <summary>
        /// Get the animation specified.
        /// </summary>
        /// <param name="animationName">Name of the animation.</param>
        /// <returns><c>ObjectAnimation</c> with the specified name.</returns>
        public static ObjectAnimation GetAnimation(string animationName)
        {
            if (!String.IsNullOrEmpty(animationName) &&
                _animations.ContainsKey(animationName))
                return _animations[animationName];
            else
                return null;
        }

        #endregion
    }
}
