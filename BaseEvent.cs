using System.Collections.Generic;

namespace Core.EventDispatcher
{
    public enum EEventName
    {
        NONE,
        COMPLETE,
        CANCEL,
    }

    public class BaseEvent<T> where T : BaseEventData
    {
        private static List<BaseEvent<T>> eventPool = new List<BaseEvent<T>>();

        private EventDispatcher mTargetDispatcher;
        private EventDispatcher mCurrentTarget;
        private EEventName mName;
        private T mData;

        public BaseEvent(EEventName name, T data)
        {
            mName = name;
            mData = data;
        }


        /// <summary>
        /// The object the event is dispatched by
        /// </summary>
        public EventDispatcher TargetDispatcher => mTargetDispatcher;

        /// <summary>
        /// The object the event is currently attached to
        /// </summary>
        public EventDispatcher CurrentTarget => mCurrentTarget;

        /// <summary>
        /// Event name 
        /// </summary>
        public EEventName Name => mName;

        /// <summary>
        /// Arbitrary data that is attached to the event.
        /// </summary>
        public T Data
        {
            get => mData;
            set => mData = value;
        }

        /// <summary>
        /// Get event from pool
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public BaseEvent<T> FromPool(EEventName type, T data)
        {
            return FromPoolInternal(type, data);
        }

        /// <summary>
        /// Add event to the event pool
        /// </summary>
        /// <param name="events"></param>
        public void ToPool(BaseEvent<T> events)
        {
            ToPoolInternal(events);
        }

        /// <summary>
        /// Dispose of any memory used by this event
        /// </summary>
        public void Dispose()
        {
            eventPool.Clear();
        }
        
        private static BaseEvent<T> FromPoolInternal(EEventName type, T data)
        {
            if (eventPool.Count > 0)
            {
                BaseEvent<T> obj = eventPool[eventPool.Count];
                obj.ResetInternal(type, data);
                eventPool.RemoveAt(eventPool.Count);
                return obj;
            }

            return new BaseEvent<T>(type, data);
        }
        
        private static void ToPoolInternal(BaseEvent<T> events)
        {
            events.mData = null;
            events.mTargetDispatcher = events.mCurrentTarget = null;
            eventPool[eventPool.Count] = events; // avoiding "push" 
        }
        
        private void ResetInternal(EEventName type, T data = null)
        {
            mName = type;
            mData = data;
            mTargetDispatcher = mCurrentTarget = null;
        }
    }
}