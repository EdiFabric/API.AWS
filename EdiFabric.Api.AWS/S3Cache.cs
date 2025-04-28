using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace EdiFabric.Api.AWS
{
    public class S3Cache
    {
        public static void Set(string apiKey)
        {
            try
            {
                var token = ReadTokenFromCache().Result;
                SerialKey.SetToken(token);

                //  Refresh token before expiration
                Refresh(apiKey);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());

                //  Try one last time
                try
                {
                    var token = GetFromApi(apiKey);
                    WriteTokenToCache(token).Wait();
                    SerialKey.SetToken(token);
                }
                catch (Exception ex2)
                {
                    Console.WriteLine(ex2.ToString());
                    //  Contact support@edifabric.com for assistance
                    throw;
                }
            }
        }

        public static async Task LoadModels()
        {
            var loadContext = GetLoadContext(Configuration.ApiKey);

            foreach (var obj in await S3Helper.ListFromCache(Configuration.BucketName))
            {
                if (obj.EndsWith(".dll"))
                {
                    if (loadContext.Assemblies.FirstOrDefault(a => a.GetName().Name == obj) == null)
                    {
                        var model = await S3Helper.ReadFromCache(Configuration.BucketName, obj);
                        model.Position = 0;
                        loadContext.LoadFromStream(model);
                    }
                }
            }
        }

        private static void Refresh(string apiKey)
        {
            try
            {
                //  Refresh the token two days before it expires
                if (SerialKey.DaysToExpiration < 3)
                    WriteTokenToCache(GetFromApi(apiKey)).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //  If can't get a token a day before the current expires - throw an exception
                //  Otherwise keep trying
                if (SerialKey.DaysToExpiration <= 1)
                    throw;
            }
        }

        private static string GetFromApi(string apiKey)
        {
            int retries = 3;
            int index = 0;

            //  Try to get a token with retries
            while (index < retries)
            {
                try
                {
                    return SerialKey.GetToken(apiKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
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

        private static AssemblyLoadContext GetLoadContext(string apiKey)
        {
            return AssemblyLoadContext.All.FirstOrDefault(alc => alc.Name == apiKey) ?? new CustomAssemblyLoadContext(apiKey);
        }

        public static List<Assembly> GetLoadedAssemblies()
        {
            return GetLoadContext(Configuration.ApiKey).Assemblies.ToList();
        }
    }

    class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public CustomAssemblyLoadContext(string apiKey) : base(apiKey)
        {
        }
    }
}
