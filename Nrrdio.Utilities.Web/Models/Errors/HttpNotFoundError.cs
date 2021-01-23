namespace Nrrdio.Utilities.Web.Models.Errors {
    public class HttpNotFoundError : HttpException {
        public override int StatusCode => 404;

        public HttpNotFoundError() : base("This resource was not found.") { }
    }
}
