using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using application.DTOs;

namespace WebApplication11.Filters;

public class ResponseWrapperFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null) return;

        switch (context.Result)
        {
            case OkObjectResult ok:
                context.Result = new ObjectResult(
                    ApiResponse<object>.Ok(ok.Value!))
                {
                    StatusCode = 200
                };
                break;

            case CreatedAtActionResult created:
                context.Result = new ObjectResult(
                    ApiResponse<object>.Created(created.Value!))
                {
                    StatusCode = 201
                };
                break;

            case CreatedResult created:
                context.Result = new ObjectResult(
                    ApiResponse<object>.Created(created.Value!))
                {
                    StatusCode = 201
                };
                break;

            case NoContentResult:
                context.Result = new ObjectResult(
                    ApiResponse.NoContent())
                {
                    // Return a body with the standard wrapper instead of empty 204 responses.
                    StatusCode = 200
                };
                break;

            case NotFoundResult:
                context.Result = new ObjectResult(
                    ApiResponse<object>.NotFound("Resource not found"))
                {
                    StatusCode = 404
                };
                break;

            case NotFoundObjectResult notFoundObj:
                context.Result = new ObjectResult(
                    ApiResponse<object>.NotFound(notFoundObj.Value?.ToString() ?? "Resource not found"))
                {
                    StatusCode = 404
                };
                break;

            case BadRequestObjectResult badReq:
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