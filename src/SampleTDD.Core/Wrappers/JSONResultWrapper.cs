namespace SampleTDD.Core.Wrappers
{
	public class JSONResultWrapper<T> : JSONResultBase
	{
		public T Item { get; set; }
	}
}
