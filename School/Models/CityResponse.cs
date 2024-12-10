using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Models
{
    public class CityResponse
    {
        [JsonProperty("data")]
        public string[] Data { get; set; }
    }
}
