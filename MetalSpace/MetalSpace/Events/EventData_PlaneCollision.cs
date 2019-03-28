using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MetalSpace.Objects;
using MetalSpace.Scene;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventData_PlaneCollision</c> class represents the data needed
    /// when the Player collide with a plane.
    /// </summary>
    class EventData_PlaneCollision: IEventData
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
        /// Store for the Plane property.
        /// </summary>
        private Plane _plane;

        /// <summary>
        /// Store for the MObject property.
        /// </summary>
        private MoveableObject _mObject;

        /// <summary>
        /// Store for the AddValue property.
        /// </summary>
        private float _addValue;

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
        /// BoundingPlane property
        /// </summary>
        /// <value>
        /// BoundingPlane that represents the surface of the collision.
        /// </value>
        public Plane BoundingPlane
        {
            get{return _plane;}
            set{_plane=value;}
        }

        /// <summary>
        /// MObject property
        /// </summary>
        /// <value>
        /// <c>MoveableObject</c> that collide with the plane surface.
        /// </value>
        public MoveableObject MObject
        {
            get { return _mObject; }
            set { _mObject = value; }
        }

        /// <summary>
        /// AddValue property
        /// </summary>
        /// <value>
        /// Value to be added to the new position of the <c>MoveableObject</c>.
        /// </value>
        public float AddValue
        {
            get { return _addValue; }
            set { _addValue = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventData_PlaneCollision</c> class.
        /// </summary>
        /// <param name="mObject"><c>MoveableObject</c> that collide.</param>
        /// <param name="plane">Plane that represents the surface of the collision.</param>
        /// <param name="addValue">Value to be added to the new position.</param>
        public EventData_PlaneCollision(MoveableObject mObject, Plane plane, 
            float addValue)
        {
            _eventType = EventManager.EventType_UnitCollision;
            
            _plane = plane;
            _mObject = mObject;
            _addValue = addValue;
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
