using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TestingGeo
{
  
    public partial class Endpoint
    {
        [JsonProperty("Url")]
        public string Url { get; set; }
    }
}
