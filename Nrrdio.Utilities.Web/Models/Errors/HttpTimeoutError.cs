namespace Nrrdio.Utilities.Web.Models.Errors {
    public class HttpTimeoutError : HttpException {
        public override int StatusCode => 408;

        public HttpTimeoutError() : base("The request timed out while attempting to access this resource.") { }
    }
}
