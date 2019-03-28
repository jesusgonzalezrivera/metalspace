using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetalSpace.Objects;
using MetalSpace.ArtificialIntelligence;

namespace MetalSpace.Interfaces
{
    /// <summary>
    /// Interface with the basic funcionality of an enemy state.
    /// </summary>
    interface IEnemyState
    {
        /// <summary>
        /// Start the animation for the state of the enemy.
        /// </summary>
        /// <param name="context">Context of the enemy.</param>
        void StartAnimation(EnemyContext context);

        /// <summary>
        /// Handle the behaviour of the current state.
        /// </summary>
        /// <param name="context">Context of the enemy.</param>
        void Handle(EnemyContext context);
    }
}
