using System;
namespace TestingGeo
{
    public class AlarmOrWarningClass
    {

        public bool AlarmsOn { get; set; }
        public bool WarningsOn { get; set; }

        public AlarmOrWarningClass()
        {
            this.AlarmsOn = false;
            this.WarningsOn = false;
        }
    }
}
