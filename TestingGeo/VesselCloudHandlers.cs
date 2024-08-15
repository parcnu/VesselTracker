using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestingGeo
{
    public class VesselCloudHandlers
    {

        ////private const string Url = "http://api.mtbcloud.net/api/vessels";
        ////private const string Url = "https://mtbtest.azurewebsites.net/api/vessels2";
        private const string Url = "https://mtbtest2.azurewebsites.net/api/vessels";
        private const string PostUri = "https://mtbtest2.azurewebsites.net/api/vessels/update";
        private readonly HttpClient httpclient = new HttpClient();
        private VesselClass deserializedvessels = new VesselClass();
        private String restContent = "";

        ////private String TestRestContent = "{\"vessels\":[{\"ID\":\"123\",\"position-latitude\":\"60.379611\",\"position-longitude\":\"22.107599\",\"heading\":\"189\",\"speed\":\"17.2\",\"class\":\"containership\",\"Url\":\"[GET] /api/vessels/123\",\"State\":\"\",\"Timestamp\":\"\"}],\"Endpoints\":[]}";


        public VesselCloudHandlers()
        {
        }

        public StringContent SerializeObject(PostJsonVesselClass obj)
        {
            //String r = JsonConvert.SerializeObject(obj);
            StringContent rest = new StringContent(JsonConvert.SerializeObject(obj));
            return rest;
        }

        public async Task<HttpResponseMessage> PostToCloud(PostJsonVesselClass obj)
        {
            StringContent rest = SerializeObject(obj);
            var resp = await httpclient.PostAsync(PostUri, rest);
            return resp;
        }

        public VesselClass DeserializeVessels(String vessels)
        {
            try
            {
                deserializedvessels = JsonConvert.DeserializeObject<VesselClass>(restContent);
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Alarm", "Deserialization fault " + ex, "OK");
            }
            return deserializedvessels;
        }

        public async Task<VesselClass> GetVesselsFromCloudAsync()
        {
       
            try
            {
                restContent = await httpclient.GetStringAsync(Url);
                //restContent = TestRestContent;
                //restContent = "{\"vessels\":[{\"ID\":\"123\",\"position-latitude\":\"60.379611\",\"position-longitude\":\"22.107599\",\"heading\":\"189\",\"speed\":\"17.2\",\"class\":\"containership\",\"Url\":\"[GET] /api/vessels/123\",\"State\":\"\",\"Timestamp\":\"\"}],\"Endpoints\":[]}";
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Alarm", "REST API fault " + ex, "OK");
            }
            deserializedvessels = DeserializeVessels(restContent);

            return deserializedvessels;
        }
    }
}
