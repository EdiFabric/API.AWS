using System.Text;

namespace EdiFabric.Api.AWS
{
    internal static class Extensions
    {
        internal static Stream LoadToStream(this string input, Encoding? encoding = null)
        {
            var enc = encoding ?? Encoding.UTF8;
            byte[] byteArray = enc.GetBytes(input);
            return new MemoryStream(byteArray);
        }

        internal static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
