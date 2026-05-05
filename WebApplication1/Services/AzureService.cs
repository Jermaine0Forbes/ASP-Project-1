using System.Reflection.Metadata;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using WebApplication1.Configurations;


namespace WebApplication1.Services
{

    public class AzureService
    (
         IOptions<AzureSettings> azure
    )
    {
        private readonly AzureSettings _azure = azure.Value;
        private BlobServiceClient? bsc = null;


        public Boolean IsBlobConnected()
        {
            string connectionString = _azure.Storage;
            bsc = new BlobServiceClient(connectionString);

            return bsc == null;
        }


        public async Task<string> UploadBlob(IFormFile file)
        {
            var containerClient = bsc?.GetBlobContainerClient("uploads") ?? throw new Exception("Blob service is not connected");

            // Ensure container exists
            await containerClient.CreateIfNotExistsAsync();

            // Get a reference to the blob
            var blobClient = containerClient.GetBlobClient(file.FileName);

            // Upload the file stream
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });


            return blobClient.Name;

        }


    }

}

