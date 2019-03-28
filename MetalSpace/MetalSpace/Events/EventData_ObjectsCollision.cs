using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetalSpace.Objects;
using MetalSpace.Scene;
using MetalSpace.Managers;
using MetalSpace.Interfaces;

namespace MetalSpace.Events
{
    /// <summary>
    /// The <c>EventData_ObjectsCollision</c> class represents the data
    /// needed in a collision of a <c>MoveableObject</c>.
    /// </summary>
    class EventData_ObjectsCollision : IEventData
    {
        /// <summary>
        /// Direction of the <c>MoveableObject</c> respect to the node.
        /// </summary>
        public enum CollisionDirection
        {
            /// <summary>
            /// Up direction.
            /// </summary>
            Up,
            /// <summary>
            /// Down direction.
            /// </summary>
            Down,
            /// <summary>
            /// Left direction.
            /// </summary>
            Left,
            /// <summary>
            /// Right direction.
            /// </summary>
            Right
        }

        /// <summary>
        /// Type of surface of the node collided with the <c>MoveableObject</c>.
        /// </summary>
        public enum CollisionSurface
        {
            /// <summary>
            /// Box type collision.
            /// </summary>
            Box,
            /// <summary>
            /// Plane type collision.
            /// </summary>
            Plane
        }

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
        /// Store for the Node property.
        /// </summary>
        private OcTreeNode _node;

        /// <summary>
        /// Store for the MObject property.
        /// </summary>
        private MoveableObject _mObject;

        /// <summary>
        /// Store for the Direction property.
        /// </summary>
        private CollisionDirection _direction;

        /// <summary>
        /// Store for the Type property.
        /// </summary>
        private CollisionSurface _type;

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
        /// Node property
        /// </summary>
        /// <value>
        /// Node that collide with the <c>MoveableObject</c>.
        /// </value>
        public OcTreeNode Node
        {
            get { return _node; }
            set { _node = value; }
        }

        /// <summary>
        /// MObject property
        /// </summary>
        /// <value>
        /// Reference to the <c>MoveableObject</c> that collide with the node.
        /// </value>
        public MoveableObject MObject
        {
            get { return _mObject; }
            set { _mObject = value; }
        }

        /// <summary>
        /// Direction property
        /// </summary>
        /// <value>
        /// Direction of the <c>MoveableObject</c>.
        /// </value>
        public CollisionDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// Type property
        /// </summary>
        /// <value>
        /// Type of surface of the node collision.
        /// </value>
        public CollisionSurface Type
        {
            get { return _type; }
            set { _type = value; }
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
        /// Constructor of the <c>EventData_ObjectsCollision</c> class.
        /// </summary>
        /// <param name="mObject"><c>MoveableObject</c> that collide with a node.</param>
        /// <param name="node">Node collisioned with the <c>MoveableObject</c>.</param>
        /// <param name="direction">Direction of the collision.</param>
        /// <param name="type">Type of surface collision.</param>
        public EventData_ObjectsCollision(MoveableObject mObject, ref OcTreeNode node, 
            CollisionDirection direction, CollisionSurface type)
        {
            _eventType = EventManager.EventType_UnitCollision;

            _addValue = 0f;
            _direction = direction;
            _type = type;
            _node = node;
            _mObject = mObject;
        }

        /// <summary>
        /// Constructor of the <c>EventData_ObjectsCollision</c> class.
        /// </summary>
        /// <param name="mObject"><c>MoveableObject</c> that collide with a node.</param>
        /// <param name="node">Node collisioned with the <c>MoveableObject</c>.</param>
        /// <param name="direction">Direction of the collision.</param>
        /// <param name="type">Type of surface collision.</param>
        /// <param name="addValue">Value to be added to the new position.</param>
        public EventData_ObjectsCollision(MoveableObject mObject, ref OcTreeNode node, 
            CollisionDirection direction, CollisionSurface type, float addValue)
        {
            _eventType = EventManager.EventType_UnitCollision;

            _addValue = addValue;
            _direction = direction;
            _type = type;
            _node = node;
            _mObject = mObject;
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
