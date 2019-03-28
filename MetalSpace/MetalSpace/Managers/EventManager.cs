using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetalSpace.Events;
using MetalSpace.Interfaces;

namespace MetalSpace.Managers
{
    /// <summary>
    /// The <c>EventManager</c> class allows handle the events from all points
    /// of the game, communicating the entities with each other and controlling
    /// the behaviour.
    /// 
    /// There are two types of entities: listeners and events. The listeners 
    /// permist to control the behaviour of one or more types of events. The
    /// events contain the necessary data to be processed in the event.
    /// 
    /// There are 10 types of listeners: LogMessage, UnitCollision,
    /// UnitStateChanged, UnitActionChanged, UnitDirectionChanged,
    /// LevelChanged, CharactersAttack, ChangePlayerObjects, SavedGames,
    /// PickedObject.
    /// </summary>
    public class EventManager
    {
        #region Fields

        /// <summary>
        /// List of the type of listeners that the manager can control.
        /// </summary>
        private static HashSet<HashType> _typeList;

        /// <summary>
        /// List of managers organized for the type of the events that can handle.
        /// </summary>
        private static Dictionary<int, List<IEventListener>> _registry;

        /// <summary>
        /// Number of the active queue.
        /// </summary>
        private static int _activeQueue = 0;

        /// <summary>
        /// List of queues that contains the events.
        /// </summary>
        private static List<IEventData>[] _queues;

        #endregion

        #region Properties

        /// <summary>
        /// Const value to identify events without type.
        /// </summary>
        const int kINFINITE = int.MaxValue;

        /// <summary>
        /// Type for the <c>EventListener_LogMessage</c>.
        /// </summary>
        public static HashType EventType_LogMessage = new HashType("event_log_message");

        /// <summary>
        /// Type for the <c>EventListener_UnitCollision</c>.
        /// </summary>
        public static HashType EventType_UnitCollision = new HashType("event_unit_collision");

        /// <summary>
        /// Type for the <c>EventListener_UnitStateChanged</c>.
        /// </summary>
        public static HashType EventType_UnitStateChanged = new HashType("event_unit_state_changed");

        /// <summary>
        /// Type for the <c>EventListener_UnitDirectionChanged</c>.
        /// </summary>
        public static HashType EventType_UnitDirectionChanged = new HashType("event_unit_direction_changed");

        /// <summary>
        /// Type for the <c>EventListener_LevelChanged</c>.
        /// </summary>
        public static HashType EventType_LevelChanged = new HashType("event_level_changed");

        /// <summary>
        /// Type for the <c>EventListener_CharactersAttack</c>.
        /// </summary>
        public static HashType EventType_CharactersAttack = new HashType("event_characters_attack");

        /// <summary>
        /// Type for the <c>EventListener_ChangePlayerObjects</c>.
        /// </summary>
        public static HashType EventType_ChangePlayerObjects = new HashType("event_change_player_objects");

        /// <summary>
        /// Type for the <c>EventListener_SavedGames</c>.
        /// </summary>
        public static HashType EventType_SavedGames = new HashType("event_saved_games");

        /// <summary>
        /// Type for the <c>EventListener_PickedObject</c>.
        /// </summary>
        public static HashType EventType_PickedObject = new HashType("event_picked_object");
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor of the <c>EventManager</c> class.
        /// </summary>
        public EventManager()
        {
            _typeList = new HashSet<HashType>();
            RegisterDefaultTypes();

            _registry = new Dictionary<int, List<IEventListener>>();

            _activeQueue = 0;
            _queues = new List<IEventData>[2];
            _queues[0] = new List<IEventData>();
            _queues[1] = new List<IEventData>();
        }

        #endregion

        #region RegisterTypes Method

        /// <summary>
        /// Register all type of events in the list of types.
        /// </summary>
        public void RegisterDefaultTypes()
        {
            EventManager.RegisterType(EventManager.EventType_LogMessage);
            EventManager.RegisterType(EventManager.EventType_UnitCollision);
            EventManager.RegisterType(EventManager.EventType_UnitStateChanged);
            EventManager.RegisterType(EventManager.EventType_UnitDirectionChanged);
            EventManager.RegisterType(EventManager.EventType_LevelChanged);
            EventManager.RegisterType(EventManager.EventType_CharactersAttack);
            EventManager.RegisterType(EventManager.EventType_ChangePlayerObjects);
            EventManager.RegisterType(EventManager.EventType_SavedGames);
            EventManager.RegisterType(EventManager.EventType_PickedObject);
        }

        #endregion

        #region AddListener Method

        /// <summary>
        /// Add a new Listener with a specified type.
        /// </summary>
        /// <param name="listener">Listener to be added.</param>
        /// <param name="eventType">Type of the events.</param>
        /// <returns>true if the listener was added, false otherwise.</returns>
        public static bool AddListener(IEventListener listener, HashType eventType)
        {
            if(!ValidateType(eventType))
                return false;
            
            // Check / Update type list
            bool eventTypeFound = _typeList.Contains(eventType);
            if (!eventTypeFound)
                _typeList.Add(eventType);

            // Find listener list entry, create one if no exists
            bool eventListenerListFound = _registry.ContainsKey(eventType.ID);
            if (!eventListenerListFound)
            {
                _registry.Add(eventType.ID, new List<IEventListener>());
            }

            // Update the list of listeners, walk the existing list
            // to prevent duplicate addition of listeners.
            List<IEventListener> listeners = _registry[eventType.ID];
            foreach (IEventListener sListener in listeners)
                if (sListener == listener)
                    return false;

            _registry[eventType.ID].Add(listener);
            
            return true;
        }

        #endregion

        #region DelListener Method

        /// <summary>
        /// Remove a listener of a specified type from the list of listeners.
        /// </summary>
        /// <param name="listener">Listener to be removed.</param>
        /// <param name="eventType">Type of the events.</param>
        /// <returns>true if the listener was removed, false otherwise.</returns>
        public static bool DelListener(IEventListener listener, HashType eventType)
        {
            if (!ValidateType(eventType))
                return false;

            if (!_registry.ContainsKey(eventType.ID))
                return false;

            foreach (IEventListener sListener in _registry[eventType.ID])
                if (sListener == listener)
                {
                    _registry[eventType.ID].Remove(listener);
                    return true;
                }

            return false;
        }

        #endregion

        #region Trigger Method

        /// <summary>
        /// Add a new event to be processed instantly.
        /// </summary>
        /// <param name="inEvent">Event to be processed.</param>
        /// <returns>true if the event was processed, false otherwise.</returns>
        public static bool Trigger(IEventData inEvent)
        { 
            if(!ValidateType(inEvent.GetEventType()))
                return false;
            
            /*if (registry.Count != 0)
            {
                foreach (IEventListener listener in registry[registry.First().Key])
                    listener.HandleEvent(inEvent);
            }*/
            
            if (!_registry.ContainsKey(inEvent.GetEventType().ID))
                return false;
            
            bool processed = false;
            foreach (IEventListener listener in _registry[inEvent.GetEventType().ID])
                if (listener.HandleEvent(inEvent))
                    processed = true;
            
            return processed;
        }

        #endregion

        #region QueueEvent Method

        /// <summary>
        /// Add a new event to the queues to be processed when it is its turn in
        /// the central loop of the event manager.
        /// </summary>
        /// <param name="inEvent">Event to be processed.</param>
        /// <returns>tur if the event was processed, false otherwise.</returns>
        public static bool QueueEvent(IEventData inEvent)
        {
            if (!(_activeQueue >= 0 && _activeQueue <= 1))
                return false;
            
            if (!ValidateType(inEvent.GetEventType()))
                return false;
            
            if (!_registry.ContainsKey(inEvent.GetEventType().ID))
            {
                if (_registry.Count == 0)
                    return false;
            }
            
            _queues[_activeQueue].Add(inEvent);
            
            return true;
        }

        #endregion

        #region AbortEvent Method

        /// <summary>
        /// Abort an existing event of the specified type.
        /// </summary>
        /// <param name="eventType">Type of the event to be aborted.</param>
        /// <param name="allOfType">true if it is necessary to abort all the events
        /// in the specified type, false otherwise.</param>
        /// <returns>true if the event was aborted, false otherwise.</returns>
        public static bool AbortEvent(HashType eventType, bool allOfType = false)
        {
            if (_activeQueue > 0 && _activeQueue < 1)
                return false;

            if (!ValidateType(eventType))
                return false;

            if (!_registry.ContainsKey(eventType.ID))
                return false;

            bool rc = false;
            foreach(IEventData sEvent in _queues[_activeQueue])
            {
                if (sEvent.GetEventType() == eventType)
                {
                    _queues[_activeQueue].Remove(sEvent);
                    rc = true;
                    if (!allOfType)
                        break;
                }
            }

            return rc;
        }

        #endregion

        #region Tick Method

        /// <summary>
        /// Process the next event of the active queue.
        /// </summary>
        /// <returns>true if the event was processed, false otherwise.</returns>
        public static bool Tick()
        {
            // Swap active queues, make sure new queue is empty after the swap
            int queueToProcess = _activeQueue;
            _activeQueue = (_activeQueue + 1) % 2;
            _queues[_activeQueue].Clear();

            // Now process as many events as we can (possibly time limited)...
            // always do at least one event, if any are available...
            while (_queues[queueToProcess].Count > 0)
            {
                IEventData sEvent = _queues[queueToProcess][0];
                _queues[queueToProcess].Remove(sEvent);
                
                // No listeners currently for this event type, skip it
                HashType type = sEvent.GetEventType();
                if (!_registry.ContainsKey(type.ID))
                    continue;

                if (_registry.ContainsKey(type.ID))
                {
                    bool processed = false;
                    //foreach (IEventListener listener in registry[registry.First().Key])
                    //    listener.HandleEvent(sEvent);

                    foreach (IEventListener listener in _registry[type.ID])
                    {
                        if (listener.HandleEvent(sEvent))
                            processed = true;
                    }
                }

                /*foreach (IEventListener listener in registry[type.ID])
                    if (listener.HandleEvent(sEvent))
                        break;*/
            }

            // If any events left to process, push them onto the active queue
            bool queueFlushed = (_queues[queueToProcess].Count == 0);
            if (!queueFlushed)
            {
                while (_queues[queueToProcess].Count > 0)
                {
                    IEventData sEvent = _queues[queueToProcess][_queues[queueToProcess].Count - 1];
                    _queues[queueToProcess].Remove(sEvent);
                    _queues[_activeQueue].Add(sEvent);
                }
            }

            // All done, this pass
            return queueFlushed;
        }

        #endregion

        #region RegisterType Method

        /// <summary>
        /// Register a new event type.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns>true if the event type was registered, false otherwise.</returns>
        public static bool RegisterType(HashType eventType)
        {
            if (eventType.StrID.Length == 0)
                return false;

            if ((eventType.ID == 0) &&
               (eventType.ID != kINFINITE))
                return false;
            
            _typeList.Add(eventType);
            
            return true;
        }

        #endregion

        #region ValidateType Method

        /// <summary>
        /// Validate the type of an event.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <returns>true if the event type is valid, false otherwise.</returns>
        public static bool ValidateType(HashType eventType)
        {
            if (eventType.StrID.Length == 0)
                return false;
            
            if ((eventType.ID == 0) &&
               (eventType.ID != kINFINITE))
                return false;
            
            if (!_typeList.Contains(eventType))
                return false;
            
            return true;
        }

        #endregion
    }
}
