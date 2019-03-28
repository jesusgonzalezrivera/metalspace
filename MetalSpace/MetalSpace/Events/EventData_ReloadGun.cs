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
    /// The <c>EventData_ReloadGun</c> class represents the data needed
    /// when the player reload his gun.
    /// </summary>
    class EventData_ReloadGun : IEventData
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
        private Player _player;

        /// <summary>
        /// Store for the NewObject property.
        /// </summary>
        private Objects.Object _newObject;

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
        /// Player property
        /// </summary>
        /// <value>
        /// Reference to the player that reload his gun.
        /// </value>
        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        /// <summary>
        /// NewObject property
        /// </summary>
        /// <value>
        /// Reference to 
        /// </value>
        public Objects.Object NewObject
        {
            get{return _newObject;}
            set{_newObject=value;}
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="newObject"></param>
        public EventData_ReloadGun(Player player, Objects.Object newObject)
        {
            _eventType = EventManager.EventType_ChangePlayerObjects;

            _player = player;
            _newObject = newObject;
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
