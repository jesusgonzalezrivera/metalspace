using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalSpace.Interfaces
{
    /// <summary>
    /// Interface of the event listener.
    /// </summary>
    public interface IEventListener
    {
        /// <summary>
        /// Get the name of the event listener.
        /// </summary>
        /// <returns>Name of the event listener.</returns>
        string GetName();

        /// <summary>
        /// Handle the behaviour of a new produced event.
        /// </summary>
        /// <param name="producedEvent">Produced event.</param>
        /// <returns>true if the produced event was handled, false otherwise.</returns>
        bool HandleEvent(IEventData producedEvent);
    }
}
