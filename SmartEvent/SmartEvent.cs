using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using SmartEvent.Interfaces;
using SmartEvent.Services;


namespace SmartEvent
{
    public partial class SmartEvent<TDelegate> 
		where TDelegate : Delegate
	{

		private readonly HandlerRegistryService<TDelegate> _handlerRegistry = new();
		private readonly LimitedCallService<TDelegate> _limitedCall = new();

		/// <summary>
		/// Subscribes a method to an event indicating the priority of the call
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="priority">Sets the priority of the call inside the event</param>
		public void Subscribe(TDelegate @delegate, ushort priority = 5)
		{
			if(priority > 9)
				priority= 9;

			_handlerRegistry.Add(@delegate, priority);
		}


		
		/// <summary>
		/// Deletes the passed handler
		/// </summary>
		/// <param name="delegate"></param>
		public void Unsubscribe(TDelegate @delegate)
		{
			_handlerRegistry.Remove(@delegate);

		}
		/// <summary>
		/// Deletes all handlers.Does not affect pinned handlers
		/// </summary>
		public void UnsubscribeAll()
		{
			_handlerRegistry.Clear();
			_limitedCall.Clear();

		}
		/// <summary>
		/// Adds a handler with a limited number of calls
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="quantity">Number of calls</param>
		public void AddToMultiQueue(TDelegate @delegate, ushort quantity = 1)
		{
			_limitedCall.Add(@delegate, quantity);
		}

		/// <summary>
		/// Calls signed handlers
		/// </summary>
		/// <param name="TDelegateParameters">Event call parameters</param>
		public void Invoke(params object[]? TDelegateParameters)
		{
			_limitedCall.Invoke(TDelegateParameters);
			_handlerRegistry.Invoke(TDelegateParameters);

		}

		public static SmartEvent<TDelegate> operator +(SmartEvent<TDelegate> smartEvent, TDelegate @delegate)
		{
			smartEvent.Subscribe(@delegate);
			return smartEvent;
		}
		public static SmartEvent<TDelegate> operator -(SmartEvent<TDelegate> smartEvent, TDelegate @delegate)
		{
			smartEvent.Unsubscribe(@delegate);
			return smartEvent;
		}
	}
}
