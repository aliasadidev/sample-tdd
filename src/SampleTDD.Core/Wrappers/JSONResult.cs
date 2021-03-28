namespace SampleTDD.Core.Wrappers
{
	public class JSONResult<T> : JSONResultWrapper<T> where T : class
	{

		public JSONResult(
		  T data = null,
		  int statusCode = 200,
		  string message = "",
		  string errorDetail = "",
		  string autoRedirectUrl = null,
		  bool autoRedirect = false)
		{
			Item = data;
			StatusCode = statusCode;
			Message = message;
			ErrorDetail = errorDetail;
			AutoRedirect = autoRedirect;
			AutoRedirectURL = autoRedirectUrl;
		}

	}
}
