using System.Collections.Generic;

using Newtonsoft.Json;

namespace DuoNotes.Model
{
    internal class ErrorMsg
    {

        [JsonProperty("domain")]
        public string Domain
        {
            get; set;
        }

        [JsonProperty("reason")]
        public string Reason
        {
            get; set;
        }

        [JsonProperty("message")]
        public string Message
        {
            get; set;
        }
    }

    internal class Error
    {

        [JsonProperty("errors")]
        public IList<Error> Errors
        {
            get; set;
        }

        [JsonProperty("code")]
        public int Code
        {
            get; set;
        }

        [JsonProperty("message")]
        public string Message
        {
            get; set;
        }
    }

    internal class Response
    {

        [JsonProperty("error")]
        public Error Error
        {
            get; set;
        }
    }

}
