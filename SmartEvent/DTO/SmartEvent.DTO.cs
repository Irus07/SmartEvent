namespace SmartEvent
{
	public partial class SmartEvent<TDelegate>
	{
		private record class Type(TDelegate @delegate, int hashCode, int index, ushort priority)
		{
		}
	}
}
