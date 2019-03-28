using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Models;
using MetalSpace.Objects;
using MetalSpace.Managers;
using MetalSpace.Settings;
using MetalSpace.Interfaces;

namespace MetalSpace.ArtificialIntelligence
{
    /// <summary>
    /// The <c>Following</c> class permits to implement the behaviour of
    /// the enemy when it is following the player.
    /// </summary>
    class Following : IEnemyState
    {
        /// <summary>
        /// Store the instance of the <c>Following</c> behaviour.
        /// </summary>
        public static Following Instance = new Following();

        /// <summary>
        /// Constructor of the <c>Following</c> class.
        /// </summary>
        public Following() { }

        /// <summary>
        /// Start the animation of the Following enemy.
        /// </summary>
        /// <param name="context">Context of the enemy.</param>
        public void StartAnimation(EnemyContext context)
        {
            ((AnimatedModel)context.Enemy.DModel).TimeSpeed = 0.75f;
            ((AnimatedModel)context.Enemy.DModel).Animation.StartClip("Walking", true);

            // Start a new sound when the enemy detect the player
            SoundManager.GetSound("EnemyPlayerDetected").Volume = 
                GameSettings.DefaultInstance.SoundVolume;
            SoundManager.GetSound("EnemyPlayerDetected").Play(true, false);
        }

        /// <summary>
        /// Handle the behaviour of the enemy when it is following the player.
        /// </summary>
        /// <param name="context">Context of the enemy.</param>
        public void Handle(EnemyContext context)
        {
            float distanceX = (float)Math.Round(context.Player.Position.X - context.Enemy.Position.X, 0);
            
            // If the distance if below 0, the player is on the left
            if (distanceX < 0)
            {
                Vector3 rotation = context.Enemy.DModel.Rotation;
                context.Enemy.Rotation = new Vector3(MathHelper.ToRadians(270), rotation.Y, rotation.Z);

                if (context.Enemy.PlayerXDirection != Enemy.XDirection.Left)
                {
                    context.Enemy.LastPlayerXDirection = context.Enemy.PlayerXDirection;
                    context.Enemy.PlayerXDirection = Enemy.XDirection.Left;
                }
                context.Enemy.Speed = new Vector2(-context.Enemy.MaxSpeed.X, context.Enemy.Speed.Y);
            }
            // If the distance if above 0, the player is on the right
            else if (distanceX > 0)
            {
                Vector3 rotation = context.Enemy.DModel.Rotation;
                context.Enemy.Rotation = new Vector3(MathHelper.ToRadians(90), rotation.Y, rotation.Z);

                if (context.Enemy.PlayerXDirection != Enemy.XDirection.Right)
                {
                    context.Enemy.LastPlayerXDirection = context.Enemy.PlayerXDirection;
                    context.Enemy.PlayerXDirection = Enemy.XDirection.Right;
                }
                context.Enemy.Speed = new Vector2(context.Enemy.MaxSpeed.X, context.Enemy.Speed.Y);
            }
        }
    }
}
