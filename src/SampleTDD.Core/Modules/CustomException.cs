using System;
using System.Net;

namespace SampleTDD.Core.Modules
{
	public class CustomException : Exception
	{
		protected int _statusCode;
		public string AutoRedirectURL { get; }

		public int StatusCode => _statusCode;
		public HttpStatusCode HttpStatusCode { get; }
		public CustomException(string message, int statusCode, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError, string autoRedirectURL = null) : base(message)
		{
			_statusCode = statusCode;
			HttpStatusCode = httpStatusCode;
			this.AutoRedirectURL = autoRedirectURL;
		}
	}
}


