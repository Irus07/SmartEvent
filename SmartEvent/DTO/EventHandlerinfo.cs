namespace SmartEvent.DTO
{
    internal record class EventHandlerinfo<TDelegate>(TDelegate @delegate, int hashCode);
}
