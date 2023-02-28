using Amazon.Lambda.APIGatewayEvents;
using System.Net;
using System.Text.Json;

static class ErrorHandler
{
    public static APIGatewayProxyResponse BuildErrorResponse(HttpStatusCode statusCode, string message)
    {
        return new APIGatewayProxyResponse
        {
            StatusCode = (int)statusCode,
            Body = JsonSerializer.Serialize(new
            {
                Code = (int)statusCode,
                Details = new List<string> { message }
            }),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }

    public static APIGatewayProxyResponse BuildErrorResponse(Exception ex)
    {
        var statusCode = ex is InvalidDataException ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError;
        return new APIGatewayProxyResponse
        {
            StatusCode = (int)statusCode,
            Body = JsonSerializer.Serialize(new
            {
                Code = (int)statusCode,
                Details = new List<string> { ex.Message }
            }),
            Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
        };
    }
}
