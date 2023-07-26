using System.Text;

namespace EdiFabric.Api.AWS
{
    public class TokenS3Cache
    {
        //  Change path to whatever you prefer        
        private static string _objectName = "token";
        private static string _bucketName = "edinationtestbucket";

        public static void Set(string serialKey)
        {
            try
            {
                var token = ReadFromCache().Result;
                SerialKey.SetToken(token);

                //  Refresh token before expiration
                Refresh(serialKey);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

                //  Try one last time
                try
                {
                    var token = GetFromApi(serialKey);
                    WriteToCache(token).Wait();
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

        private static void Refresh(string serialKey)
        {
            try
            {
                //  Refresh the token two days before it expires
                if (SerialKey.DaysToExpiration < 3)
                    WriteToCache(GetFromApi(serialKey)).Wait();
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

        private static async Task<string> ReadFromCache()
        {
            var result = await S3Helper.ReadFromCache(_bucketName, _objectName);
            return LoadString(result);
        }

        public static async Task WriteToCache(string token)
        {
            await S3Helper.WriteToCache(_bucketName, _objectName, LoadStream(token));
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
