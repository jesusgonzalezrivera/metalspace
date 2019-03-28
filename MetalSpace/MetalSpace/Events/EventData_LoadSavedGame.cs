using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Objects;
using MetalSpace.Scene;
using MetalSpace.Managers;
using MetalSpace.Interfaces;
using MetalSpace.GameScreens;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventData_LoadSavedGame</c> class represents the data
    /// needed when we load a saved game.
    /// </summary>
    class EventData_LoadSavedGame : IEventData
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
        /// Store for the FileName property.
        /// </summary>
        private string _filename;
        
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
        /// Filename property
        /// </summary>
        /// <value>
        /// Filename of the saved game.
        /// </value>
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventData_LoadSavedGame</c> class.
        /// </summary>
        /// <param name="fileName">Filename of the saved game.</param>
        public EventData_LoadSavedGame(string fileName)
        {
            _eventType = EventManager.EventType_SavedGames;

            _filename = fileName;
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
