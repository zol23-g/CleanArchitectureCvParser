// using Core.Common;
// using Microsoft.Extensions.Logging;
// using System.Net;
// using System.Text.Json;

// namespace Infrastructure.Middleware
// {
//     public class ExceptionHandlingMiddleware
//     {
//         private readonly RequestDelegate _next;
//         private readonly ILogger<ExceptionHandlingMiddleware> _logger;

//         public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
//         {
//             _next = next;
//             _logger = logger;
//         }

//         public async Task InvokeAsync(HttpContext context)
//         {
//             try
//             {
//                 await _next(context);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, "Unhandled exception");

//                 context.Response.ContentType = "application/json";
//                 context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

//                 var response = ApiResponse<string>.Fail("An unexpected error occurred.");
//                 var json = JsonSerializer.Serialize(response);

//                 await context.Response.WriteAsync(json);
//             }
//         }
//     }

//     internal class RequestDelegate
//     {
//     }
// }
