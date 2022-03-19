using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace PaymentsDashboard.Helpers
{
	public static class HttpContextHelpers
	{
		public static string GetUserId(this HttpContext context)
		{
			return context.User.FindFirstValue(ClaimTypes.NameIdentifier);
		}
	}
}
