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
        readonly string ext = ".html";
        public AzureService() {

            BlobContainerClient = new BlobContainerClient(ConectionString, ContainerName);
        }

        public async Task<string> UploadToAzureBlobStorage(string filePath, string fileName) {

            using (UserDialogs.Instance.Loading(AppResources.Loading)) {
                var blob = BlobContainerClient.GetBlobClient(fileName);
                await blob.UploadAsync(filePath, true);
            }
        
            return $"https://notesbucket.blob.core.windows.net/notes/{fileName}";

        }

        public async Task DeleteFileFromBlobStorage(string fileName) {

            var blob = BlobContainerClient.GetBlobClient($"{fileName}{ext}");
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }
        public async Task<string> GetBlobStorage(string FileName) {
            string text = string.Empty;

            using (UserDialogs.Instance.Loading(AppResources.Downloading)) {

                await Task.Delay(1);
                var blob = BlobContainerClient.GetBlobClient(FileName);
                BlobDownloadInfo download = blob.Download();
                var content = download.Content;
                using (var streamReader = new StreamReader(content)) {
                    while (!streamReader.EndOfStream) {
                        text = await streamReader.ReadLineAsync();
                    }
                }
            }

            return text;
        }

        public string GetUrlBlobStorage(string FileName) {
            var x = $"https://notesbucket.blob.core.windows.net/notes/{FileName}.html";

            return x;
        }
    }
}