using SmartEvent.DTO;


namespace SmartEvent
{
	public class HandlerRegistry<TDelegate>
		where TDelegate : Delegate
	{
		private SortedList<(int priority, int position),
			EventHandlerinfo<TDelegate>>
			_sortedHandlers = new SortedList<(int priority, int position),
				EventHandlerinfo<TDelegate>>();

		private int _positionCounter = 0;

		public void AddHandler(TDelegate @delegate, ushort priority)
		{
			var x = (10 - priority, _positionCounter);
			var y = new EventHandlerinfo<TDelegate>(@delegate, @delegate.Method.GetHashCode());
			_sortedHandlers.Add(x, y);

			_positionCounter++;
		}

		public void Invoke(params object[]? arg)
		{
			foreach (var handler in _sortedHandlers)
				handler.Value.@delegate.DynamicInvoke(arg);
		}
	}
}
