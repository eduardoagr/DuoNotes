using Acr.UserDialogs;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using DuoNotes.Constants;
using DuoNotes.Resources;

using System.IO;
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

        public async Task DeleteFileFromBlobStorage(string fileName) {

            var blob = BlobContainerClient.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }

        public async Task<string> GetTextfromBlobStorage(string FileName) {

            var blob = BlobContainerClient.GetBlobClient(FileName);
            BlobDownloadInfo download = blob.Download();
            var content = download.Content;
            string text = string.Empty;
            using (var streamReader = new StreamReader(content)) {
                while (!streamReader.EndOfStream) {
                    text = await streamReader.ReadLineAsync();
                }
            }

            return text;
        }

        public Stream GetStreamBlobStorage(string FileName) {

            var blob = BlobContainerClient.GetBlobClient(FileName);
            BlobDownloadInfo download = blob.Download();
            var content = download.Content;

            return content;
        }
    }
}