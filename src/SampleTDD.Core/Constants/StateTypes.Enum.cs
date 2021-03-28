using System.ComponentModel;

namespace SampleTDD.Core.Constants
{
	/// <summary>
	/// System states
	/// </summary>
	public enum StateTypes : int
	{
		/// <summary>
		///  Start
		/// </summary>
		[Description("Start")]
		Start = 1,

		/// <summary>
		///  Request New Job
		/// </summary>
		[Description("Request New Job")]
		RequestNewJob = 2,

		/// <summary>
		/// Finished Successfully
		/// </summary>
		[Description("Finished Successfully")]
		FinishedSuccessfully = 100,

		/// <summary>
		/// Finished Unsuccessfully
		/// </summary>
		[Description("Finished Unsuccessfully")]
		FinishedUnsuccessfully = 101,

		/// <summary>
		///Closed
		/// </summary>
		[Description("Closed")]
		Closed = 102,
	}
}
