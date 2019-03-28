using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Objects;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventData_LevelChanged</c> class represents the data
    /// needed when the player go to another level.
    /// </summary>
    class EventData_LevelChanged : IEventData
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
        /// Store for the MapName property.
        /// </summary>
        private string _mapName;

        /// <summary>
        /// Store for the Player property.
        /// </summary>
        private Player _player = null;

        /// <summary>
        /// Store for the NewPosition property.
        /// </summary>
        private Vector3 _newPosition = Vector3.Zero;

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
        /// MapName property
        /// </summary>
        /// <value>
        /// Name of the map that contains the new level.
        /// </value>
        public string MapName
        {
            get { return _mapName; }
            set { _mapName = value; }
        }

        /// <summary>
        /// Player property
        /// </summary>
        /// <value>
        /// Reference to the main player.
        /// </value>
        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        /// <summary>
        /// NewPosition property
        /// </summary>
        /// <value>
        /// Position of the player when appears in the new level.
        /// </value>
        public Vector3 NewPosition
        {
            get { return _newPosition; }
            set { _newPosition = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventData_LevelChanged</c> class.
        /// </summary>
        /// <param name="mapName">Name of the map.</param>
        public EventData_LevelChanged(string mapName)
        {
            _eventType = EventManager.EventType_LevelChanged;

            _mapName = mapName;
        }

        /// <summary>
        /// Constructor of the <c>EventData_LevelChanged</c> class.
        /// </summary>
        /// <param name="mapName">Name of the map.</param>
        /// <param name="player">Reference to the player.</param>
        /// <param name="newPosition">Position of the player in the new level.</param>
        public EventData_LevelChanged(string mapName, Player player, 
            Vector3 newPosition) : this(mapName)
        {
            _player = player;
            _newPosition = newPosition;
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
