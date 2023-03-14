namespace Nrrdio.Utilities.Web.Models.Errors;

public class HttpForbiddenError : HttpException {
	public override int StatusCode => 403;

	public HttpForbiddenError() : base("You are forbidden from accessing this resource.") { }
}
