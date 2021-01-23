using System;
using System.Text.Json;

namespace Nrrdio.Utilities.Web.Models.Errors {
    public partial class HttpException : ApplicationException {
        public virtual int StatusCode { get; }
        public string ContentType { get; } = "text/plain";

        public HttpException(string message) : base(message) { }
        public HttpException(string message, Exception inner) : base(message, inner) { }
        public HttpException(JsonElement errorObject) : this(errorObject.ToString()) => ContentType = "application/json";
    }
}
