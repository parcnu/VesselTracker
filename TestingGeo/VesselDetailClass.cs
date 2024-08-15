using System;
using Newtonsoft.Json;
using Xamarin.Forms.Maps;

namespace TestingGeo
{
    public class VesselDetailClass
    {
        
        public string ID { get; set; }
        public string Name { get; set; }
        public string VesselClass { get; set; }
        public Distance DistanceToLocation { get; set; }
        
    }
}
