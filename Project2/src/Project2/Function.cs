using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Util;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.LambdaJsonSerializer))]

namespace Project2
{
    public class Function
    {
        IAmazonS3 S3Client { get; set; }

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            S3Client = new AmazonS3Client();
        }

        /// <summary>
        /// Constructs an instance with a preconfigured S3 client. This can be used for testing the outside of the Lambda environment.
        /// </summary>
        /// <param name="s3Client"></param>
        public Function(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
        }

        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
        /// to respond to S3 notifications.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(S3Event evnt, ILambdaContext context)
        {
            var s3Event = evnt.Records?[0].S3;
            if (s3Event == null)
            {
                return;
            }

            try
            {
                FileService _fileService;
                ImageService _imageService;

                var bucketName = s3Event.Bucket.Name;
                var localFilePSD = "/tmp/" + s3Event.Object.Key.Split("/").Last(); // Only /tmp/ folder is writable
                var localFilePNG = "/tmp/" + s3Event.Object.Key.Split("/").Last().Replace(".psd", ".png"); // Only /tmp/ folder is writable
                var remoteFilePSD = s3Event.Object.Key;
                var remoteFilePNG = s3Event.Object.Key.Replace(".psd", "_thumb.png");


                _fileService = new FileService(this.S3Client, context);
                await _fileService.DownloadFromS3(bucketName, remoteFilePSD, localFilePSD);

                _imageService = new ImageService(localFilePSD, localFilePNG);
                _imageService.Resize(200, 200, false);

                await _fileService.UploadToS3(bucketName, remoteFilePNG, localFilePNG);
            }
            catch (Exception e)
            {
                context.Logger.LogLine($"Error getting object {s3Event.Object.Key} from bucket {s3Event.Bucket.Name}. Make sure they exist and your bucket is in the same region as this function.");
                context.Logger.LogLine(e.Message);
                context.Logger.LogLine(e.StackTrace);
                throw;
            }
        }
    }
}
