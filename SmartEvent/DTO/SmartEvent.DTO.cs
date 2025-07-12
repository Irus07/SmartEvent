using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEvent.Classes
{
	public partial class SmartEvent<TDelegate>
	{
		private record class Type(TDelegate @delegate, int hashCode, int index, ushort priority)
		{
		}
	}
}
