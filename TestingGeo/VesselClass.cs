using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TestingGeo
{
    public class VesselClass
    {
        [JsonProperty("vessels")]
        public List<VesselDetailJson> vessels { get; set; }
        [JsonProperty("Endpoints")]
        public List<EndPointClass> Endpoints { get; set; }

    }
}

