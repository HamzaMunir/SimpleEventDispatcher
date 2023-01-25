using System.Collections.Generic;

namespace Core.EventDispatcher
{
    /// <summary>
    /// Base Event Dispatcher class that can be inherited by objects
    /// </summary>
    public class EventDispatcher : IEventDispatcher
    {
        private readonly Dictionary<EEventName, List<EventDispatcherDelegate>> m_listeners =
            new Dictionary<EEventName, List<EventDispatcherDelegate>>();

        /// <summary>
        /// public method to add event to this listener
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public void AddEventToListener(EEventName eventName, EventDispatcherDelegate callback)
        {
            AddEventToListenerInternal(eventName, callback);
        }

        /// <summary>
        /// public method to remove event from this listener
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public void RemoveEventFromListener(EEventName eventName, EventDispatcherDelegate callback)
        {
            RemoveEventFromListenerInternal(eventName, callback);
        }

        /// <summary>
        /// public method to dispatch <see cref="eventName" /> with <see cref="eventData"/>
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventData"></param>
        public void Dispatch(EEventName eventName, IEventDispatcher eventData)
        {
            DispatchInternal(eventName, eventData);
        }

        /// <summary>
        /// Remove all events from this event listener of given <see cref="type"/>
        /// </summary>
        /// <param name="type"></param>
        public void RemoveAllEvents(EEventName type = EEventName.NONE)
        {
            RemoveAllEventsInternal(type);
        }
        
        
        private void AddEventToListenerInternal(EEventName evtName, EventDispatcherDelegate callback)
        {
            List<EventDispatcherDelegate> evtListeners = null;
            if (m_listeners.TryGetValue(evtName, out evtListeners))
            {
                evtListeners.Remove(callback); //make sure we dont add duplicate
                evtListeners.Add(callback);
            }
            else
            {
                evtListeners = new List<EventDispatcherDelegate>();
                evtListeners.Add(callback);

                m_listeners.Add(evtName, evtListeners);
            }
        }

        private void RemoveEventFromListenerInternal(EEventName evtName, EventDispatcherDelegate callback)
        {
            List<EventDispatcherDelegate> evtListeners = null;
            if (m_listeners.TryGetValue(evtName, out evtListeners))
            {
                for (int i = 0; i < evtListeners.Count; i++)
                {
                    evtListeners.Remove(callback);
                }
            }
        }

        /// <summary>
        /// Removes all event listeners with a certain <see cref="type"/>, or all of them if type is null. 
        ///  Be careful when removing all event listeners: you never know who else was listening.
        /// </summary>
        /// <param name="type"></param>
        private void RemoveAllEventsInternal(EEventName type = EEventName.NONE)
        {
            if (type == EEventName.NONE && m_listeners != null)
                m_listeners.Remove(type);
            else
                m_listeners.Clear();
        }

        private void DispatchInternal(EEventName evtName, IEventDispatcher evt = null)
        {
            //FIXME: might need to COPY the list<dispatchers> here so that an 
            //	event listener that results in adding/removing listeners does 
            //	not invalidate this for loop

            List<EventDispatcherDelegate> evtListeners = null;
            var CopyOfListeners = m_listeners;
            if (CopyOfListeners.TryGetValue(evtName, out evtListeners))
            {
                List<EventDispatcherDelegate> temp = new List<EventDispatcherDelegate>(evtListeners);
                for (int i = 0; i < temp.Count; i++)
                {
                    temp[i](evt);
                }
            }
        }

        /// <summary>
        /// Returns if there are listeners registered for a certain event type. 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasEventListener(EEventName type)
        {
            return m_listeners.ContainsKey(type);
        }
        
    }
}