namespace Beeline.Tests.Helpers
{
	using System;
	using System.Collections.Specialized;
	using System.Web;
	using System.Web.Mvc;

	public class HttpContextStub : HttpContextBase
	{
		private readonly HttpRequestStub _request;
		private readonly HttpResponseStub _response;

		public HttpContextStub(String appPath = "/", String requestUrl = "~/", HttpVerbs method = HttpVerbs.Get)
		{
			_request = new HttpRequestStub(appPath, requestUrl, method);
			_response = new HttpResponseStub();
		}

		public override HttpRequestBase Request
		{
			get { return _request; }
		}

		public override HttpResponseBase Response
		{
			get { return _response; }
		}
	}

	public class HttpRequestStub : HttpRequestBase
	{
		private readonly String _appPath;
		private readonly String _requestUrl;
		private readonly HttpVerbs _method;

		public HttpRequestStub(String appPath, String requestUrl, HttpVerbs method)
		{
			_appPath = appPath;
			_requestUrl = requestUrl;
			_method = method;
		}

		public override String ApplicationPath
		{
			get { return _appPath; }
		}

		public override String AppRelativeCurrentExecutionFilePath
		{
			get { return _requestUrl; }
		}

		public override String PathInfo
		{
			get { return String.Empty; }
		}

		public override NameValueCollection ServerVariables
		{
			get { return new NameValueCollection(); }
		}

		public override string HttpMethod
		{
			get { return _method.ToString().ToUpper(); }
		}
	}

	public class HttpResponseStub : HttpResponseBase
	{
		public override String ApplyAppPathModifier(String virtualPath)
		{
			return virtualPath;
		}
	}
}
