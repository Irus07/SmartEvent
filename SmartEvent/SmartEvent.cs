using System.Reflection;
using System.Runtime.CompilerServices;

namespace SmartEvent.Classes
{
    public partial class SmartEvent<TDelegate>
        where TDelegate : Delegate
    {
        #region Constructors
        public SmartEvent(params TDelegate[] delegates)
        {

        }
        public SmartEvent()
        {

        }
        #endregion

        private List<Type> _sortedDelegate = new List<Type>();
        private List<Type> _delegate = new List<Type>();
        private List<Type> _pinned = new List<Type>();
        private Dictionary<TDelegate, int> _InvokeMultiple = new Dictionary<TDelegate, int>();
        private int _count = 0;



        public void Subscribe(TDelegate @delegate)
        {
            Subscribe(@delegate, 5);
        }
        public void Subscribe(TDelegate @delegate, ushort priority)
        {

            _delegate.Add(
                new Type(
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

        public void Unsubscribe(TDelegate @delegate)
        {
            var rez = from t in _delegate
                      where t.hashCode != @delegate.GetHashCode()
                      select t;

            _delegate = rez.ToList();

            Sort();

        }
        public void UnsubscribeAll()
        {
            _sortedDelegate = new List<Type> { };
            _delegate = new List<Type>();
        }
        public void AddToMultiQueue(TDelegate @delegate, ushort quantity = 1)
        {
            if (quantity == 0)
                return;

            _InvokeMultiple.Add(@delegate, quantity);
        }

        public void Invoke(params object[]? TDelegateParameters)
        {
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
            {
                item.@delegate.DynamicInvoke(TDelegateParameters);
            }
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
