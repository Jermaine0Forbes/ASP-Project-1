using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Configurations;


namespace WebApplication1.Services
{

    public class AzureService
    (
         IOptions<AzureSettings> settings
    )
    {
        private readonly AzureSettings _settings = settings.Value;
        private BlobServiceClient? bsc = null;
        private string blobContainerName = "uploads";


        public Boolean IsBlobConnected()
        {
            string connectionString = _settings.Storage;
            bsc = new BlobServiceClient(connectionString);

            return bsc != null;
        }


        public bool HasConnectionStr()
        {
            return !_settings.Storage.IsNullOrEmpty();
        }

        public void SetBlobContainerName(string name)
        {
            // blobContainerName = Path.Combine(blobContainerName,name);
            blobContainerName = name;
        }


        public async Task<string> UploadBlob(IFormFile file, string userId = "")
        {
            if(!userId.IsNullOrEmpty()) SetBlobContainerName(userId);
            
            var containerClient = bsc?.GetBlobContainerClient(blobContainerName) ?? throw new Exception("Blob service is not connected");

            // Ensure container exists
            await containerClient.CreateIfNotExistsAsync();

            // Get a reference to the blob
            var blobClient = containerClient.GetBlobClient(file.FileName);

            // Upload the file stream
            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });


            return blobClient.Name;

        }

        public async Task<string> GetBlob(string fileName, string userId = "")
        {
            if(!userId.IsNullOrEmpty()) SetBlobContainerName(userId);

            if (!IsBlobConnected())
            {
                throw new Exception("Blob service is not connected");
            }

            BlobContainerClient containerClient = bsc?.GetBlobContainerClient(blobContainerName)
                ?? throw new Exception("Blob service is not connected");
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            if (!blobClient.CanGenerateSasUri)
            {
                throw new Exception("Blob client cannot generate SAS Uri");
            }


            var prop = await blobClient.GetPropertiesAsync();

            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = prop.GetRawResponse().Headers.FirstOrDefault(h => h.Name == "x-ms-meta-container").Value 
                ?? blobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1) // Link valid for 1 hour
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return  blobClient.GenerateSasUri(sasBuilder).ToString();
        }


    }

}

