using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetalSpace.Objects;
using MetalSpace.Models;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventListener_UnitStateChanged</c> class represents the behaviour
    /// when the player changes his state.
    /// 
    /// Respond to: EventData_UnitActionChanged
    /// </summary>
    class EventListener_UnitStateChanged : IEventListener
    {
        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventListener_UnitStateChanged</c> class.
        /// </summary>
        public EventListener_UnitStateChanged()
        {
            
        }

        #endregion

        /// <summary>
        /// Get the name of the event listener.
        /// </summary>
        /// <returns>Name of the event listener.</returns>
        public string GetName() { return "Unit_State_Changed_Event_Listener"; }

        /// <summary>
        /// Handle the behaviour of the listener when the player change its state.
        /// </summary>
        /// <param name="producedEvent"><c>IEventData</c> that contains the produced event.</param>
        /// <returns>True if the event has been processed and false otherwise.</returns>
        public bool HandleEvent(IEventData producedEvent) 
        {
            if (!(producedEvent is EventData_UnitStateChanged))
                return false;

            if (((EventData_UnitStateChanged)producedEvent).Unit.PlayerState != 
                ((EventData_UnitStateChanged)producedEvent).Unit.LastPlayerState)
                ((EventData_UnitStateChanged)producedEvent).Unit.LastPlayerState = 
                    ((EventData_UnitStateChanged)producedEvent).Unit.PlayerState;

            if (((EventData_UnitStateChanged)producedEvent).NewState == Player.State.Jumping &&
                ((EventData_UnitStateChanged)producedEvent).Unit.EndJump == true)
            {
                ((EventData_UnitStateChanged)producedEvent).Unit.Jump = true;
                ((EventData_UnitStateChanged)producedEvent).Unit.EndJump = false;

                ((AnimatedModel) ((EventData_UnitStateChanged)producedEvent).Unit.DModel).TimeSpeed = 1.2f;
                ((AnimatedModel) ((EventData_UnitStateChanged)producedEvent).Unit.DModel).Animation.StartClip("StartJump", true);
            }

            if (((EventData_UnitStateChanged)producedEvent).NewState == Player.State.Climbing)
                ((AnimatedModel)((EventData_UnitStateChanged)producedEvent).Unit.DModel).Animation.SetFrameFromClip("Climbing", 1);

            ((EventData_UnitStateChanged)producedEvent).Unit.PlayerState = ((EventData_UnitStateChanged)producedEvent).NewState;

            return true;
        }
    }
}
