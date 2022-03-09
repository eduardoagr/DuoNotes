using Acr.UserDialogs;

using Azure.Storage.Blobs;

using DuoNotes.Constants;
using DuoNotes.Resources;

using System.Threading.Tasks;

namespace DuoNotes.Services {
    public class AzureService {

        readonly string ConectionString = AppConstant.ConectionString;
        readonly string ContainerName = AppConstant.ContanerName;
        readonly BlobContainerClient BlobContainerClient;

        public AzureService() {

            BlobContainerClient = new BlobContainerClient(ConectionString, ContainerName);
        }

        public async Task<string> UploadToAzureBlobStorage(string filePath, string fileName) {

            UserDialogs.Instance.ShowLoading(AppResources.Loading);
            var blob = BlobContainerClient.GetBlobClient(fileName);
            await blob.UploadAsync(filePath, true);
            UserDialogs.Instance.HideLoading();

            return $"https://notesbucket.blob.core.windows.net/notes/{fileName}";

        }

        public async void DeleteFileFromBlobStorage(string fileName) {

            var blob = BlobContainerClient.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
        }
    }
}