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
    /// The <c>Unconscious</c> class permits to implement the behaviour of
    /// the enemy when it is unconscious.
    /// </summary>
    class Unconscious: IEnemyState
    {
        /// <summary>
        /// Store the instance of the <c>Unconscious</c> behaviour.
        /// </summary>
        public static Unconscious Instance = new Unconscious();

        /// <summary>
        /// Constructor of the <c>Unconscious</c> class.
        /// </summary>
        public Unconscious() { }

        /// <summary>
        /// Start the animation of the Unconscious enemy.
        /// </summary>
        /// <param name="context">Context of the enemy</param>
        public void StartAnimation(EnemyContext context)
        {
            ((AnimatedModel)context.Enemy.DModel).TimeSpeed = 1f;
            ((AnimatedModel)context.Enemy.DModel).Animation.StartClip("Dying", false);
        }

        /// <summary>
        /// Handle the behaviour of the enemy when it is unconscious.
        /// </summary>
        /// <param name="context">Context of the enemy</param>
        public void Handle(EnemyContext context)
        {
            context.Enemy.Speed = new Vector2(0f, 0f);
        }
    }
}
