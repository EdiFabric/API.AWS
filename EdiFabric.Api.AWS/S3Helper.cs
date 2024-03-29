﻿using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace EdiFabric.Api.AWS
{
    public class S3Helper
    {
        private static IAmazonS3 _s3Client = new AmazonS3Client(RegionEndpoint.USEast1);

        public static async Task<List<string>> ListFromCache(string bucketName)
        {
            var result = new List<string>();

            var request = new ListObjectsV2Request
            {
                BucketName = bucketName
            };

            ListObjectsV2Response response;
            do
            {
                response = await _s3Client.ListObjectsV2Async(request);

                foreach (var obj in response.S3Objects)
                {
                    result.Add(obj.Key);
                }

                request.ContinuationToken = response.NextContinuationToken;
            }
            while (response.IsTruncated);

            return result;
        }

        public static async Task<Stream> ReadFromCache(string bucketName, string objectName)
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
                {
                    ms.Position = 0;
                    return ms;
                }

                throw new Exception($"Could not download {objectName} to {bucketName}.");
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
