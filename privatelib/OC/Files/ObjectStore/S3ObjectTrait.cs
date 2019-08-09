using System.Collections.Generic;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;

namespace OC.Files.ObjectStore
{
    class S3ObjectTrait : S3ConnectionTrait
    {
        static int S3_UPLOAD_PART_SIZE = 524288000; // 500MB

        /**
         * @param string urn the unified resource name used to identify the object
         * @return resource stream with the read data
         * @throws \Exception when something goes wrong, message will be logged
         * @since 7.0.0
         */
        public Stream readObject(string urn)
        {
            var client = this.getConnection();
            return client.GetObjectStreamAsync(this.bucket, urn, new Dictionary<string, object>()).Result;
        }

        /**
         * @param string urn the unified resource name used to identify the object
         * @param resource stream stream with the data to write
         * @throws \Exception when something goes wrong, message will be logged
         * @since 7.0.0
         */
        public void writeObject(string urn, Stream stream)
        {
            this.getConnection()
                .UploadObjectFromStreamAsync(this.bucket, urn, stream, new Dictionary<string, object>()).Wait();
        }

        /**
         * @param string urn the unified resource name used to identify the object
         * @return void
         * @throws \Exception when something goes wrong, message will be logged
         * @since 7.0.0
         */
        public void deleteObject(string urn)
        {
            var deleteObjectRequest = new DeleteObjectRequest();
            deleteObjectRequest.Key = urn;
            deleteObjectRequest.BucketName = this.bucket;
            this.getConnection().DeleteObjectAsync(deleteObjectRequest).Wait();
        }

        public bool objectExists(string urn)
        {
            try
            {
                this.getConnection().GetObjectMetadataAsync(new GetObjectMetadataRequest()
                {
                    BucketName = this.bucket,
                    Key = urn
                }).Wait();

                return true;
            }

            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                //status wasn't not found, so throw the exception
                throw;
            }
        }
    }
}