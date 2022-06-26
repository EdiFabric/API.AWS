using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using EdiFabric.Api;

public class VdaFunctions
{
    private static EdiFunctions _ediFunctions;
    static VdaFunctions()
    {
        _ediFunctions = new EdiFunctions(EdiFabricServices.Get<IVdaService>());
    }

    public async Task<APIGatewayProxyResponse> ReadVdaAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Read(request, context.Logger);
    }

    public async Task<APIGatewayProxyResponse> WriteVdaAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Write(request, context.Logger);
    }

    public async Task<APIGatewayProxyResponse> ValidateVdaAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Validate(request, context.Logger);
    }
}
