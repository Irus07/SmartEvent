using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using SmartEvent.Interfaces;


namespace SmartEvent
{
	public partial class SmartEvent<TDelegate> : ISmartEvent<TDelegate>
		where TDelegate : Delegate
	{

		private List<EventHandlerType> _sortedDelegate = new List<EventHandlerType>();
		private List<EventHandlerType> _delegate = new List<EventHandlerType>();
		private List<EventHandlerType> _pinned = new List<EventHandlerType>();
		private Dictionary<TDelegate, int> _InvokeMultiple = new Dictionary<TDelegate, int>();
		private int _count = 0;

		private readonly HandlerRegistry<TDelegate> _handlerRegistry = new HandlerRegistry<TDelegate>();



		/// <summary>
		/// Subscribes a method to an event
		/// </summary>
		/// <param name="delegate"></param>
		public void Subscribe(TDelegate @delegate)
		{
			Subscribe(@delegate, 5);
		}
		/// <summary>
		/// Subscribes a method to an event indicating the priority of the call
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="priority">Sets the priority of the call inside the event</param>
		public void Subscribe(TDelegate @delegate, ushort priority)
		{

			_delegate.Add(
				new EventHandlerType(
					@delegate,
					@delegate.GetMethodInfo().GetHashCode(),
					_count,
					priority));


			_count++;

			Sort();
		}

		private void Sort()
		{
			var rez = from t in _delegate
					  orderby t.index ascending
					  orderby t.priority descending
					  select t;

			_sortedDelegate = rez.ToList();
		}
		/// <summary>
		/// Deletes the passed handler
		/// </summary>
		/// <param name="delegate"></param>
		public void Unsubscribe(TDelegate @delegate)
		{
			var rez = from t in _delegate
					  where t.hashCode != @delegate.GetHashCode()
					  select t;

			_delegate = rez.ToList();

			Sort();

		}
		/// <summary>
		/// Deletes all handlers.Does not affect pinned handlers
		/// </summary>
		public void UnsubscribeAll()
		{
			_sortedDelegate = new List<EventHandlerType> { };
			_delegate = new List<EventHandlerType>();
		}
		/// <summary>
		/// Adds a handler with a limited number of calls
		/// </summary>
		/// <param name="delegate"></param>
		/// <param name="quantity">Number of calls</param>
		public void AddToMultiQueue(TDelegate @delegate, ushort quantity = 1)
		{
			if (quantity == 0)
				return;

			_InvokeMultiple.Add(@delegate, quantity);
		}
		/// <summary>
		/// Calls signed handlers
		/// </summary>
		/// <param name="TDelegateParameters">Event call parameters</param>
		public void Invoke(params object[]? TDelegateParameters)
		{

			foreach (var t in _pinned)
				t.@delegate.DynamicInvoke(TDelegateParameters);


			foreach (var item in _InvokeMultiple)
			{
				if (item.Value > 0)
				{
					item.Key.DynamicInvoke(TDelegateParameters);
					_InvokeMultiple[item.Key] = item.Value - 1;
				}
				else
				{
					_InvokeMultiple.Remove(item.Key);
				}
			}

			foreach (var item in _sortedDelegate)
				item.@delegate.DynamicInvoke(TDelegateParameters);
		}

		public void ParalelInvoke(params object[]? TDelegateParameters)
		{
			var rez =
				from t in _sortedDelegate.AsParallel()
				select t.@delegate.DynamicInvoke(TDelegateParameters);

		}


		public void Pin(TDelegate @delegate)
		{
			_pinned.Add(new EventHandlerType(
				@delegate,
				@delegate.GetHashCode(),
				1,
				1
				));
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
