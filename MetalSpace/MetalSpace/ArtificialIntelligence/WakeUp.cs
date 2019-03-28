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
    /// The <c>WakeUp</c> class permits to implement the behaviour of
    /// the enemy when it is waking up.
    /// </summary>
    class WakeUp : IEnemyState
    {
        /// <summary>
        /// Store the instance of the <c>WakeUp</c> behaviour.
        /// </summary>
        public static WakeUp Instance = new WakeUp();

        /// <summary>
        /// Constructor of the <c>WakeUp</c> class.
        /// </summary>
        public WakeUp() { }

        /// <summary>
        /// Start the animation of the WakeUp enemy.
        /// </summary>
        /// <param name="context">Context of the enemy.</param>
        public void StartAnimation(EnemyContext context)
        {
            ((AnimatedModel)context.Enemy.DModel).TimeSpeed = 1f;
            ((AnimatedModel)context.Enemy.DModel).Animation.StartClip("WakingUp", false);
        }

        /// <summary>
        /// Handle the behaviour of the enemy when it is waking up.
        /// </summary>
        /// <param name="context">Context of the enemy.</param>
        public void Handle(EnemyContext context)
        {
            if (((AnimatedModel)context.Enemy.DModel).Animation.Done == true)
                context.ChangeState(Inactive.Instance);
        }
    }
}
