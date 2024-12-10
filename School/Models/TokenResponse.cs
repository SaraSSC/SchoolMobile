using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Models
{
    public class TokenResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expiration")]
        public string Expiration { get; set; }
    }
}
