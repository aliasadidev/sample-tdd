using SampleTDD.Core.DTOs.Security;

namespace SampleTDD.Core.Wrappers
{
	public abstract class JSONResultBase
	{
		public string Message { get; set; }
		public int StatusCode { get; set; }
		public string ErrorDetail { get; set; }
		public bool AutoRedirect { get; set; }
		public string AutoRedirectURL { get; set; }
	}
}
