using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetalSpace.Interfaces;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventListener_UnitActionChanged</c> class represents the behaviour
    /// when the player changes his action.
    /// 
    /// Respond to: EventData_UnitActionChanged
    /// </summary>
    class EventListener_UnitActionChanged : IEventListener
    {
        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventListener_UnitActionChanged</c> class.
        /// </summary>
        public EventListener_UnitActionChanged()
        {
            
        }

        #endregion

        /// <summary>
        /// Get the name of the event listener.
        /// </summary>
        /// <returns>Name of the event listener.</returns>
        public string GetName() { return "Unit_Action_Changed_Event_Listener"; }

        /// <summary>
        /// Handle the behaviour of the listener when the player change its action.
        /// </summary>
        /// <param name="producedEvent"><c>IEventData</c> that contains the produced event.</param>
        /// <returns>True if the event has been processed and false otherwise.</returns>
        public bool HandleEvent(IEventData producedEvent) 
        {
            if (producedEvent is EventData_UnitActionChanged)
            {
                if (((EventData_UnitActionChanged)producedEvent).Unit.LastPlayerAction != 
                    ((EventData_UnitActionChanged)producedEvent).Unit.PlayerAction)
                    ((EventData_UnitActionChanged)producedEvent).Unit.LastPlayerAction = 
                        ((EventData_UnitActionChanged)producedEvent).Unit.PlayerAction;

                ((EventData_UnitActionChanged)producedEvent).Unit.PlayerAction = 
                    ((EventData_UnitActionChanged)producedEvent).NewAction;

                // Change animation

                return true;
            }

            return false;
        }
    }
}
