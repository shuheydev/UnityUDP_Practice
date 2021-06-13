using System;

namespace DataModelFromPhone
{
    public class GeolocationInfo
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Altitude { get; set; }
        public double Timestamp { get; set; }
    }
}
