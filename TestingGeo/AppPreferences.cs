using System;
using Xamarin.Forms.Maps;

namespace TestingGeo
{
    public static class AppPreferences
    {
        private static Int32 mmsi = 123456789;

        public static Int32 Mmsi {
            get
            {
                return mmsi;
            }
            set
            {
                if (GetIntegerDigitCountString(Convert.ToInt32(value)) == 9)
                {
                    mmsi = Convert.ToInt32(value);
                }
                else
                {
                    throw new System.ArgumentException("MMSI sould be 9 digits " + value);
                }
            }
        }

        static int GetIntegerDigitCountString(int val)
        {
            return val.ToString().Length;
        }

        public static Distance WarningRange { get; set; }
        public static Distance AlarmRange { get; set; }


        public static void WriteToDisk()
        {

        }

        public static void ReadFromDisk()
        {

        }

        public static bool NotMe(Int32 ID)
        {
            if (Convert.ToInt32(ID) != Mmsi)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
