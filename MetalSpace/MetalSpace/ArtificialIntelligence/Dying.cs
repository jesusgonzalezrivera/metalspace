using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Objects;
using MetalSpace.Models;
using MetalSpace.Interfaces;

namespace MetalSpace.ArtificialIntelligence
{
    /// <summary>
    /// The <c>Dying</c> class permits to implement the behaviour of
    /// the enemy when it is dying.
    /// </summary>
    class Dying : IEnemyState
    {
        /// <summary>
        /// Store the instance of the <c>Dying</c> behaviour.
        /// </summary>
        public static Dying Instance = new Dying();

        /// <summary>
        /// Constructor of the <c>Dying</c> class.
        /// </summary>
        public Dying() { }

        /// <summary>
        /// Start the animation of the Dying enemy.
        /// </summary>
        /// <param name="context">Context of the enemy.</param>
        public void StartAnimation(EnemyContext context)
        {
            ((AnimatedModel)context.Enemy.DModel).TimeSpeed = 1f;
            ((AnimatedModel)context.Enemy.DModel).Animation.StartClip("Dying", false);
        }

        /// <summary>
        /// Handle the behaviour of the enemy when it is dying.
        /// </summary>
        /// <param name="context"></param>
        public void Handle(EnemyContext context)
        {
            // Change the current speed for disappear in the ground.
            context.Enemy.Speed = new Vector2(1f, 0f);
        }
    }
}
