using System;
using System.Collections.Generic;
using System.Text;
using MetalSpace.Interfaces;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventListener_LogMessage</c> class represents the behaviour
    /// when it is necessary to print a message int he log file.
    /// 
    /// Respond to: EventListener_LogMessage
    /// </summary>
    class EventListener_LogMessage : IEventListener
    {
        #region Fields

        /// <summary>
        /// <c>LogHelper</c> where messages are written.
        /// </summary>
        private LogHelper _logHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventListener_LogMessage</c> class.
        /// </summary>
        /// <param name="logFileName">Name of the log file.</param>
        public EventListener_LogMessage(string logFileName)
        {
            _logHelper = new LogHelper(logFileName);
        }

        #endregion

        /// <summary>
        /// Get the name of the event listener.
        /// </summary>
        /// <returns>Name of the event listener.</returns>
        public string GetName() { return "Log_Message_Event_Listener"; }

        /// <summary>
        /// Handle the behaviour of the listener when it is necessary to write a message in the log file.
        /// </summary>
        /// <param name="producedEvent"><c>IEventData</c> that contains the produced event.</param>
        /// <returns>True if the event has been processed and false otherwise.</returns>
        public bool HandleEvent(IEventData producedEvent) 
        {
            if (producedEvent is EventData_LogMessage)
            {
                _logHelper.Write(((EventData_LogMessage)producedEvent).Message);
                return true;
            }
            else
                return false;
        }
    }
}
