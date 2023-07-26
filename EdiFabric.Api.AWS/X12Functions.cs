using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using EdiFabric.Api;

public class X12Functions
{
    private static EdiFunctions _ediFunctions;
    static X12Functions()
    {
        _ediFunctions = new EdiFunctions(EdiFabricServices.Get<IX12Service>());
    }

    public async Task<APIGatewayProxyResponse> ReadX12Async(APIGatewayProxyRequest request, ILambdaContext context)
    {        
        return await _ediFunctions.Read(request, context.Logger);
    }

    public async Task<APIGatewayProxyResponse> WriteX12Async(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Write(request, context.Logger);
    }

    public async Task<APIGatewayProxyResponse> ValidateX12Async(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Validate(request, context.Logger);
    }

    public async Task<APIGatewayProxyResponse> AckX12Async(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Ack(request, context.Logger);
    }
    /// <summary>
    /// This is a system operation used only for the in-house web translator.
    /// </summary>
    public async Task<APIGatewayProxyResponse> AnalyzeX12Async(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Analyze(request, context.Logger);
    }
}
