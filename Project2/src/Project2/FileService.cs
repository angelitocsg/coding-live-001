using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;

namespace Project2
{
    public class FileService
    {
        private readonly IAmazonS3 _S3Client;
        private readonly ILambdaContext _context;

        public FileService(IAmazonS3 S3Client, ILambdaContext context)
        {
            _S3Client = S3Client;
            _context = context;
        }

        public async Task DownloadFromS3(string bucketName, string fileName, string destFileName)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                };

                System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();

                using (GetObjectResponse response = await _S3Client.GetObjectAsync(request))
                {
                    await response.WriteResponseStreamToFileAsync(destFileName, false, cts.Token);
                }
            }
            catch (Exception ex)
            {
                _context.Logger.LogLine("Download exception: " + ex.Message);
            }
        }

        public async Task UploadToS3(string bucketName, string fileName, string filePath)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = fileName,
                    FilePath = filePath
                };

                await _S3Client.PutObjectAsync(request);
            }
            catch (Exception ex)
            {
                _context.Logger.LogLine("Upload exception: " + ex.Message);
            }
        }
    }
}