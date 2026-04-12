using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using application.DTOs;

namespace WebApplication11.Filters;

// IActionFilter gives us two hooks:
//   OnActionExecuting  — runs BEFORE the controller action
//   OnActionExecuted   — runs AFTER the controller action returns
// We only need OnActionExecuted to wrap the result
public class ResponseWrapperFilter : IActionFilter
{
    // Runs before the controller action — we don't need to do anything here
    // but the interface requires us to implement it
    public void OnActionExecuting(ActionExecutingContext context) { }

    // Runs after every controller action completes
    // context.Result = whatever the controller returned (OkObjectResult, NotFoundResult etc.)
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // If an exception was thrown, let ErrorHandlingMiddleware deal with it
        // We only handle successful results here
        if (context.Exception != null) return;

        // Extract the HTTP status code and data from whatever the controller returned
        switch (context.Result)
        {
            // OkObjectResult = return Ok(someData)  → 200 with a body
            case OkObjectResult ok:
                // Wrap the data in ApiResponse and replace the result
                context.Result = new ObjectResult(
                    ApiResponse<object>.Ok(ok.Value!))
                {
                    StatusCode = 200
                };
                break;

            // CreatedAtActionResult / CreatedResult = return Created(...)  → 201
            case CreatedAtActionResult created:
                context.Result = new ObjectResult(
                    ApiResponse<object>.Created(created.Value!))
                {
                    StatusCode = 201
                };
                break;

            // NoContentResult = return NoContent()  → 204 (DELETE success)
            // 204 normally has no body, but we add one for consistency
            case NoContentResult:
                context.Result = new ObjectResult(
                    ApiResponse.NoContent())
                {
                    StatusCode = 200 // changed to 200 so body is visible to clients
                };
                break;

            // NotFoundResult = return NotFound()  → 404 with no message
            case NotFoundResult:
                context.Result = new ObjectResult(
                    ApiResponse<object>.NotFound("Resource not found"))
                {
                    StatusCode = 404
                };
                break;

            // NotFoundObjectResult = return NotFound("Venue not found")  → 404 with message
            case NotFoundObjectResult notFoundObj:
                context.Result = new ObjectResult(
                    ApiResponse<object>.NotFound(notFoundObj.Value?.ToString() ?? "Resource not found"))
                {
                    StatusCode = 404
                };
                break;

            // BadRequestObjectResult = return BadRequest(...)  → 400
            // This also catches ASP.NET model validation errors automatically
            case BadRequestObjectResult badReq:
                // Model validation errors come as ValidationProblemDetails
                // We extract all field errors and join them into one message
                var badMessage = badReq.Value is ValidationProblemDetails vpd
                    ? string.Join("; ", vpd.Errors.SelectMany(e =>
                        e.Value.Select(msg => $"{e.Key}: {msg}")))
                    : badReq.Value?.ToString() ?? "Bad request";

                context.Result = new ObjectResult(
                    ApiResponse<object>.BadRequest(badMessage))
                {
                    StatusCode = 400
                };
                break;

            // ObjectResult is the base for all results that have a body
            // Catches anything not matched above (e.g. return StatusCode(422, data))
            case ObjectResult obj when obj.StatusCode >= 400:
                context.Result = new ObjectResult(
                    ApiResponse<object>.ServerError(obj.Value?.ToString() ?? "Error"))
                {
                    StatusCode = obj.StatusCode
                };
                break;
        }
    }
}