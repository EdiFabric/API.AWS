using System.Text;

namespace EdiFabric.Api.AWS
{
    public class S3Cache
    {
        public static void Set()
        {
            try
            {
                var token = ReadTokenFromCache().Result;
                SerialKey.SetToken(token);

                //  Refresh token before expiration
                Refresh();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

                //  Try one last time
                try
                {
                    var token = GetFromApi();
                    WriteTokenToCache(token).Wait();
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

        public static async Task LoadModels(IModelService modelService)
        {
            foreach (var obj in await S3Helper.ListFromCache(Configuration.BucketName))
            {
                if (obj.StartsWith("EdiNation") && obj.EndsWith(".dll"))
                {
                    var model = await S3Helper.ReadFromCache(Configuration.BucketName, obj);
                    model.Position = 0;
                    await modelService.Load(Configuration.ApiKey, obj, model);
                }
            }
        }

        private static void Refresh()
        {
            try
            {
                //  Refresh the token two days before it expires
                if (SerialKey.DaysToExpiration < 3)
                    WriteTokenToCache(GetFromApi()).Wait();
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

        private static string GetFromApi()
        {
            int retries = 3;
            int index = 0;

            //  Try to get a token with retries
            while (index < retries)
            {
                try
                {
                    return SerialKey.GetToken(Configuration.ApiKey);
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

        private static async Task<string> ReadTokenFromCache()
        {
            var result = await S3Helper.ReadFromCache(Configuration.BucketName, Configuration.ObjectName);
            return LoadString(result);
        }

        private static async Task WriteTokenToCache(string token)
        {
            await S3Helper.WriteToCache(Configuration.BucketName, Configuration.ObjectName, LoadStream(token));
        }

        private static string LoadString(Stream stream)
        {
            return new StreamReader(stream, Encoding.UTF8).ReadToEnd();  
        }

        private static MemoryStream LoadStream(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value));
        }
    }
}
