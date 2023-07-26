using System.Reflection;
using System.Text;

namespace EdiFabric.Api.AWS
{
    public class TokenS3Cache
    {      
        public static void Set(string serialKey, string bucketName, string objectName)
        {
            try
            {
                var token = ReadTokenFromCache(bucketName, objectName).Result;
                SerialKey.SetToken(token);

                //  Refresh token before expiration
                Refresh(serialKey, bucketName, objectName);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

                //  Try one last time
                try
                {
                    var token = GetFromApi(serialKey);
                    WriteTokenToCache(token, bucketName, objectName).Wait();
                    SerialKey.SetToken(token);
                }
                catch (Exception ex2)
                {
                    Console.WriteLine(ex2.Message);
                    //  Contact support@edifabric.com for assistance
                    throw;
                }
            }
        }

        private static void Refresh(string serialKey, string bucketName, string objectName)
        {
            try
            {
                //  Refresh the token two days before it expires
                if (SerialKey.DaysToExpiration < 3)
                    WriteTokenToCache(GetFromApi(serialKey), bucketName, objectName).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //  If can't get a token a day before the current expires - throw an exception
                //  Otherwise keep trying
                if (SerialKey.DaysToExpiration <= 1)
                    throw;
            }
        }

        private static string GetFromApi(string serialKey)
        {
            int retries = 3;
            int index = 0;

            //  Try to get a token with retries
            while (index < retries)
            {
                try
                {
                    return SerialKey.GetToken(serialKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    index++;

                    if (index >= retries)
                        throw;
                }
            }

            throw new Exception("Can't get a token.");
        }

        private static async Task<string> ReadTokenFromCache(string bucketName, string objectName)
        {
            var result = await S3Helper.ReadFromCache(bucketName, objectName);
            return LoadString(result);
        }

        private static async Task WriteTokenToCache(string token, string bucketName, string objectName)
        {
            await S3Helper.WriteToCache(bucketName, objectName, LoadStream(token));
        }

        private static string LoadString(Stream stream, Encoding encoding = null)
        {
            stream.Position = 0;
            using (var reader = new StreamReader(stream, encoding ?? Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private static MemoryStream LoadStream(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value));
        }
    }
}
