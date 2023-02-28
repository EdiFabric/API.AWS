using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using EdiFabric.Api;

public class Hl7Functions
{
    private static EdiFunctions _ediFunctions;
    static Hl7Functions()
    {
        _ediFunctions = new EdiFunctions(EdiFabricServices.Get<IHl7Service>());
    }

    public async Task<APIGatewayProxyResponse> ReadHl7Async(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Read(request, context.Logger);
    }

    public async Task<APIGatewayProxyResponse> WriteHl7Async(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Write(request, context.Logger);
    }

    public async Task<APIGatewayProxyResponse> ValidateHl7Async(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Validate(request, context.Logger);
    }
    /// <summary>
    /// This is a system operation used only for the in-house web translator.
    /// </summary>
    public async Task<APIGatewayProxyResponse> AnalyzeHl7Async(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Analyze(request, context.Logger);
    }
}
