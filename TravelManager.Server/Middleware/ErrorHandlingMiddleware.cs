using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace TravelManager.Server.Middleware
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ErrorHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			var result = new
			{
				success = false,
				message = "Ocorreu um erro no servidor.",
				error = exception.Message
			};

			return context.Response.WriteAsJsonAsync(result);
		}
	}
}