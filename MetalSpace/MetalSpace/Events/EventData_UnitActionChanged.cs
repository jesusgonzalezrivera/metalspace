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
    /// The <c>EventData_ActionChanged</c> class represents the data needed
    /// when an unit change its action.
    /// </summary>
    class EventData_UnitActionChanged : IEventData
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
        /// Store for the NewAction property.
        /// </summary>
        private Player.Action _newAction;

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
        /// NewAction property
        /// </summary>
        /// <value>
        /// The new <c>Player.Action</c> that the user is going to use.
        /// </value>
        public Player.Action NewAction
        {
            get { return _newAction; }
            set { _newAction = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the <c>EventData_UnitActionChanged</c> class.
        /// </summary>
        /// <param name="unit">Reference to the unit.</param>
        /// <param name="newAction">New <c>Player.Action</c> for the unit.</param>
        public EventData_UnitActionChanged(ref Player unit, Player.Action newAction)
        {
            _eventType = EventManager.EventType_UnitActionChanged;

            _unit = unit;
            _newAction = newAction;
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
