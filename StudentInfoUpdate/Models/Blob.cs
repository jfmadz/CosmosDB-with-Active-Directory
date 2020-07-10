using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace StudentInfoUpdate
{
    public class Blob
    {
        public void UploadPhotoAsync(string containername, HttpPostedFileBase file)
        {
            var container = GetBlobContainer(containername);

            //Get File Name
            var fileName = Path.GetFileName(file.FileName);

            // Retrieve reference to a blob named "myblob".
            var blockBlob = container.GetBlockBlobReference(fileName);

            // Create or overwrite the "myblob" blob with contents from a local file.
            blockBlob.UploadFromStreamAsync(file.InputStream);
        }

        //Upload photo using optimistic concurrency
        public void UploadPhotoOptimistic(string containername, HttpPostedFileBase file)
        {
            var container = GetBlobContainer(containername);

            //Get File Name
            var fileName = Path.GetFileName(file.FileName);

            // Retrieve reference to a blob named "myblob".
            var blockBlob = container.GetBlockBlobReference(fileName);

            //Use the etag with optimistic concurrency
            AccessCondition accessCondition = new AccessCondition
            {
                IfMatchETag = blockBlob.Properties.ETag
            };

            try
            {
                // Create or overwrite the "myblob" blob with contents from a local file.
                blockBlob.UploadFromStream(file.InputStream, accessCondition);
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == (int)System.Net.HttpStatusCode.PreconditionFailed)
                {
                    throw new Exception("Precondition failure. Blob's orignal etag no longer matches");
                }
                else throw;
            }

        }
        private static CloudBlobContainer GetBlobContainer(string containername)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
             CloudConfigurationManager.GetSetting("StorageConnectionString"));

            //var storageAccount = CloudStorageAccount.Parse(
            //    CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            var container = blobClient.GetContainerReference(containername);

            // Set the permissions so the blobs are public. 
            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };

            //create container if it does not exist
            container.CreateIfNotExists();

            //set permission
            container.SetPermissions(permissions);

            return container;
        }
    }
}