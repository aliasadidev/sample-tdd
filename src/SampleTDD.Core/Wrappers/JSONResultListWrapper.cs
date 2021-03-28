using System.Collections.Generic;

namespace SampleTDD.Core.Wrappers
{
	public class JSONResultListWrapper<T> : JSONResultBase
	{
		public IEnumerable<T> Items { get; set; }
	}
}
