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
    /// The <c>EventData_SaveGame</c> class represents the data needed
    /// to save the current state of the game.
    /// </summary>
    class EventData_SaveGame : IEventData
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
        private Character _player;

        /// <summary>
        /// Store for the Enemies property.
        /// </summary>
        private Character[] _enemies;

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
        /// Name of the level where the player moves.
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
        public Character Player
        {
            get { return _player; }
            set { _player = value; }
        }

        /// <summary>
        /// Enemies property
        /// </summary>
        /// <value>
        /// Reference to the enemies in the map.
        /// </value>
        public Character[] Enemies
        {
            get { return _enemies; }
            set { _enemies = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventData_SavedGame</c> class.
        /// </summary>
        /// <param name="mapName">Name of the map where the player moves.</param>
        /// <param name="player">Reference to the main player.</param>
        /// <param name="enemies">Reference to the enemies in the map.</param>
        public EventData_SaveGame(string mapName, Character player, Character[] enemies)
        {
            _eventType = EventManager.EventType_SavedGames;

            _mapName = mapName;
            _player = player;
            _enemies = enemies;
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
