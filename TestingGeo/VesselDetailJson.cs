using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Xamarin.Forms.Maps;

namespace TestingGeo
{
    public class VesselDetailJson:INotifyPropertyChanged
    {
        
        [JsonProperty("ID")]
        public string ID { get; set; }

        [JsonProperty("position-latitude")]
        public string latitude { get; set; }

        [JsonProperty("position-longitude")]
        public string longitude { get; set; }

        [JsonProperty("cog")]
        public string cog { get; set; }

        [JsonProperty("sog")]
        public string sog { get; set; }

        [JsonProperty("heading")]
        public string heading { get; set; }

        [JsonProperty("speed")]
        public string speed { get; set; }

        [JsonProperty("class")]
        public string vesselclass { get; set; }

        [JsonProperty("Url")]
        public string url { get; set; }

        [JsonProperty("State")]
        public string state { get; set; }

        [JsonProperty("Timestamp")]
        public string timestamp { get; set; }

        public CustomPin Pin { get; set; }

        private Int32 distToLoc = 0;

        public Int32 DistanceToLocation
        {
            get
            {
                return distToLoc;
            }
            set
            {
                distToLoc = value;
            }
        }


        public VesselPropertyChanged VesselPropertyChanged { get; set; } = new VesselPropertyChanged();

        private bool vesselInAlarmRange = false;
        public bool VesselInAlarmRange
        {
            get
            {
                return this.vesselInAlarmRange;
            }
            set
            {
                if (this.vesselInAlarmRange != value &&
                    AppPreferences.NotMe(Convert.ToInt32(this.ID)))
                {
                    this.vesselInAlarmRange = value;
                    if (value == true)
                    {
                        OnVesselInAlarmRangePropertyChanged(this.ID + ":" +"A" + ":" + "T");
                    }
                    else
                    {
                        OnVesselInAlarmRangePropertyChanged(this.ID + ":" +"A" + ":" + "F");
                    }
                }
            }
        }

        private bool vesselInWarningRange = false;
        public bool VesselInWarningRange
        {
            get
            {
                return this.vesselInWarningRange;
            }
            set
            {
                if (this.vesselInWarningRange != value &&
                    AppPreferences.NotMe(Convert.ToInt32(this.ID)))
                {
                    this.vesselInWarningRange = value;
                    if (value == true)
                    {
                        OnVesselInAlarmRangePropertyChanged(this.ID + ":" + "W" + ":" + "T");
                    }
                    else
                    {
                        OnVesselInAlarmRangePropertyChanged(this.ID + ":" + "W" + ":" + "F");
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnVesselInAlarmRangePropertyChanged(String id)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(id));

        }
    }
}
