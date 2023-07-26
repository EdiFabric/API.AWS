using System.Net;
using EdiFabric.Api;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using EdiFabric.Api.AWS;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

public class EdiFunctions
{
    IEdiService _ediService;

    //  Change this to your API key
    private static string _apiKey = "3ecf6b1c5cf34bd797a5f4c57951a1cf";
    private static string _objectName = "token";
    private static string _bucketName = "edinationtestbucket";
    
    private readonly string _noData = "No data in request body.";
    private IModelService _modelService;

     public EdiFunctions(IEdiService ediService)
    {
        _ediService = ediService;
        _modelService = EdiFabricServices.Get<IModelService>();
        LoadModels().Wait();
    }

    public async Task<APIGatewayProxyResponse> Read(APIGatewayProxyRequest req, ILambdaLogger logger)
    {
        try
        {
            if (req == null || req.Body == null || req.Body.Length == 0)
            {
                logger.LogError(_noData);
                return ErrorHandler.BuildErrorResponse(HttpStatusCode.BadRequest, _noData);
            }

            TokenS3Cache.Set(_apiKey, _bucketName, _objectName);
            var body = req.IsBase64Encoded ? req.Body.Base64Decode() : req.Body;           
            using (var input = body.LoadToStream())
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = await _ediService.ReadAsync(input, _apiKey, req.GetReadParams()),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }           
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return ErrorHandler.BuildErrorResponse(ex);
        }
    }

    public async Task<APIGatewayProxyResponse> Write(APIGatewayProxyRequest req, ILambdaLogger logger)
    {
        try
        {
            if (req == null || req.Body == null || req.Body.Length == 0)
            {
                logger.LogError(_noData);
                return ErrorHandler.BuildErrorResponse(HttpStatusCode.BadRequest, _noData);
            }

            TokenS3Cache.Set(_apiKey, _bucketName, _objectName);
            var body = req.IsBase64Encoded ? req.Body.Base64Decode() : req.Body;
            using (var input = body.LoadToStream())
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = await _ediService.WriteAsync(input, _apiKey, req.GetWriteParams()),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/octet-stream; charset=utf-8" } }
                };
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return ErrorHandler.BuildErrorResponse(ex);
        }
    }

    public async Task<APIGatewayProxyResponse> Validate(APIGatewayProxyRequest req, ILambdaLogger logger)
    {
        try
        {
            if (req == null || req.Body == null || req.Body.Length == 0)
            {
                logger.LogError(_noData);
                return ErrorHandler.BuildErrorResponse(HttpStatusCode.BadRequest, _noData);
            }

            TokenS3Cache.Set(_apiKey, _bucketName, _objectName);
            var body = req.IsBase64Encoded ? req.Body.Base64Decode() : req.Body;
            using (var input = body.LoadToStream())
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = await _ediService.ValidateAsync(input, _apiKey, req.GetValidateParams()),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return ErrorHandler.BuildErrorResponse(ex);
        }
    }

    public async Task<APIGatewayProxyResponse> Ack(APIGatewayProxyRequest req, ILambdaLogger logger)
    {
        try
        {
            if (req == null || req.Body == null || req.Body.Length == 0)
            {
                logger.LogError(_noData);
                return ErrorHandler.BuildErrorResponse(HttpStatusCode.BadRequest, _noData);
            }

            TokenS3Cache.Set(_apiKey, _bucketName, _objectName);
            var body = req.IsBase64Encoded ? req.Body.Base64Decode() : req.Body;
            using (var input = body.LoadToStream())
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = await _ediService.GenerateAckAsync(input, _apiKey, req.GetAckParams()),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return ErrorHandler.BuildErrorResponse(ex);
        }
    }

    /// <summary>
    /// This is a system operation used only for the in-house web translator.
    /// </summary>
    public async Task<APIGatewayProxyResponse> Analyze(APIGatewayProxyRequest req, ILambdaLogger logger)
    {
        try
        {
            if (req == null || req.Body == null || req.Body.Length == 0)
            {
                logger.LogError(_noData);
                return ErrorHandler.BuildErrorResponse(HttpStatusCode.BadRequest, _noData);
            }

            TokenS3Cache.Set(_apiKey, _bucketName, _objectName);
            var body = req.IsBase64Encoded ? req.Body.Base64Decode() : req.Body;
            using (var input = body.LoadToStream())
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = await _ediService.AnalyzeAsync(input, _apiKey, req.GetAnalyzeParams()),
                    Headers = new Dictionary<string, string> { 
                        { "Content-Type", "application/json" }, 
                        { "Access-Control-Allow-Headers", "Content-Type,Ocp-Apim-Subscription-Key" },
                        { "Access-Control-Allow-Origin", "*"},
                        { "Access-Control-Allow-Methods", "OPTIONS,POST"}
                    }
                };
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return ErrorHandler.BuildErrorResponse(ex);
        }
    }

    public async Task LoadModels()
    {
        foreach (var obj in await S3Helper.ListFromCache(_bucketName))
        {
            if (obj.StartsWith("EdiNation") && obj.EndsWith(".dll"))
            {
                var model = await S3Helper.ReadFromCache(_bucketName, obj);
                model.Position = 0;
                await _modelService.Load(_apiKey, obj, model);
            }
        }
    }
}
