using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TestingGeo
{

    public class Geometry
    {
        private String _type = "Point";

        [JsonProperty("type")]
        public String type {
            get
            {
                return _type;
            }
            set
            {
                _type = value.ToString();
            }
        }
        public List<Double> coordinates = new List<Double>();

        public Geometry()
        {
            coordinates.Add(123);
            coordinates.Add(321);
        }
    }

    public class GeoProperties
	{
        private Double _sog = 0.0;
        private Double _cog = 0.0;
        private Int32 _navStat = 0;
        private Int32 _rot = 0;
        private bool _posAcc = false;
        private bool _raim = false;
        private Int32 _heading = 0;
        private Int32 _timestamp = 22;
        private long _externalTimestamp = 0000;

		[JsonProperty("mmsi")]
		public Int32 mmsi {
            get
            {
                return Convert.ToInt32(GlobMmsi.globmmsi);
            }
            set
            {
                GlobMmsi.globmmsi = Convert.ToInt32(value);
            }
        }
		[JsonProperty("sog")]
		public Double sog {
            get
            {
                return Convert.ToDouble(_sog);
            }
            set
            {
                _sog = Convert.ToDouble(value);
            }
        }
        [JsonProperty("cog")]
        public Double cog {
            get
            {
                return Convert.ToDouble(_cog);
            }
            set
            {
                _cog = Convert.ToDouble(value);
            }
        }
        [JsonProperty("navStat")]
		public Int32 navStat {
            get
            {
                return Convert.ToInt32(_navStat);
            }
            set
            {
                _navStat = Convert.ToInt32(value);
            }
        }
        [JsonProperty("rot")]
        public Int32 rot
        {
            get
            {
                return Convert.ToInt32(_rot);
            }
            set
            {
                _rot = Convert.ToInt32(value);
            }
        }
        [JsonProperty("posAcc")]
        public bool posAcc {
            get
            {
                return _posAcc;
            }
            set
            {
                _posAcc = value;
            }
        }
        [JsonProperty("raim")]
        public bool raim {
            get
            {
                return _raim;
            }
            set
            {
                _raim = value;
            }
        }
        [JsonProperty("heading")]
        public Int32 heading {
            get
            {
                return Convert.ToInt32(_heading);
            }
            set
            {
                _heading = Convert.ToInt32(value);
            }
        }
        [JsonProperty("timestamp")]
        public Int32 timestamp {
            get
            {
                return Convert.ToInt32(_timestamp);
            }
            set
            {
                _timestamp = Convert.ToInt32(value);
            }
        }
        [JsonProperty("timestampExternal")]
        public long timestampExternal {
            get
            {
                return (long)_externalTimestamp;
            }
            set
            {
                _externalTimestamp = (long)value;
            }
        }
	}

    public static class GlobMmsi
    {
        public static Int32 globmmsi { get; set; }
    }

	public class PostJsonVesselClass
    {

        [JsonProperty("mmsi")]
        public Int32 mmsi {
            get
            {
                return Convert.ToInt32(GlobMmsi.globmmsi);
            }
            set
            {
                GlobMmsi.globmmsi = Convert.ToInt32(value);
            }
        }

        private String _type = "Feature";
        [JsonProperty("type")]
        public String type {
            get
            {
                return _type;
            }
            set
            {
                _type = value.ToString();
            }
            
        }
        [JsonProperty("geometry")]
        public Geometry geometry { get; set; } = new Geometry();

        [JsonProperty("properties")]
        public GeoProperties properties { get; set; } = new GeoProperties();

	}
}
