using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Objects;
using MetalSpace.Models;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventListener_UnitDirectionChanged</c> class represents the behaviour
    /// when the player change its direction.
    /// 
    /// Respond to: EventData_UnitDirectionXChanged, EventData_UnitDirectionYChanged
    /// </summary>
    class EventListener_UnitDirectionChanged : IEventListener
    {
        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventListener_UnitDirectionChanged</c> class.
        /// </summary>
        public EventListener_UnitDirectionChanged()
        {
            
        }

        #endregion

        /// <summary>
        /// Get the name of the event listener.
        /// </summary>
        /// <returns>Name of the event listener.</returns>
        public string GetName() { return "Unit_Direction_Changed_Event_Listener"; }

        /// <summary>
        /// Handle the behaviour of the listener when the player change its direction.
        /// </summary>
        /// <param name="producedEvent"><c>IEventData</c> that contains the produced event.</param>
        /// <returns>True if the event has been processed and false otherwise.</returns>
        public bool HandleEvent(IEventData producedEvent) 
        {
            if (producedEvent is EventData_UnitDirectionXChanged)
            {
                // Change X Direction
                if (((EventData_UnitDirectionXChanged)producedEvent).Unit.LastPlayerXDirection != 
                    ((EventData_UnitDirectionXChanged)producedEvent).Unit.PlayerXDirection)
                    ((EventData_UnitDirectionXChanged)producedEvent).Unit.LastPlayerXDirection = 
                        ((EventData_UnitDirectionXChanged)producedEvent).Unit.PlayerXDirection;

                ((EventData_UnitDirectionXChanged)producedEvent).Unit.PlayerXDirection = 
                    ((EventData_UnitDirectionXChanged)producedEvent).DirectionX;

                if (((EventData_UnitDirectionXChanged)producedEvent).Unit.PlayerState == Player.State.Climbing)
                    return true;

                if (((EventData_UnitDirectionXChanged)producedEvent).DirectionX == Player.XDirection.Right)
                {
                    Vector3 rotation = ((EventData_UnitDirectionXChanged)producedEvent).Unit.Rotation;
                    ((EventData_UnitDirectionXChanged)producedEvent).Unit.Rotation =
                        new Vector3(MathHelper.ToRadians(90), rotation.Y, rotation.Z);

                    ((EventData_UnitDirectionXChanged)producedEvent).Unit.DirectionIndicator = 1;

                    if (((EventData_UnitDirectionXChanged)producedEvent).Unit.PlayerState != Player.State.Jumping)
                    {
                        ((AnimatedModel)((EventData_UnitDirectionXChanged)producedEvent).Unit.DModel).TimeSpeed = 3.0f;
                        if (((EventData_UnitDirectionXChanged)producedEvent).Unit.PlayerYDirection == Player.YDirection.Up)
                            ((AnimatedModel)((EventData_UnitDirectionXChanged)producedEvent).Unit.DModel).Animation.StartClip("RunningD", true);
                        else 
                            ((AnimatedModel)((EventData_UnitDirectionXChanged)producedEvent).Unit.DModel).Animation.StartClip("RunningH", true);
                    }
                }
                else if (((EventData_UnitDirectionXChanged)producedEvent).DirectionX == Player.XDirection.Left)
                {
                    Vector3 rotation = ((EventData_UnitDirectionXChanged)producedEvent).Unit.Rotation;
                    ((EventData_UnitDirectionXChanged)producedEvent).Unit.Rotation =
                        new Vector3(MathHelper.ToRadians(270), rotation.Y, rotation.Z);
                    
                    ((EventData_UnitDirectionXChanged)producedEvent).Unit.DirectionIndicator = -1;
                    if (((EventData_UnitDirectionXChanged)producedEvent).Unit.PlayerState != Player.State.Jumping)
                    {
                        ((AnimatedModel)((EventData_UnitDirectionXChanged)producedEvent).Unit.DModel).TimeSpeed = 3.0f;
                        if (((EventData_UnitDirectionXChanged)producedEvent).Unit.PlayerYDirection == Player.YDirection.Up)
                            ((AnimatedModel)((EventData_UnitDirectionXChanged)producedEvent).Unit.DModel).Animation.StartClip("RunningD", true);
                        else
                            ((AnimatedModel)((EventData_UnitDirectionXChanged)producedEvent).Unit.DModel).Animation.StartClip("RunningH", true);
                    }
                }
                else
                {
                    ((EventData_UnitDirectionXChanged)producedEvent).Unit.DirectionIndicator = 0;
                    if (((EventData_UnitDirectionXChanged)producedEvent).Unit.PlayerState != Player.State.Jumping)
                    {
                        if (((EventData_UnitDirectionXChanged)producedEvent).Unit.PlayerYDirection == Player.YDirection.Up)
                        {
                            ((AnimatedModel)((EventData_UnitDirectionXChanged)producedEvent).Unit.DModel).TimeSpeed = 1.0f;
                            ((AnimatedModel)((EventData_UnitDirectionXChanged)producedEvent).Unit.DModel).Animation.StartClip("ShootUp", true);
                        }
                        else
                        {
                            ((AnimatedModel)((EventData_UnitDirectionXChanged)producedEvent).Unit.DModel).TimeSpeed = 1.0f;
                            ((AnimatedModel)((EventData_UnitDirectionXChanged)producedEvent).Unit.DModel).Animation.StartClip("Waiting", true);
                        }
                    }
                }

                return true;
            }
            else if (producedEvent is EventData_UnitDirectionYChanged)
            {
                /* Change Y Direction */
                if (((EventData_UnitDirectionYChanged)producedEvent).Unit.LastPlayerYDirection != ((EventData_UnitDirectionYChanged)producedEvent).Unit.PlayerYDirection)
                    ((EventData_UnitDirectionYChanged)producedEvent).Unit.LastPlayerYDirection = ((EventData_UnitDirectionYChanged)producedEvent).Unit.PlayerYDirection;

                ((EventData_UnitDirectionYChanged)producedEvent).Unit.PlayerYDirection = ((EventData_UnitDirectionYChanged)producedEvent).DirectionY;
                
                if (((EventData_UnitDirectionYChanged)producedEvent).Unit.PlayerState == Player.State.Climbing)
                {
                    if (((EventData_UnitDirectionYChanged)producedEvent).Unit.PlayerYDirection != Player.YDirection.None)
                    {
                        ((AnimatedModel)((EventData_UnitDirectionYChanged)producedEvent).Unit.DModel).TimeSpeed = 1.5f;
                        ((AnimatedModel)((EventData_UnitDirectionYChanged)producedEvent).Unit.DModel).Animation.StartClip("Climbing", true);
                    }
                    else
                    {
                        ((AnimatedModel)((EventData_UnitDirectionYChanged)producedEvent).Unit.DModel).Animation.StopClip("Climbing");
                    }
                }
                else if (((EventData_UnitDirectionYChanged)producedEvent).Unit.PlayerState == Player.State.Waiting)
                {
                    if (((EventData_UnitDirectionYChanged)producedEvent).Unit.PlayerYDirection == Player.YDirection.Up)
                    {
                        ((AnimatedModel)((EventData_UnitDirectionYChanged)producedEvent).Unit.DModel).TimeSpeed = 1f;
                        ((AnimatedModel)((EventData_UnitDirectionYChanged)producedEvent).Unit.DModel).Animation.StartClip("ShootUp", true);
                    }
                    else
                    {
                        ((AnimatedModel)((EventData_UnitDirectionYChanged)producedEvent).Unit.DModel).TimeSpeed = 1.0f;
                        ((AnimatedModel)((EventData_UnitDirectionYChanged)producedEvent).Unit.DModel).Animation.StartClip("Waiting", true);
                    }
                }
                else if (((EventData_UnitDirectionYChanged)producedEvent).Unit.PlayerState == Player.State.Running)
                {
                    if (((EventData_UnitDirectionYChanged)producedEvent).Unit.PlayerYDirection == Player.YDirection.Up)
                    {
                        ((AnimatedModel)((EventData_UnitDirectionYChanged)producedEvent).Unit.DModel).TimeSpeed = 3.0f;
                        ((AnimatedModel)((EventData_UnitDirectionYChanged)producedEvent).Unit.DModel).Animation.StartClip("RunningD", true);
                    }
                    else
                    {
                        ((AnimatedModel)((EventData_UnitDirectionYChanged)producedEvent).Unit.DModel).TimeSpeed = 3.0f;
                        ((AnimatedModel)((EventData_UnitDirectionYChanged)producedEvent).Unit.DModel).Animation.StartClip("RunningH", true);
                    }
                }

                return true;
            }
            else
                return false;
        }
    }
}
