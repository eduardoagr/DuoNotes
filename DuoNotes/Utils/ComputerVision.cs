using System;
using System.Threading.Tasks;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace DuoNotes.Utils
{
    public class ComputerVision
    {

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
            return client;
        }

        public static async Task<System.Collections.Generic.IList<ReadResult>> ReadText(ComputerVisionClient client, string urlFile)
        {

            // Read text from URL
            var textHeaders = await client.ReadAsync(urlFile, null, null, "latest");
            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;

            // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            ReadOperationResult results;
            do
            {
                results = await client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running ||
                results.Status == OperationStatusCodes.NotStarted));

            return results.AnalyzeResult.ReadResults;
        }

    }
}
