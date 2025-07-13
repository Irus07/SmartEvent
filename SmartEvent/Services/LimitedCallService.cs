using SmartEvent.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartEvent.Services
{
    internal class LimitedCallService<TDelegate> : IHandlerCollection<TDelegate>
        where TDelegate : Delegate
    {
        private Dictionary<TDelegate, int> _limitedCallDispatcher = new Dictionary<TDelegate, int>();

        public void Add(TDelegate @delegate, ushort quantity)
        {
            _limitedCallDispatcher.Add(@delegate, quantity);
        }

        public void Clear() => _limitedCallDispatcher.Clear();



        public void Invoke(params object[]? arg)
        {
            foreach (var handler in _limitedCallDispatcher)
            {
                if (handler.Value > 0)
                {
                    handler.Key.DynamicInvoke(arg);
                    _limitedCallDispatcher[handler.Key] -= 1;
                }
            }
        }

        public void Remove(TDelegate @delegate)
        {
            int @delegateHash = @delegate.Method.GetHashCode();

            foreach (var handler in _limitedCallDispatcher.Reverse())
            {
                if (handler.Key.Method.GetHashCode() == delegateHash)
                {
                    _limitedCallDispatcher.Remove(handler.Key);
                    break;
                }
            }
        }
    }
}
