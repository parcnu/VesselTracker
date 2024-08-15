using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace TestingGeo
{
    public class CustomMap : Xamarin.Forms.Maps.Map
    {
        public List<CustomPin> CustomPins { get; set; }
        public List<VesselDetailJson> MapVessels { get; set; } = new List<VesselDetailJson>();

        /* VesselIdAlreadyAdded
         * takes in VesselDelailJson type
         *
         * Checks if VesselID is already added in CustomMap.MapVessels List.
         * */
        public bool VesselIdAlreadyAdded(VesselDetailJson ves)
        {
            bool ret = false;
            //Parallel.For(0, this.MapVessels.Count - 1, (i, state) =>
            //  {
            //      if (ves.ID == this.MapVessels[i].ID)
            //      { ret = true; }
            //  });

            for(Int32 i=0; i<=this.MapVessels.Count - 1;i++)
            {
                if (this.MapVessels[i].ID.Contains(ves.ID) )
                { ret = true; }
            }

            return ret;
        }

        public Int32 FindPinIndexOf(VesselDetailJson vessel)
        {
            Int32 ret = -1;
            for (Int32 i = 0; i <= this.Pins.Count - 1; i++)
            {
                if (Convert.ToInt32(vessel.ID) == Convert.ToInt32(this.Pins[i].Address))
                {
                    ret = i;
                    break;
                }
                else
                {
                    // Nothing here
                }

            }
            return ret;
        }

        public bool AddPin(VesselDetailJson vessel)
        {
            bool pinadded = false;
            //await Task.Run(()=>
            //{
            Location pos = new Location(Convert.ToDouble(vessel.latitude), Convert.ToDouble(vessel.longitude));

            if (!VesselIdAlreadyAdded(vessel)) //AppPreferences.Mmsi != Convert.ToInt32(vessel.ID))
            {
                if (AppPreferences.NotMe(Convert.ToInt32(vessel.ID)))
                {
                    LabelClass label = new LabelClass();
                    label.ID = vessel.ID;
                    //label.Name = vesseldetails.Name;
                    label.Spd = vessel.sog;
                    label.Hdg = vessel.cog;
                    label.Vesselclass = vessel.vesselclass;
                    label.Latitude = pos.Latitude;
                    label.Longitude = pos.Longitude;

                    vessel.Pin = new CustomPin
                    {

                        Type = PinType.SearchResult,
                        Position = new Position(label.Latitude, label.Longitude),
                        VesselID = vessel.ID,
                        Address = vessel.ID,

                        Label = "Name:" + label.Name + " " +
                                "HDG: " + label.Hdg + " " +
                                "Spd: " + label.Spd + " " +
                                "Class:" + label.Vesselclass + " " +
                                "MaxSpd:" + label.MaxSpd + " " +
                                "Timest:" + label.Timestamp

                    };
                    this.Pins.Add(vessel.Pin);
                    this.MapVessels.Add(vessel);
                    pinadded = true;
                }

            }
            else
            {
                var index = this.FindPinIndexOf(vessel);
                if (index != -1)
                {
                    if (this.Pins[index].Position.Latitude.ToString() != vessel.latitude.ToString() || this.Pins[index].Position.Longitude.ToString() != vessel.longitude.ToString())
                    {
                        this.Pins[index].Position = new Position(pos.Latitude, pos.Longitude);
                    }

                    if (this.MapVessels[index].latitude.ToString() != vessel.latitude.ToString() || this.MapVessels[index].longitude.ToString() != vessel.longitude.ToString())
                    {
                        this.MapVessels[index].latitude = pos.Latitude.ToString();
                        this.MapVessels[index].longitude = pos.Longitude.ToString();
                    }

                }
                else
                {
                    //throw exeption
                }
            }

            return pinadded;

        }


        public AlarmOrWarningClass UpdateVesselsInAlarmAndWarningRange(Location ownloc)
        {
            AlarmOrWarningClass ret = new AlarmOrWarningClass();
            //Parallel.For(0, this.MapVessels.Count - 1, (i, state) =>
            //   {
            //       var distance = Location.CalculateDistance(ownloc.Latitude, ownloc.Longitude, Convert.ToDouble(this.MapVessels[i].latitude), Convert.ToDouble(this.MapVessels[i].longitude), DistanceUnits.Kilometers);
            //       this.MapVessels[i].DistanceToLocation = Distance.FromKilometers(distance);
            //       if (distance <= AlarmRange.Alarmrange.Kilometers && VesselSogNotZero(this.MapVessels[i].ID))
            //       {
            //           this.MapVessels[i].VesselInAlarmRange = true;
            //           ret.AlarmsOn = true;
            //       }
            //       else
            //       {
            //           this.MapVessels[i].VesselInAlarmRange = false;
            //       }

            //       if (distance <= AlarmRange.Warningrange.Kilometers && !this.MapVessels[i].VesselInAlarmRange && VesselSogNotZero(this.MapVessels[i].ID))
            //       {
            //           this.MapVessels[i].VesselInWarningRange = true;
            //           ret.WarningsOn = true;
            //       }
            //       else
            //       {
            //           this.MapVessels[i].VesselInWarningRange = false;
            //       }

            //   });

            for(Int32 i=0; i<=this.MapVessels.Count - 1;i++)
            {
                GeoCoordinate ownlocation = new GeoCoordinate(ownloc.Latitude,ownloc.Longitude);
                GeoCoordinate targetloc = new GeoCoordinate(Convert.ToDouble(this.Pins[i].Position.Latitude), Convert.ToDouble(this.Pins[i].Position.Longitude));
                //GeoCoordinate targetloc = new GeoCoordinate(Convert.ToDouble(this.MapVessels[i].latitude), Convert.ToDouble(this.MapVessels[i].longitude));
                Int32 distance = Convert.ToInt32(ownlocation.GetDistanceTo(targetloc));
                //var distance = Location.CalculateDistance(ownloc.Latitude, ownloc.Longitude, Convert.ToDouble(this.MapVessels[i].latitude), Convert.ToDouble(this.MapVessels[i].longitude), DistanceUnits.Kilometers);
                this.MapVessels[i].DistanceToLocation = distance;
                    
                if (distance <= AlarmRange.Alarmrange &&
                    !VesselSogZero(this.Pins[i].Address) && //MapVessels[i].ID) &&
                    AppPreferences.NotMe(Convert.ToInt32(this.Pins[i].Address))) //MapVessels[i].ID)))
                {
                    System.Diagnostics.Debug.WriteLine("*************************");
                    System.Diagnostics.Debug.WriteLine("Alarm set to ID " + this.Pins[i].Address + " i " + i + " Distance " + distance);
                    System.Diagnostics.Debug.WriteLine("*************************");
                    this.MapVessels[i].VesselInAlarmRange = true;
                    ret.AlarmsOn = true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Alarm unset to ID " + this.Pins[i].Address + " i " + i + " Distance " + distance);
                    this.MapVessels[i].VesselInAlarmRange = false;
                }
                   
                if (distance <= AlarmRange.Warningrange &&
                    !this.MapVessels[i].VesselInAlarmRange &&
                    //!VesselSogZero(this.MapVessels[i].ID) &&
                    AppPreferences.NotMe(Convert.ToInt32(this.Pins[i].Address))) //MapVessels[i].ID)))
                {
                    System.Diagnostics.Debug.WriteLine("Warning set to ID " + this.Pins[i].Address + " i " + i + " Distance " + distance);
                    this.MapVessels[i].VesselInWarningRange = true;
                    ret.WarningsOn = true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Warning unset to ID " + this.Pins[i].Address + " i " + i + " Distance " + distance);
                    this.MapVessels[i].VesselInWarningRange = false;
                }

            }

            return ret;
        }

        public Int32 GetClosestVessel()
        {
            /*Distance smallestDistance = Distance.FromKilometers(0);
            Parallel.For(0, this.MapVessels.Count - 1, (i, state) => { 
                  if (i == 0)
                  {
                      smallestDistance = this.MapVessels[i].DistanceToLocation;
                  }
                  else if (this.MapVessels[i].DistanceToLocation.Kilometers <= smallestDistance.Kilometers)
                  {
                      smallestDistance = this.MapVessels[i].DistanceToLocation;
                  }
                  else
                  {

                  }
              });
            return smallestDistance;*/

            Int32 smallestDistance = 0;// Distance.FromKilometers(0);
            for(Int32 i=0; i <= this.MapVessels.Count - 1; i++)
                {
                if (i == 0)
                {
                    smallestDistance = this.MapVessels[i].DistanceToLocation;
                }
                else if (this.MapVessels[i].DistanceToLocation <= smallestDistance)
                {
                    smallestDistance = this.MapVessels[i].DistanceToLocation;
                }
                else
                {

                }
            }
            return smallestDistance;
        }



		public Int32 GetUtmostVessel()
		{
            /*Distance smallestDistance = Distance.FromKilometers(0);
            Parallel.For(0, this.MapVessels.Count - 1, (i, state) => { 
                  if (i == 0)
                  {
                      smallestDistance = this.MapVessels[i].DistanceToLocation;
                  }
                  else if (this.MapVessels[i].DistanceToLocation.Kilometers <= smallestDistance.Kilometers)
                  {
                      smallestDistance = this.MapVessels[i].DistanceToLocation;
                  }
                  else
                  {

                  }
              });
            return smallestDistance;*/

            Int32 utmostDistance = 0; // Distance.FromKilometers(0);
			for (Int32 i = 0; i <= this.MapVessels.Count - 1; i++)
			{
				if (i == 0)
				{
					utmostDistance = this.MapVessels[i].DistanceToLocation;
				}
				else if (this.MapVessels[i].DistanceToLocation >= utmostDistance)
				{
					utmostDistance = this.MapVessels[i].DistanceToLocation;
				}
				else
				{

				}
			}
			return utmostDistance;
		}


		public double GetVesselSog(string id)
        {
            double vesselsog = 0.0;
            //Parallel.For(0, MapVessels.Count - 1, (i, state) =>
            //    {
            //        if (this.MapVessels[i].ID.Contains(id))
            //        {
            //            vesselsog = Convert.ToDouble(this.MapVessels[i].sog);
            //        }

            //    });

            for (Int32 i = 0; i <= MapVessels.Count - 1; i++)
            {
                if (this.MapVessels[i].ID.Contains(id))
                {
                    vesselsog = Convert.ToDouble(this.MapVessels[i].sog);
                }

            }

            return vesselsog;
        }

        public bool VesselSogZero(string id)
        {
            
            if (Math.Abs(GetVesselSog(id)) <= 0.0)
            {
                return true;
            }
            return false;
        }


        public double GetVesselCog(string id)
        {
            double vesselcog = 0.0;
            //Parallel.For(0, MapVessels.Count - 1, (i, state) =>
            //{
            //    if (this.MapVessels[i].ID.Contains(id))
            //    {
            //        vesselcog = Convert.ToDouble(this.MapVessels[i].sog);
            //    }

            //});

            for (Int32 i=0; i<= MapVessels.Count - 1; i++)
            {
                if (this.MapVessels[i].ID.Contains(id))
                {
                    vesselcog = Convert.ToDouble(this.MapVessels[i].sog);
                }

            }

            return vesselcog;
        }

    }
}
