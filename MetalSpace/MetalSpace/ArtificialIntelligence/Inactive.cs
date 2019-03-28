using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Objects;
using MetalSpace.Models;
using MetalSpace.Interfaces;

namespace MetalSpace.ArtificialIntelligence
{
    /// <summary>
    /// The <c>Inactive</c> class permits to implement the behaviour of
    /// the enemy when it is inactive.
    /// </summary>
    class Inactive : IEnemyState
    {
        /// <summary>
        /// Store the instance of the <c>Inactive</c> behaviour.
        /// </summary>
        public static Inactive Instance = new Inactive();

        /// <summary>
        /// Constructor of the <c>Inactive</c> class.
        /// </summary>
        public Inactive() { }

        /// <summary>
        /// Start the animation of the Inactive enemy.
        /// </summary>
        /// <param name="context">Context of the enemy.</param>
        public void StartAnimation(EnemyContext context)
        {
            ((AnimatedModel) context.Enemy.DModel).TimeSpeed = 0.25f;
            ((AnimatedModel)context.Enemy.DModel).Animation.StartClip("Waiting", true);
        }

        /// <summary>
        /// Handle the behaviour of the enemy when it is inactive.
        /// </summary>
        /// <param name="context">Context of the enemy.</param>
        public void Handle(EnemyContext context)
        {
            context.Enemy.Speed = new Vector2(0, context.Enemy.Speed.Y);
        }
    }
}
