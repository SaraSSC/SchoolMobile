using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Models
{
    public class CityRequest
    {
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
