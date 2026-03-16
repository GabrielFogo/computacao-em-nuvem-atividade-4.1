using Microsoft.AspNetCore.Mvc;

public class ApiExceptionFilter : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (Exception exception)
        {
            var status = StatusCodes.Status500InternalServerError;

            var problemDetails = new ProblemDetails
            {
                Status = status,
                Title = "Ocorreu um problema ao tratar a sua solicitação",
                Detail = exception.Message,
                Instance = context.HttpContext.Request.Path,
                Type = exception.GetType().Name
            };

            // retorna os detalhes da exception quando a estoura uma exception
            return Results.Problem(
                detail: problemDetails.Detail,
                title: problemDetails.Title,
                statusCode: problemDetails.Status,
                instance: problemDetails.Instance,
                type: problemDetails.Type
            );
        }
    }
}