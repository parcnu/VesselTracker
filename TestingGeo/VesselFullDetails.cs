using System;
namespace TestingGeo
{
    public class VesselFullDetails : VesselDetailJson
    {
        /*
         *
        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("position-latitude")]
        public string latitude { get; set; }

        [JsonProperty("position-longitude")]
        public string longitude { get; set; }

        [JsonProperty("heading")]
        public string heading { get; set; }

        [JsonProperty("speed")]
        public string speed { get; set; }

        [JsonProperty("class")]
        public string vesselclass { get; set; }

        [JsonProperty("Url")]
        public string url { get; set; }
         * */

        public string Name { get; set; }
      
        public string Lenght { get; set; }
      
        public string MaxDraft { get; set; }
     
        public string MaxSpeed { get; set; }
    }
}
