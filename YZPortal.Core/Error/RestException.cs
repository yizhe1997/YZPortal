using System.Net;

namespace YZPortal.Core.Error
{
    public class RestException : Exception
    {
        public HttpStatusCode Code { get; }
        public string Error { get; }

        public RestException(HttpStatusCode code, List<string> errors = null)
        {
            Code = code;
            Error = errors == null ? code.ToString() : string.Join(",", errors);
        }

        public RestException(HttpStatusCode code, string message)
        {
            Code = code;
            Error = message;
        }

        public RestException(HttpStatusCode code)
        {
            Code = code;
            Error = code.ToString();
        }
    }
}
