using SmartEvent.DTO;
using SmartEvent.Interfaces;
using System.Collections.Generic;


namespace SmartEvent.Services
{
    internal class HandlerRegistryService<TDelegate> : IHandlerCollection<TDelegate> where TDelegate : Delegate
    {
        private SortedList<(int priority, int position),
            EventHandlerinfo<TDelegate>>
            _sortedHandlers = new SortedList<(int priority, int position),
                EventHandlerinfo<TDelegate>>();

        private int _positionCounter = 0;

        public void Add(TDelegate @delegate, ushort priority)
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

        public void Remove(TDelegate @delegate)
        {
            foreach (var item in _sortedHandlers.Reverse())
            {
                if (item.Value.hashCode == @delegate.Method.GetHashCode())
                {
                    _sortedHandlers.Remove(item.Key);
                    break;
                }
            }
        }
        public void Clear() => _sortedHandlers.Clear();
    }
}
