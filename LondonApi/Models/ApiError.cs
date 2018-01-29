using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LondonApi.Models
{
    public class ApiError
    {
        public string Message { get; set; }

        public string Detail { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling =DefaultValueHandling.Ignore)]
        [DefaultValue("")] // If stack trace is empty, it wont be added to the responce. 
        public string StackTrace { get; set; }

    }
}
