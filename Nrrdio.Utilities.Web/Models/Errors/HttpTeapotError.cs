namespace Nrrdio.Utilities.Web.Models.Errors {
    // https://tools.ietf.org/html/rfc2324
    // https://tools.ietf.org/html/rfc7168
    public class HttpTeapotError : HttpException {
        public override int StatusCode => 418;

        public HttpTeapotError() : base("I'm a teapot.") { }
    }
}
