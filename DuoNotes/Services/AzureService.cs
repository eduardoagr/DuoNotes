using Azure.Storage.Blobs;

using DuoNotes.Constants;

using System.Threading.Tasks;

namespace DuoNotes.Services {
    public class AzureService {

        public async Task<string> UploadToAzureBlobStorage(string filePath, string fileName) {

            var conectionString = AppConstant.ContanerName;
            var containerName = AppConstant.ContanerName;

            var AzureStorage = new BlobContainerClient(conectionString, containerName);

            var blob = AzureStorage.GetBlobClient(fileName);
            await blob.UploadAsync(filePath);

            return $"https://notesbucket.blob.core.windows.net/notes/{fileName}";

        }

        public async void DeleteFileFromBlobStorage(string fileName) {

            var conectionString = AppConstant.ContanerName;
            var containerName = AppConstant.ContanerName;

            var AzureStorage = new BlobContainerClient(conectionString, containerName);

            await AzureStorage.GetBlobClient(fileName).DeleteIfExistsAsync();


        }
    }
}
