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
    /// The <c>Attacked</c> class permits to implement the behaviour of
    /// the enemy when it is attacked by the player.
    /// </summary>
    class Attacked : IEnemyState
    {
        /// <summary>
        /// Store the instance of the <c>Attacked</c> behaviour.
        /// </summary>
        public static Attacked Instance = new Attacked();

        /// <summary>
        /// Constructor of the <c>Attacked</c> class.
        /// </summary>
        public Attacked() { }

        /// <summary>
        /// Start the animation of the attacked enemy.
        /// </summary>
        /// <param name="context">Context of the enemy.</param>
        public void StartAnimation(EnemyContext context)
        {
            // Set the animation speed and play the Attacked animation.
            ((AnimatedModel)context.Enemy.DModel).TimeSpeed = 2f;
            ((AnimatedModel)context.Enemy.DModel).Animation.StartClip("Attacked", false);
        }

        /// <summary>
        /// Handle the behaviour of the attacked enemy. When the attacked
        /// animation finish, change the behaviour of the last behaviour.
        /// </summary>
        /// <param name="context"></param>
        public void Handle(EnemyContext context)
        {
            if (((AnimatedModel)context.Enemy.DModel).Animation.Done == true)
            {
                ((AnimatedModel)context.Enemy.DModel).Damaged = false;
                context.ChangeState(context.LastState);
            }
        }
    }
}
