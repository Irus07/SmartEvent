namespace SmartEvent.Interfaces
{
    internal interface IHandlerCollection<TDelegate> where TDelegate : Delegate
    {
        void Add(TDelegate @delegate, ushort priority);
        void Clear();
        void Invoke(params object[]? arg);
        void Remove(TDelegate @delegate);
    }
}