using System.Collections.Generic;

namespace SampleTDD.Core.Wrappers
{
	public class JSONResultList<T> : JSONResultListWrapper<T> where T : class
	{
		public JSONResultList() { }
		public JSONResultList(
		  IEnumerable<T> data = null,
		  int statusCode = 200,
		  string message = "",
		  string errorDetail = "",
		  string autoRedirectUrl = null,
		  bool autoRedirect = false)
		{
			Items = data;
			StatusCode = statusCode;
			Message = message;
			ErrorDetail = errorDetail;
			AutoRedirect = autoRedirect;
			AutoRedirectURL = autoRedirectUrl;
		}
	}
}
