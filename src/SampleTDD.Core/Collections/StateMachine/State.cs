using SampleTDD.Core.Constants;

namespace SampleTDD.Core.Collections.StateMachine
{
	public class State : BaseCollection
	{
		public StateTypes ID { get; set; }
		public string EnglishTitle { get; set; }
		public string HCode { get; set; }
	}
}
