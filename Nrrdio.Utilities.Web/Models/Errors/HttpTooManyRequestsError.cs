namespace Nrrdio.Utilities.Web.Models.Errors;

public class HttpTooManyRequestsError : HttpException {
	public override int StatusCode => 429;

	public HttpTooManyRequestsError() : base("Either you are sending requests too quickly or the servers are experiencing high traffic.") { }
}
