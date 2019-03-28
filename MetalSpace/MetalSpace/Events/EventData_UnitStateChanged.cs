using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetalSpace.Objects;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventData_StateChanged</c> class represents the data needed
    /// when an unit change its state.
    /// </summary>
    class EventData_UnitStateChanged : IEventData
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
        /// Store for the Unit property.
        /// </summary>
        private Player _unit;

        /// <summary>
        /// Store for the NewState property.
        /// </summary>
        private Player.State _newState;

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
        /// Unit property
        /// </summary>
        /// <value>
        /// Reference to the player that change its action.
        /// </value>
        public Player Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        /// <summary>
        /// NewState property
        /// </summary>
        /// <value>
        /// The new <c>Player.State</c> that the user is going to use.
        /// </value>
        public Player.State NewState
        {
            get { return _newState; }
            set { _newState = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the <c>EventData_UnitStateChanged</c> class.
        /// </summary>
        /// <param name="unit">Reference to the unit.</param>
        /// <param name="newState">New <c>Player.State</c> for the unit.</param>
        public EventData_UnitStateChanged(Player unit, Player.State newState)
        {
            _eventType = EventManager.EventType_UnitStateChanged;

            _unit = unit;
            _newState = newState;
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
