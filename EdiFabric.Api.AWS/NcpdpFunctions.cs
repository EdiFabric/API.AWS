using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using EdiFabric.Api;

public class NcpdpFunctions
{
    private static EdiFunctions _ediFunctions;
    static NcpdpFunctions()
    {
        _ediFunctions = new EdiFunctions(EdiFabricServices.Get<INcpdpService>());
    }

    public async Task<APIGatewayProxyResponse> ReadNcpdpAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Read(request, context.Logger);
    }

    public async Task<APIGatewayProxyResponse> WriteNcpdpAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Write(request, context.Logger);
    }

    public async Task<APIGatewayProxyResponse> ValidateNcpdpAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Validate(request, context.Logger);
    }
    /// <summary>
    /// This is a system operation used only for the in-house web translator.
    /// </summary>
    public async Task<APIGatewayProxyResponse> AnalyzeNcpdpAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Analyze(request, context.Logger);
    }
}
