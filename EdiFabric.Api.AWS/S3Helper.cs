using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace EdiFabric.Api.AWS
{
    public class S3Helper
    {
        private static IAmazonS3 _s3Client = new AmazonS3Client(RegionEndpoint.USEast1);
        
        public static async Task<Stream> ReadFromCache(string bucketName, string objectName)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectName,
                };

                using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
                {
                    var ms = new MemoryStream();
                    using (Stream responseStream = response.ResponseStream)
                    {
                        responseStream.CopyTo(ms);
                    }

                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        return ms;

                    throw new Exception($"Could not download {objectName} to {bucketName}.");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public static async Task WriteToCache(string bucketName, string objectName, Stream data)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
                InputStream = data
            };

            var response = await _s3Client.PutObjectAsync(request);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"Could not upload {objectName} to {bucketName}.");
        }
    }
}
