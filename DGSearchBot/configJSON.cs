using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DGSearchBot
{
    public struct ConfigJSON
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
        [JsonProperty("weatherapi")]
        public string Weatherapi { get; set; }
    }
}
