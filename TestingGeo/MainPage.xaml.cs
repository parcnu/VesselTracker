using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Plugin.Geolocator;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace TestingGeo
{

    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private VesselClass deserializedvessels = new VesselClass();
        private VesselCloudHandlers cloudHand = new VesselCloudHandlers();
        private Int32 count = 0;
        private Location lastRecievedPosition = new Location { Latitude = 0.0, Longitude = 0.0, Timestamp = new DateTimeOffset(DateTime.UtcNow) };
        private CustomMap customMap = new CustomMap { MapType = MapType.Street };
        private CustomPin pin = new CustomPin();
        private StackLayout stack = new StackLayout { Spacing = 0 };
        private Location ownlocation;
        private List<VesselDetailJson> previousvessels = new List<VesselDetailJson>();
        private VesselCloudHandlers vesselCloud = new VesselCloudHandlers();

        /*
         * SerializePostJson
         * Object to be serialized and own location as parameter.
         * 
         * On testing purposes checks the Platform and creats MMSI based on platform.
         * Serializes the object and Posts that to Cloud.
         * */

        public async Task<bool> SerializePostJson(PostJsonVesselClass obj,Location ownloc)
        {
            //System.Math.Round(value, 2);
            if (Device.RuntimePlatform == Device.iOS)
            {
                obj.properties.sog = Convert.ToInt32(11);
                obj.properties.heading = Convert.ToInt32(111);
                obj.properties.timestamp = Convert.ToInt32(11);
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                obj.properties.sog = Convert.ToInt32(22);
                obj.properties.heading = Convert.ToInt32(222);
                obj.properties.timestamp = Convert.ToInt32(22);
            }

            obj.mmsi = AppPreferences.Mmsi;
            obj.geometry.coordinates[0] = System.Math.Round(Convert.ToDouble(ownloc.Longitude), 6);
            obj.geometry.coordinates[1] = System.Math.Round(Convert.ToDouble(ownloc.Latitude), 6);

            obj.properties.timestampExternal = (long)DateTime.Now.MillisecondsTimestamp();

            var ret = await vesselCloud.PostToCloud(obj);
            if (ret.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /*
         * updateTargets
         * no parameters
         * 
         * Reads and updates targets from REST api and desetializes values to vessel object.
         * Creates Dictionary of the targets.
         * Creates pins for each target.
         *
         * */
        public async Task updateTargetsAsync(CustomMap map)
        {
            deserializedvessels = await cloudHand.GetVesselsFromCloudAsync();
            previousvessels = deserializedvessels.vessels;
            if (previousvessels == null)
            {
                previousvessels = deserializedvessels.vessels;
            }
            for (Int32 i = 0; i <= deserializedvessels.vessels.Count() - 1; i++)
            {
                 bool pinadded = map.AddPin(deserializedvessels.vessels[i]);
                 if (pinadded)
                 {
                     Int32 index = map.MapVessels.IndexOf(deserializedvessels.vessels[i]);
                     map.MapVessels[index].PropertyChanged += VesselInAlarmRange_PropertyChanged;
                     map.MapVessels[index].VesselPropertyChanged.Index = index;
                     map.MapVessels[index].VesselPropertyChanged.Bind = true;
                 }
                //if (!map.MapVessels[i].VesselPropertyChanged.Bind && !pinadded)
                //{
                //    Int32 index = map.MapVessels.IndexOf(serializedvessels.vessels[i]);
                //    map.MapVessels[index].VesselPropertyChanged.Index = index;
                //    map.MapVessels[index].VesselPropertyChanged.Bind = true;
                //}
            }
           
            map.UpdateVesselsInAlarmAndWarningRange(ownlocation);
            customMap = map;
        }


        /*
         * updateMap
         * Location and customMap as paramaters.
         * Redraws map containing new own location and new targets (customMap.Pins)
         * Uses getDistance functionnality to calculate map to show all targets (Pins)
         *
         * */
        public async Task updateMapAsync(Location loc, CustomMap map)
        {
            AlarmOrWarningClass ret = new AlarmOrWarningClass();
            Distance displaydist = Distance.FromKilometers(1);
            Int32 utmostVessel = map.GetUtmostVessel(); //getFarthestDistance(loc, map);
            await Task.Run(() =>
            {
                ret = map.UpdateVesselsInAlarmAndWarningRange(loc);
                

                if (utmostVessel <= 500)//distances.FarthestVesselDistance < 0.5)
                {
                    displaydist = Distance.FromMeters(1000);
                }
                else if (ret.AlarmsOn)
                {
                    displaydist = Distance.FromMeters(map.GetClosestVessel() + 500);
                }
                else if (ret.WarningsOn)
                {
                    displaydist = Distance.FromMeters(map.GetClosestVessel() + 500);
                }
                else
                {
                    displaydist = Distance.FromMeters(1000); // distances.FarthestVesselDistance);
                }
            });
            map.MoveToRegion(
                        MapSpan.FromCenterAndRadius(
                        new Position(loc.Latitude, loc.Longitude), displaydist));

                map.IsShowingUser = true;
                map.HeightRequest = 100;
                map.WidthRequest = 960;
                map.VerticalOptions = LayoutOptions.FillAndExpand;
                stack.Children.Add(map);
                Content = stack;
        }


        /*
         * MainPage
         * Starts background listener for GPS.
         * Minimum time interval 1 sec
         * Minimum location change to trigger new location 0.5 meters.
         *
         * */
        public MainPage()
        {
            InitializeComponent();
            try
            {
                if (!CrossGeolocator.Current.IsListening)
                {
                    CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(1), 0.5);
                }
            }
            catch (Exception ex)
            {
                    DisplayAlert("Alarm", "Something went wrong " + ex, "OK");
            }
        }

        /*
         * Sleep to generate sleep for wanted millisec time.
         *
         * */
        public static async Task Sleep(int ms)
        {
            await Task.Delay(ms);
            System.Diagnostics.Debug.WriteLine("Sleep " + ms + " ms");
        }


        /*
         * OnAppearing
         * Will be triggered when application comes visible.
         * Updates targets found from REST.
         * Binds position chnage events and error events
         * finds ownlocation and updates map accorginto targets and own location.
         * 
         * */
        protected async override void OnAppearing() 
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                AppPreferences.Mmsi = Convert.ToInt32(123456789);
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                AppPreferences.Mmsi = Convert.ToInt32(987654321);
            }
                try
            {
                CrossGeolocator.Current.PositionChanged += CrossGeolocator_Current_PositionChanged;
                CrossGeolocator.Current.PositionError += CrossGeolocator_Current_PositionError;
                await Sleep(1000);
                var location = CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(1)).Result;
                ownlocation = new Location(Convert.ToDouble(location.Latitude), Convert.ToDouble(location.Longitude));
                System.Diagnostics.Debug.WriteLine("Location read in OnApprearing\n" + "Time " + location.Timestamp + " Lat " + ownlocation.Latitude + " Long " + ownlocation.Longitude + " Alt " + location.Altitude);
                _ = updateTargetsAsync(customMap);
                _ = updateMapAsync(ownlocation, customMap);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alarm", "Something went wrong! " + ex, "OK");
            }
            base.OnAppearing();
        }


        /*
         * VesselInAlarmRange_PropertyChanged
         * Receives events when Vessels closer than AlarmRange.Alarmrange.
         * Events are sent from VesselDetailJson
         * Sets DisplayActionSheet
         * 
         * */
        private async void VesselInAlarmRange_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //":W:T" Warning True
            //":A:T" Alarm True
            // 0=ID 1=A/W 2=T/F

            System.Diagnostics.Debug.WriteLine("Property changed\n" + e.PropertyName);
            String[] str = e.PropertyName.Split(':');
            if (str[1] == "A")
            {
                if (str[2] == "T")
                {
                    if (MainThread.IsMainThread)
                    {
                        var action = await this.DisplayActionSheet("Vessel too Close", null, null, str[0], "OK");
                        System.Diagnostics.Debug.WriteLine("Display Action shown " + action);
                    }
                    else
                    {
                        MainThread.BeginInvokeOnMainThread(
                            () =>
                            { 
                                var action = DisplayActionSheet("Vessel too Close", null, null, str[0], "OK");
                                System.Diagnostics.Debug.WriteLine("Display Action shown " + action);
                            });
                    }
                }
                else if (str[2] == "F")
                {
                    //Int32 index = ((VesselDetailJson)sender).VesselPropertyChanged.Index;
                    //((VesselDetailJson)sender).VesselPropertyChanged.Bind = false;
                    //customMap.MapVessels[index].PropertyChanged -= VesselInAlarmRange_PropertyChanged;
                }
            }
            else if (str[2] == "T" && str[1] == "W")
            {
                System.Diagnostics.Debug.WriteLine("Property changed: Just a warning");
            }
        }


        /*
         * CrossGeolocator_Current_PositionError
         * Event that is triggered when some error happens in GeoLocations.
         * 
         * */
        public async void CrossGeolocator_Current_PositionError(object sender, Plugin.Geolocator.Abstractions.PositionErrorEventArgs e)
        {

            await DisplayAlert("Error", "Someting went wrong \n " + e.Error,"OK");
        }


        /*
         * CrossGeolocator_Current_PositionChanged
         * Event that is triggered when new location based on GPS is available
         * updates new targets.
         * gets new location and updates tha map.
         * 
         * */
        public async void CrossGeolocator_Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            PostJsonVesselClass obj = new PostJsonVesselClass();
            await SerializePostJson(obj, new Location(Convert.ToDouble(e.Position.Latitude), Convert.ToDouble(e.Position.Longitude)));
            if (e.Position.Timestamp == lastRecievedPosition.Timestamp)
            { // Reject it we got a double fire !!!
                count++;
            }
            else
            {
                lastRecievedPosition.Timestamp = e.Position.Timestamp;
                ownlocation = new Location(Convert.ToDouble(e.Position.Latitude), Convert.ToDouble(e.Position.Longitude));
                _ = updateTargetsAsync(customMap);
                System.Diagnostics.Debug.WriteLine("Location changed\n" + "Time " + e.Position.Timestamp + " Lat " + ownlocation.Latitude + " Long " + ownlocation.Longitude + " Alt " + e.Position.Altitude);
                _ = updateMapAsync(ownlocation, customMap);
            }
            
        }
        
        

        /*
         * OnDisappearing
         * Event that is triggered when MainPage disappears.
         * removes binding and stops listener. 
         * 
         * */
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            try
            {
                CrossGeolocator.Current.PositionChanged -= CrossGeolocator_Current_PositionChanged;
                CrossGeolocator.Current.PositionError -= CrossGeolocator_Current_PositionError;
                try
                {
                    if (CrossGeolocator.Current.IsListening)
                    {
                        CrossGeolocator.Current.StopListeningAsync();
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("Alarm", "Something went wrong! " + ex, "OK");
                }
                
            }
            catch(Exception ex)
            {
                DisplayAlert("Alarm", "Something went wrong! " + ex, "OK");
            }

        }    
    }
}
