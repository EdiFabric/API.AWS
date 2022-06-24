using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

namespace EdiFabric.Api.AWS
{
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
    }
}
