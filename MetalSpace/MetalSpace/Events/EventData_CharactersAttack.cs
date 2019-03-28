using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Objects;
using MetalSpace.Scene;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventData_CharactersAttack</c> class represents the data
    /// needed in a collision between characters.
    /// </summary>
    class EventData_CharactersAttack : IEventData
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
        /// Store for the Attacker property.
        /// </summary>
        private Character _attacker;

        /// <summary>
        /// Store for the Attacked property.
        /// </summary>
        private Character _attacked;

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
            set {_eventType = value; }
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
        /// Attacker property
        /// </summary>
        /// <value>
        /// Reference to the attacker.
        /// </value>
        public Character Attacker
        {
            get { return _attacker; }
            set { _attacker = value; }
        }

        /// <summary>
        /// Attacked property
        /// </summary>
        /// <value>
        /// Reference to the attacked.
        /// </value>
        public Character Attacked
        {
            get { return _attacked; }
            set { _attacked = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventData_CharactersAttack</c> class.
        /// </summary>
        /// <param name="attacker">Reference to the attacker.</param>
        /// <param name="attacked">Reference to the attacked.</param>
        public EventData_CharactersAttack(Character attacker, Character attacked)
        {
            _eventType = EventManager.EventType_CharactersAttack;
            
            _attacker = attacker;
            _attacked = attacked;
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
