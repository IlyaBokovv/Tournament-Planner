using Entities.ErrorModel;
using TournamentPlanner.Exceptions;
using TournamentPlanner.Services.Exceptions;

namespace TournamentPlanner.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (MaxMatchesCountReachedException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new ErrorDetails<object> { Success = false, Message = ex.Message });
            }
            catch (MatchNotFoundIdRequestExeption ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(new ErrorDetails<object> { Success = false, Message = ex.Message });
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new ErrorDetails<object> { Success = false });
            }
        }
    }
}
