using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DTL23_API.Attributes {
	internal class GenerateAntiforgeryTokenCookieAttribute : ResultFilterAttribute {
		public override void OnResultExecuting(ResultExecutingContext context) {
			var antiforgery = context.HttpContext.RequestServices.GetService<IAntiforgery>();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
			var tokens = antiforgery.GetAndStoreTokens(context.HttpContext);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
			context.HttpContext.Response.Cookies.Append(
				"RequestVerificationToken",
				tokens.RequestToken,
				new CookieOptions() { HttpOnly = false });
#pragma warning restore CS8604 // Possible null reference argument.
		}

		public override void OnResultExecuted(ResultExecutedContext context) {
			
		}
	}
}
