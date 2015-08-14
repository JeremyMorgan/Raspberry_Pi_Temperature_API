using System;

namespace TemperatureCheck.Models
{
    public class Status
    {
        public float TempFahrenheit { get; set; }
        public float TempCelcius { get; set; }
        public float Humidity { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}