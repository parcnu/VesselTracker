using System;
namespace TestingGeo
{
   
    public class LabelClass
    {
        private String name;
        private String id;
        private String vesselclass;
        private String hdg;
        private String spd;
        private String maxspd;
        private String state;
        private String timestamp;
  


        public String Name
        {
            get
            {
                if (name != null && name != "")
                {
                    return name;
                }
                else
                {
                    return "NA";
                }
            }
            set
            {
                if (value != null && value != "")
                {
                    name = value;
                }
                else
                {
                    name = "NA";
                }
            }
        }
        public String ID
        {
            get
            {
                if (id != null && id != "")
                {
                    return id;
                }
                else
                {
                    return "NA";
                }
            }
            set
            {
                if (value != null && value != "")
                {
                    id = value;
                }
                else
                {
                    id = "NA";
                }
            }
        }
        public String Vesselclass
        {
            get
            {
                if (vesselclass != null && vesselclass != "")
                {
                    return vesselclass;
                }
                else
                {
                    return "NA";
                }
            }
            set
            {
                if (value != null && value != "")
                {
                    vesselclass = value;
                }
                else
                {
                    vesselclass = "NA";
                }
            }
        }
        public String Hdg
        {
            get
            {
                if (hdg != null && hdg != "")
                {
                    return hdg;
                }
                else
                {
                    return "NA";
                }
            }
            set
            {
                if (value != null && value != "")
                {
                    hdg = value;
                }
                else
                {
                    hdg = "NA";
                }
            }
        }
        public String Spd
        {
            get
            {
                if (spd != null && spd != "")
                {
                    return spd;
                }
                else
                {
                    return "NA";
                }
            }
            set
            {
                if (value != null && value != "")
                {
                    spd = value;
                }
                else
                {
                    spd = "NA";
                }
            }
        }
        public String MaxSpd
        {
            get
            {
                if (maxspd != null && maxspd != "")
                {
                    return maxspd;
                }
                else
                {
                    return "NA";
                }
            }
            set
            {
                if (value != null && value !="")
                {
                    maxspd = value;
                }
                else
                {
                    maxspd = "NA";
                }
            }
        }
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }

        public String State
        {
            get
            {
                if (state != null && state != "")
                {
                    return state;
                }
                else
                {
                    return "NA";
                }
            }
            set
            {
                if (value != null && value != "")
                {
                    state = value;
                }
                else
                {
                    state = "NA";
                }
            }
        }

        public String Timestamp
        {
            get
            {
                if (timestamp != null && timestamp != "")
                {
                    return timestamp;
                }
                else
                {
                    return "NA";
                }
            }
            set
            {
                if (value != null && value != "")
                {
                    timestamp = value;
                }
                else
                {
                    timestamp = "NA";
                }
            }
        }

    }
}
