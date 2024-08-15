using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

/*
 * AlarmRange class
 * Holds Alarm range triggers and methods for application.
 * Static class which can be clled from anywhere inside namespace.
 * 
 **/

namespace TestingGeo
{
    public static class AlarmRange
    {
        private static Int32 crashrange = 10;
        private static Int32 criticalrange = 100;
        private static Int32 alarmrange = 300; //meters //Distance.FromKilometers(0.3);//km
        private static Int32 warningrange = 600; //meters//Distance.FromKilometers(0.6); //km
       // private static List<String> vessellist = new List<string>();
       // private static List<String> warninglist = new List<String>();
       // private static List<String> vesselAndTresholdlist = new List<string>();
        private static double treshold = 0.1; //km


        /*
         * Alarmrange property
         * Holds Alarm range value.
         * VesselDetailsJson & VesselClass holds alarm value by each object.
         **/
        public static Int32 Alarmrange
        {
            get
            {
                return alarmrange;
            }
            set
            {
                alarmrange = Convert.ToInt32(value);
            }
        }

		/*
         * Warningrange property
         * Holds Warningrange value.
         * VesselDetailsJson & VesselClass holds alarm value by each object.
         **/
		public static Int32 Warningrange
        {
            get
            {
                return warningrange;
            }
            set
            {
                warningrange = Convert.ToInt32(value);
            }
        }

        /*
         * Treshold property
         * Holds Treshold range value which can be used for removing vesssel from Alarm Range.
         * 
         **/
        public static double Treshold
        {
            get
            {
                return treshold;
            }
            set
            {
                treshold = Convert.ToDouble(value);
            }
        }

        public static Int32 Crashrange
        {
            get
            {
                return crashrange;
            }
            set
            {
                crashrange = Convert.ToInt32(value);
            }
        }

        public static Int32 Criticalrange
        {
            get
            {
                return criticalrange;
            }
            set
            {
                criticalrange = Convert.ToInt32(value);
            }
        }

    }
}
