using Nrrdio.Utilities.Web.Models.Errors;

namespace Nrrdio.Utilities.Web;

public class HttpErrorSelector {
	public static Type Get(int code) {
		return code switch {
			400 => typeof(HttpBadRequestError),
			401 => typeof(HttpUnauthorizedError),
			403 => typeof(HttpForbiddenError),
			404 => typeof(HttpNotFoundError),
			408 => typeof(HttpTimeoutError),
			418 => typeof(HttpTeapotError),
			500 => typeof(HttpInternalServerError),
			_ => typeof(HttpException)
		};
	}
}
