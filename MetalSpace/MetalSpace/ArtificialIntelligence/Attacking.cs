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
    /// The <c>Attacking</c> class permits to implement the behaviour of
    /// the enemy when it attacks the player.
    /// </summary>
    class Attacking : IEnemyState
    {
        /// <summary>
        /// Store the instance of the <c>Attacking</c> behaviour.
        /// </summary>
        public static Attacking Instance = new Attacking();

        /// <summary>
        /// Constructor of the <c>Attacking</c> class.
        /// </summary>
        public Attacking() { }

        /// <summary>
        /// Start the animation of the attacking enemy.
        /// </summary>
        /// <param name="context">Context of the enemy.</param>
        public void StartAnimation(EnemyContext context)
        {
            ((AnimatedModel)context.Enemy.DModel).TimeSpeed = 5f;
            ((AnimatedModel)context.Enemy.DModel).Animation.StartClip("RepeatAttack", true);
        }

        /// <summary>
        /// Handle the behaviour of the enemy when it is attacking.
        /// </summary>
        /// <param name="context"></param>
        public void Handle(EnemyContext context)
        {
            // Change the current speed of the enemy for avoid the horizontal
            // position change.
            context.Enemy.Speed = new Vector2(0f, context.Enemy.Speed.Y);
        }
    }
}
