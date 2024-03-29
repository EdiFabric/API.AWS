﻿using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using EdiFabric.Api;

public class EdifactFunctions
{
    private static EdiFunctions _ediFunctions;
    static EdifactFunctions()
    {
        _ediFunctions = new EdiFunctions(EdiFabricServices.Get<IEdifactService>());
    }

    public async Task<APIGatewayProxyResponse> ReadEdifactAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Read(request, context.Logger);
    }

    public async Task<APIGatewayProxyResponse> WriteEdifactAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Write(request, context.Logger);
    }

    public async Task<APIGatewayProxyResponse> ValidateEdifactAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Validate(request, context.Logger);
    }

    public async Task<APIGatewayProxyResponse> AckEdifactAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Ack(request, context.Logger);
    }
    /// <summary>
    /// This is a system operation used only for the in-house web translator.
    /// </summary>
    public async Task<APIGatewayProxyResponse> AnalyzeEdifactAsync(APIGatewayProxyRequest request, ILambdaContext context)
    {
        return await _ediFunctions.Analyze(request, context.Logger);
    }
}
