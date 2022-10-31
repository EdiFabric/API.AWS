using Amazon.Lambda.APIGatewayEvents;
using EdiFabric.Api;
using System.Text;

internal static class Extensions
{
    public static ReadParams GetReadParams(this APIGatewayProxyRequest req)
    {        
        var result = new ReadParams();

        if (req.QueryStringParameters != null)
        {
            var queryDictionary = req.QueryStringParameters;

            if (queryDictionary.TryGetValue("continueOnError", out string coe) && bool.TryParse(coe, out bool continueOnError))
            {
                result.ContinueOnError = continueOnError;
            }

            if (queryDictionary.TryGetValue("charSet", out string charSet) && !string.IsNullOrEmpty(charSet))
            {
                result.CharSet = charSet;
            }

            if (queryDictionary.TryGetValue("eancomS3", out string es3) && bool.TryParse(es3, out bool eancomS3))
            {
                result.EancomS3IsDefault = eancomS3;
            }

            if (queryDictionary.TryGetValue("ignoreNullValues", out string inv) && bool.TryParse(inv, out bool ignoreNullValues))
            {
                result.IgnoreNullValues = ignoreNullValues;
            }

            if (queryDictionary.TryGetValue("model", out string model) && !string.IsNullOrEmpty(model))
            {
                result.Model = model;
            }
        }

        return result;
    }

    public static WriteParams GetWriteParams(this APIGatewayProxyRequest req)
    {
        var result = new WriteParams();

        if (req.QueryStringParameters != null)
        {
            var queryDictionary = req.QueryStringParameters;

            if (queryDictionary.TryGetValue("preserveWhitespace", out string pw) && bool.TryParse(pw, out bool preserveWhitespace))
            {
                result.PreserveWhitespace = preserveWhitespace;
            }

            result.ContentType = "application/octet-stream; charset=utf-8";
            if (queryDictionary.TryGetValue("charSet", out string charSet) && !string.IsNullOrEmpty(charSet))
            {
                result.CharSet = charSet;
                result.ContentType = $"application/octet-stream; charset={charSet}";
            }

            if (queryDictionary.TryGetValue("postfix", out string postFix) && !string.IsNullOrEmpty(postFix))
            {
                result.Postfix = postFix;
            }

            if (queryDictionary.TryGetValue("eancomS3", out string es3) && bool.TryParse(es3, out bool eancomS3))
            {
                result.EancomS3IsDefault = eancomS3;
            }

            if (queryDictionary.TryGetValue("noG1", out string ng) && bool.TryParse(ng, out bool noG1))
            {
                result.NoG1 = noG1;
            }

            if (queryDictionary.TryGetValue("trailerMessage", out string trailerMessage) && !string.IsNullOrEmpty(trailerMessage))
            {
                result.NcpdpTrailerMessage = trailerMessage;
            }
        }

        return result;
    }

    public static ValidateParams GetValidateParams(this APIGatewayProxyRequest req)
    {
        var result = new ValidateParams();

        if (req.QueryStringParameters != null)
        {
            var queryDictionary = req.QueryStringParameters;

            if (queryDictionary.TryGetValue("skipTrailer", out string st) && bool.TryParse(st, out bool skipTrailer))
            {
                result.SkipTrailerValidation = skipTrailer;
            }

            if (queryDictionary.TryGetValue("decimalPoint", out string decimalPoint) && !string.IsNullOrEmpty(decimalPoint))
            {
                result.DecimalPoint = decimalPoint == "." ? '.' : ',';
            }

            if (queryDictionary.TryGetValue("syntaxSet", out string syntaxSet) && !string.IsNullOrEmpty(syntaxSet))
            {
                result.SyntaxSet = syntaxSet;
            }

            if (queryDictionary.TryGetValue("structureOnly", out string so) && bool.TryParse(so, out bool structureOnly))
            {
                result.StructureOnly = structureOnly;
            }

            if (queryDictionary.TryGetValue("eancomS3", out string es3) && bool.TryParse(es3, out bool eancomS3))
            {
                result.EancomS3IsDefault = eancomS3;
            }

            if (queryDictionary.TryGetValue("basicSyntax", out string bs) && bool.TryParse(bs, out bool basicSyntax))
            {
                result.BasicSyntax = basicSyntax;
            }
        }

        return result;
    }

    public static AckParams GetAckParams(this APIGatewayProxyRequest req)
    {
        var result = new AckParams();

        if (req.QueryStringParameters != null)
        {
            var queryDictionary = req.QueryStringParameters;

            if (queryDictionary.TryGetValue("syntaxSet", out string syntaxSet) && !string.IsNullOrEmpty(syntaxSet))
            {
                result.SyntaxSet = syntaxSet;
            }

            if (queryDictionary.TryGetValue("detectDuplicates", out string dd) && bool.TryParse(dd, out bool detectDuplicates))
            {
                result.DetectDuplicates = detectDuplicates;
            }

            if (queryDictionary.TryGetValue("ackForValidTrans", out string avm) && bool.TryParse(avm, out bool ackForValidTrans))
            {
                result.GenerateForValidMessages = ackForValidTrans;
            }

            if (queryDictionary.TryGetValue("tranRefNumber", out string mcn) && int.TryParse(mcn, out int tranRefNumber))
            {
                result.MessageControlNumber = tranRefNumber;
            }

            if (queryDictionary.TryGetValue("technicalAck", out string technicalAck) && !string.IsNullOrEmpty(technicalAck))
            {
                result.TechnicalAck = technicalAck;
            }

            if (queryDictionary.TryGetValue("eancomS3", out string es3) && bool.TryParse(es3, out bool eancomS3))
            {
                result.EancomS3IsDefault = eancomS3;
            }

            if (queryDictionary.TryGetValue("batchAcks", out string ba) && bool.TryParse(ba, out bool batchAcks))
            {
                result.BatchAcks = batchAcks;
            }

            if (queryDictionary.TryGetValue("interchangeRefNumber", out string irn) && int.TryParse(irn, out int interchangeRefNumber))
            {
                result.InterchangeControlNumber = interchangeRefNumber;
            }

            if (queryDictionary.TryGetValue("ack", out string ack) && !string.IsNullOrEmpty(ack))
            {
                result.AckVersion = ack;
            }

            if (queryDictionary.TryGetValue("batchAcks", out string ak9isP) && bool.TryParse(ak9isP, out bool ak901isP))
            {
                result.Ak901ShouldBeP = ak901isP;
            }
        }

        return result;
    }

    public static AnalyzeParams GetAnalyzeParams(this APIGatewayProxyRequest req)
    {
        var result = new AnalyzeParams();

        if (req.QueryStringParameters != null)
        {
            var queryDictionary = req.QueryStringParameters;

            if (queryDictionary.TryGetValue("model", out string model) && !string.IsNullOrEmpty(model))
            {
                result.Model = model;
            }

            if (queryDictionary.TryGetValue("syntaxSet", out string syntaxSet) && !string.IsNullOrEmpty(syntaxSet))
            {
                result.SyntaxSet = syntaxSet;
            }

            if (queryDictionary.TryGetValue("eancomS3", out string es3) && bool.TryParse(es3, out bool eancomS3))
            {
                result.EancomS3IsDefault = eancomS3;
            }

            if (queryDictionary.TryGetValue("basicSyntax", out string bs) && bool.TryParse(bs, out bool basicSyntax))
            {
                result.BasicSyntax = basicSyntax;
            }

            if (queryDictionary.TryGetValue("ack", out string ack) && !string.IsNullOrEmpty(ack))
            {
                result.AckVersion = ack;
            }

            if (queryDictionary.TryGetValue("charSet", out string charSet) && !string.IsNullOrEmpty(charSet))
            {
                result.CharSet = charSet;
            }

            if (queryDictionary.TryGetValue("skipSeq", out string sr) && bool.TryParse(sr, out bool skipSeq))
            {
                result.SkipSeqCountValidation = skipSeq;
            }
        }

        return result;
    }

    public static Stream LoadToStream(this string input, Encoding? encoding = null)
    {
        var enc = encoding ?? Encoding.UTF8;
        byte[] byteArray = enc.GetBytes(input);
        return new MemoryStream(byteArray);
    }

    public static string Base64Decode(this string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }
}
