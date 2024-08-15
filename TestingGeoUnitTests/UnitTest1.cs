using NUnit.Framework;
using TestingGeo;
using Xamarin.Essentials;

namespace TestingGeoUnitTests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        
       /* [Test]
        [Category("AlarmRange")]
        public void AlarmRange_AddNonexistentVesselInAlarmList()
        {
            AlarmRange.Clear();
            AlarmRange.AddVesselInAlarmList("123");
            bool expected = true;
            bool actual = AlarmRange.VesselInAlarmList("123");

            Assert.AreEqual(expected, actual, "AlarmRange_AddNonexixtentVesselInAlarmList failed - expecting " + expected + " instead of " + actual);
        }*/

        

        [TestCase]
        [Category("CustomMap")]
        public void CustomMap_UpdateVesselsInAlarmAndWarningRange()
        {
            Location ownloc = new Location() { Latitude = 12.3456, Longitude = 65.4321 };

        }
    }

}
