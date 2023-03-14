namespace Nrrdio.Utilities.Web.Models.Errors;

public class HttpUnauthorizedError : HttpException {
	public override int StatusCode => 401;

	public HttpUnauthorizedError() : base("You are not authorized to access this resource.") { }
}
