using Newtonsoft.Json;

namespace WeatherUpdateApp.Models
{
    public class LocationDTO
    {
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Continent { get; set; }
        public string Area { get; set; }
    }

    public class LocationInfo
    {
        public string Status { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string Region { get; set; }
        public string RegionName { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public string Timezone { get; set; }
        public string Isp { get; set; }
        public string Org { get; set; }
        public string As { get; set; }
        public string Query { get; set; }
    }

    public class Address
    {
        [JsonProperty("continent")]
        public string Continent { get; set; }
    }

    // Class for reverse geocoding response
    public class GeoLocation
    {
        [JsonProperty("address")]
        public Address Address { get; set; }
    }
}
