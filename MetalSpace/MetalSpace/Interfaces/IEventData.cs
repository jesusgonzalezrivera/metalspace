using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetalSpace.Events;

namespace MetalSpace.Interfaces
{
    /// <summary>
    /// Interface with the common event data.
    /// </summary>
    public interface IEventData
    {
        #region Properties

        /// <summary>
        /// Hash that contains the type of the event.
        /// </summary>
        HashType MyEventType { get; set; }

        /// <summary>
        /// Time stamp of the event.
        /// </summary>
        float TimeStamp { get; set; }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Get the hash type of the event.
        /// </summary>
        /// <returns>Type of the event.</returns>
        HashType GetEventType();

        /// <summary>
        /// Get the time stamp of the event.
        /// </summary>
        /// <returns>Time stamp of the event.</returns>
        float GetTimeStamp();

        #endregion
    }
}
