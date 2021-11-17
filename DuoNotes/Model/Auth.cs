using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Text;

namespace DuoNotes.Model {
    public class ErrorMsg {

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class Error {

        [JsonProperty("errors")]
        public IList<Error> Errors { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class Response {

        [JsonProperty("error")]
        public Error Error { get; set; }
    }

}
