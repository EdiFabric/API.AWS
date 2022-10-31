﻿using System.Net;
using EdiFabric.Api;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

public class EdiFunctions
{
    IEdiService _ediService;

    private readonly string _apiKey = "Ocp-Apim-Subscription-Key";
    private readonly string _noApiKey = "No Ocp-Apim-Subscription-Key in header.";
    private readonly string _noData = "No data in request body.";

    public EdiFunctions(IEdiService ediService)
    {
        _ediService = ediService;
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

            if (!req.Headers.TryGetValue(_apiKey, out var apiKey))
            {
                logger.LogError(_noApiKey);
                return ErrorHandler.BuildErrorResponse(HttpStatusCode.BadRequest, _noApiKey);
            }

            var body = req.IsBase64Encoded ? req.Body.Base64Decode() : req.Body;           
            using (var input = body.LoadToStream())
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = await _ediService.ReadAsync(input, apiKey, req.GetReadParams()),
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

            if (!req.Headers.TryGetValue(_apiKey, out var apiKey))
            {
                logger.LogError(_noApiKey);
                return ErrorHandler.BuildErrorResponse(HttpStatusCode.BadRequest, _noApiKey);
            }

            var body = req.IsBase64Encoded ? req.Body.Base64Decode() : req.Body;
            using (var input = body.LoadToStream())
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = await _ediService.WriteAsync(input, apiKey, req.GetWriteParams()),
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

            if (!req.Headers.TryGetValue(_apiKey, out var apiKey))
            {
                logger.LogError(_noApiKey);
                return ErrorHandler.BuildErrorResponse(HttpStatusCode.BadRequest, _noApiKey);
            }

            var body = req.IsBase64Encoded ? req.Body.Base64Decode() : req.Body;
            using (var input = body.LoadToStream())
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = await _ediService.ValidateAsync(input, apiKey, req.GetValidateParams()),
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

            if (!req.Headers.TryGetValue(_apiKey, out var apiKey))
            {
                logger.LogError(_noApiKey);
                return ErrorHandler.BuildErrorResponse(HttpStatusCode.BadRequest, _noApiKey);
            }

            var body = req.IsBase64Encoded ? req.Body.Base64Decode() : req.Body;
            using (var input = body.LoadToStream())
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = await _ediService.GenerateAckAsync(input, apiKey, req.GetAckParams()),
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

    public async Task<APIGatewayProxyResponse> Analyze(APIGatewayProxyRequest req, ILambdaLogger logger)
    {
        try
        {
            if (req == null || req.Body == null || req.Body.Length == 0)
            {
                logger.LogError(_noData);
                return ErrorHandler.BuildErrorResponse(HttpStatusCode.BadRequest, _noData);
            }

            if (!req.Headers.TryGetValue(_apiKey, out var apiKey))
            {
                logger.LogError(_noApiKey);
                return ErrorHandler.BuildErrorResponse(HttpStatusCode.BadRequest, _noApiKey);
            }

            var body = req.IsBase64Encoded ? req.Body.Base64Decode() : req.Body;
            using (var input = body.LoadToStream())
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Body = await _ediService.AnalyzeAsync(input, apiKey, req.GetAnalyzeParams()),
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
}
