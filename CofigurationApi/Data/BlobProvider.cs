using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CofigurationApi.Data
{
    public class BlobProvider
    {
        private CloudBlobContainer _cloudBlobContainer;

        public async Task InitAsync()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=configurationstor;AccountKey=CY1iDTXZCPdEjbQ/v8iJ/ZC/fQ8Hf/9WUWUNk0xFV86hEO99zX5FD4vqv4yiYWLR3WtCRT2W+jH1raSm5xx9xQ==;EndpointSuffix=core.windows.net";

            CloudStorageAccount storageAccount;

            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {

                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer cloudBlobContainer =
                    cloudBlobClient.GetContainerReference("configurationcontainer");
                await cloudBlobContainer.CreateIfNotExistsAsync();
               //         Guid.NewGuid().ToString());
               // await cloudBlobContainer.CreateAsync();

                // Set the permissions so the blobs are public.
                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                await cloudBlobContainer.SetPermissionsAsync(permissions);
                _cloudBlobContainer = cloudBlobContainer;

            }
            else
            {
                // Otherwise, let the user know that they need to define the environment variable.
                Console.WriteLine(
                    "A connection string has not been defined in the system environment variables. " +
                    "Add an environment variable named 'CONNECT_STR' with your storage " +
                    "connection string as a value.");
                Console.WriteLine("Press any key to exit the application.");
                Console.ReadLine();
            }
        }

        public static void CreateFileSampleForBlob()
        {
            // Create a file in your local MyDocuments folder to upload to a blob.
            string localPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string localFileName = "QuickStart_" + Guid.NewGuid().ToString();
            string sourceFile = Path.Combine(localPath, localFileName);
            // Write text to the file.
            File.WriteAllText(sourceFile, "Hello, World!");
        }

        public static async Task UploadFileToBlob(CloudBlobContainer cloudBlobContainer, string localFileName, string sourceFile)
        {
            Console.WriteLine("Temp file = {0}", sourceFile);
            Console.WriteLine("Uploading to Blob storage as blob '{0}'", localFileName);
            // Get a reference to the blob address, then upload the file to the blob.
            // Use the value of localFileName for the blob name.
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);
            await cloudBlockBlob.UploadFromFileAsync(sourceFile);
        }

        public async Task<string> DownloadBlob(string blobName)
        {
            return await _cloudBlobContainer.GetBlockBlobReference(blobName).DownloadTextAsync();
        }

        public async Task ListAllBlobs(CloudBlobContainer cloudBlobContainer)
        {
            // List the blobs in the container.
            Console.WriteLine("List blobs in container.");
            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var results = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                // Get the value of the continuation token returned by the listing call.
                blobContinuationToken = results.ContinuationToken;
                foreach (IListBlobItem item in results.Results)
                {
                    Console.WriteLine(item.Uri);
                }
            } while (blobContinuationToken != null); // Loop while the continuation token is not null.
        }

        //public async Task<string> DownloadTextAsync(ICloudBlob blob)
        //{
        //    using (Stream memoryStream = new MemoryStream())
        //    {
        //        IAsyncResult asyncResult = blob.BeginDownloadToStream(memoryStream, null, null);
        //        await Task.Factory.FromAsync(asyncResult, (r) => { blob.EndDownloadToStream(r); });
        //        memoryStream.Position = 0;

        //        using (StreamReader streamReader = new StreamReader(memoryStream))
        //        {
        //            // is this good enough?
        //            return streamReader.ReadToEnd();

        //            // or do we need this?
        //            return await streamReader.ReadToEndAsync();
        //        }
        //    }
        // }
    }
}
