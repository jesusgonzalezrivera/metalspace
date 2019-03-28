using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventData_LogMessage</c> class represents the data
    /// needed save a new message in the log file.
    /// </summary>
    class EventData_LogMessage : IEventData
    {
        #region Fields

        /// <summary>
        /// Store for the EventType property.
        /// </summary>
        private HashType _eventType;

        /// <summary>
        /// Store for the TimeStamp property.
        /// </summary>
        private float _timeStamp = 0.0f;

        /// <summary>
        /// Store for the Message property.
        /// </summary>
        private string _message;

        #endregion

        #region Properties

        /// <summary>
        /// EventType property
        /// </summary>
        /// <value>
        /// Hash that contains the event type.
        /// </value>
        public HashType MyEventType
        {
            get { return _eventType; }
            set { _eventType = value; }
        }

        /// <summary>
        /// TimeStamp property
        /// </summary>
        /// <value>
        /// Time stamp of the event.
        /// </value>
        public float TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }

        /// <summary>
        /// Message property
        /// </summary>
        /// <value>
        /// Message to be written in the log file.
        /// </value>
        public string Message
        {
            get { return _message; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventData_LogMessage</c> class.
        /// </summary>
        /// <param name="message">Message to be written in the log file.</param>
        public EventData_LogMessage(string message)
        {
            _message = message;
            _eventType = EventManager.EventType_LogMessage;
        }

        #endregion

        #region Virtual Methods implementation

        /// <summary>
        /// Get the type of the event.
        /// </summary>
        /// <returns>Type of the event.</returns>
        public HashType GetEventType() { return _eventType; }

        /// <summary>
        /// Get the time stamp of the event.
        /// </summary>
        /// <returns>Time stamp of the event.</returns>
        public float GetTimeStamp() { return _timeStamp; }

        #endregion
    }
}
