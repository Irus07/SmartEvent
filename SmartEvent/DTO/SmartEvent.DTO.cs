namespace SmartEvent
{
	public partial class SmartEvent<TDelegate>
	{
		internal record class EventHandlerType(TDelegate @delegate, int hashCode, int index, ushort priority)
		{
		}
	}
}
