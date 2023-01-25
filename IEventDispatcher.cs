using System.Collections;
using System.Collections.Generic;
using Core.EventDispatcher;
using UnityEngine;

namespace Core.EventDispatcher
{
	public delegate void EventDispatcherDelegate(IEventDispatcher eventData = null);

	/// <summary>
	/// An interface for implementing simple Event Dispatcher
	/// </summary>
	public interface IEventDispatcher
	{
		/// <summary>
		/// Add event to the event dispatcher
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="callback"></param>
		void AddEventToListener(EEventName eventName, EventDispatcherDelegate callback);

		/// <summary>
		/// Remove Event from event dispatcher
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="callback"></param>
		void RemoveEventFromListener(EEventName eventName, EventDispatcherDelegate callback);

		/// <summary>
		/// Dispatch Event to event listnerers
		/// </summary>
		/// <param name="eventName"></param>
		/// <param name="eventData"></param>
		void Dispatch(EEventName eventName, IEventDispatcher eventData);

		/// <summary>
		/// Removes all events from this listener
		/// </summary>
		/// <param name="type"></param>
		void RemoveAllEvents(EEventName type = EEventName.NONE);
	}
}