namespace WeatherUpdateApp.Models
{
    public class Weather
    {
        public int Id { get; set; }
        public string City { get; set; }
        public double Temperature { get; set; }
        public string Condition { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
