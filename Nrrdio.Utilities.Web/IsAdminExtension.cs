using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Nrrdio.Utilities.Web;

public static class IsAdminExtension {
	public static async Task<bool> IsAdmin(this IAuthorizationService authorizationService, ClaimsPrincipal user) {
		var check = await authorizationService.AuthorizeAsync(user, "Admin");
		return check.Succeeded;
	}

	public static async Task<bool> IsParent(this IAuthorizationService authorizationService, ClaimsPrincipal user) {
		var check = await authorizationService.AuthorizeAsync(user, "Parent");
		return check.Succeeded;
	}
}
