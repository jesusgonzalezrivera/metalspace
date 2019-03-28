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
    /// The <c>EventData_PickedObject</c> class represents the data
    /// needed when the player picks up a object.
    /// </summary>
    class EventData_PickedObject : IEventData
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
        /// Store for the Player property.
        /// </summary>
        private Character _player;

        /// <summary>
        /// Store for the Object property.
        /// </summary>
        private MoveableObject _object;

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
        /// MObject property
        /// </summary>
        /// <value>
        /// Reference to the <c>MoveableObject</c> picked up by the player.
        /// </value>
        public MoveableObject MObject
        {
            get { return _object; }
            set { _object = value; }
        }

        /// <summary>
        /// Player property
        /// </summary>
        /// <value>
        /// Reference to the main player.
        /// </value>
        public Character Player
        {
            get { return _player; }
            set { _player = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the <c>EventData_PickedObject</c> class.
        /// </summary>
        /// <param name="player">Reference to the player.</param>
        /// <param name="pObject">Reference to the picked object.</param>
        public EventData_PickedObject(Character player, MoveableObject pObject)
        {
            _eventType = EventManager.EventType_PickedObject;

            _player = player;
            _object = pObject;
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
