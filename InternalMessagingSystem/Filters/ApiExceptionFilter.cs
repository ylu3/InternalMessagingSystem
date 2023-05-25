using InternalMessagingSystem.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace InternalMessagingSystem.Filters
{
    /// <summary>
    /// Exception filter for handling API exceptions and returning appropriate JSON responses.
    /// </summary>
    public class ApiExceptionFilter : IAsyncExceptionFilter
    {
        /// <summary>
        /// Handles the exception asynchronously and returns an appropriate JSON response.
        /// </summary>
        /// <param name="context">The exception context.</param>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;

            var response = context.HttpContext.Response;
            response.ContentType = "application/json";

            // Set the appropriate status code based on the exception type
            response.StatusCode = ex switch
            {
                UserNotFoundException _ or MessageNotFoundException _ => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };

            // Create a JSON response containing the error message
            context.Result = new JsonResult(new
            {
                error = new
                {
                    message = ex.Message
                }
            });

            // Mark the exception as handled
            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }
    }
}
