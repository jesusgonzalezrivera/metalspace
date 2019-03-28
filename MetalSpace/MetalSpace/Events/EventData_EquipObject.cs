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
    /// The <c>EventData_EquipObject</c> class represents the data
    /// needed when the main player equip a object.
    /// </summary>
    class EventData_EquipObject : IEventData
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

        /// <summary>
        /// Store for the InventoryScreen property.
        /// </summary>
        private InventoryScreen _inventoryScreen;

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
        /// Player property
        /// </summary>
        /// <value>
        /// Reference to the player that equip the object.
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
        /// Reference to the object that the player equip.
        /// </value>
        public Objects.Object NewObject
        {
            get{return _newObject;}
            set{_newObject=value;}
        }

        /// <summary>
        /// InventoryScreen property
        /// </summary>
        /// <value>
        /// Reference to the screen that represents the player inventory.
        /// </value>
        public InventoryScreen InventoryScreen
        {
            get { return _inventoryScreen; }
            set { _inventoryScreen = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventData_EquipObject</c> class.
        /// </summary>
        /// <param name="player">Reference to the main player.</param>
        /// <param name="newObject">Reference to the object to equip.</param>
        /// <param name="inventoryScreen">Reference to the inventory screen.</param>
        public EventData_EquipObject(Player player, Objects.Object newObject, 
            InventoryScreen inventoryScreen)
        {
            _eventType = EventManager.EventType_ChangePlayerObjects;

            _player = player;
            _newObject = newObject;
            _inventoryScreen = inventoryScreen;
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
