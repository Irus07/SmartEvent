namespace SmartEvent.Interfaces
{
    public interface ISmartEvent<TDelegate> where TDelegate : Delegate
    {
        void Invoke(params object[]? TDelegateParameters);
        void Subscribe(TDelegate @delegate);
        void Unsubscribe(TDelegate @delegate);
    }
}