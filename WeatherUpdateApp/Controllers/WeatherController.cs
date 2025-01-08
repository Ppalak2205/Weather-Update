//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Net.Mail;
//using WeatherUpdateApp.Data;
//using WeatherUpdateApp.Models;

//namespace WeatherUpdateApp.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class WeatherController : ControllerBase
//    {
//        private readonly WeatherContext _context;

//        public WeatherController(WeatherContext context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Weather>>> GetWeatherUpdates()
//        {
//            return await _context.WeatherUpdates.ToListAsync();
//        }

//        [HttpPost]
//        public async Task<ActionResult<Weather>> PostWeatherUpdate(Weather weather)
//        {
//            weather.UpdatedAt = DateTime.UtcNow;
//            _context.WeatherUpdates.Add(weather);
//            await _context.SaveChangesAsync();

//            SendEmail(weather);

//            return CreatedAtAction(nameof(GetWeatherUpdates), new { id = weather.Id }, weather);
//        }

//        private void SendEmail(Weather weather)
//        {
//            var fromAddress = new MailAddress("your-email@example.com", "Weather Update");
//            var toAddress = new MailAddress(weather.Email);
//            const string fromPassword = "your-email-password";
//            const string subject = "Weather Update";
//            string body = $"Weather update for {weather.City}: {weather.Temperature}°C, {weather.Condition}";

//            var smtp = new SmtpClient
//            {
//                Host = "smtp.example.com",
//                Port = 587,
//                EnableSsl = true,
//                DeliveryMethod = SmtpDeliveryMethod.Network,
//                UseDefaultCredentials = false,
//                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
//            };
//            using (var message = new MailMessage(fromAddress, toAddress)
//            {
//                Subject = subject,
//                Body = body
//            })
//            {
//                smtp.Send(message);
//            }
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Mail;
using WeatherUpdateApp.Data;
using WeatherUpdateApp.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GeoCoordinatePortable;
using System.Net.Sockets;
using System.Net.Http;

namespace WeatherUpdateApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WeatherController> _logger;
        private readonly HttpClient _httpClient;
        public WeatherController(HttpClient httpClient, ILogger<WeatherController> logger, WeatherContext context, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Weather>>> GetWeatherUpdates()
        {
            return await _context.WeatherUpdates.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Weather>> PostWeatherUpdate(Weather weather)
        {
            weather.UpdatedAt = DateTime.UtcNow;
            _context.WeatherUpdates.Add(weather);
            await _context.SaveChangesAsync();

            SendEmail(weather);

            return CreatedAtAction(nameof(GetWeatherUpdates), new { id = weather.Id }, weather);
        }

        private void SendEmail(Weather weather)
        {
            var fromAddress = new MailAddress("your-email@example.com", "Weather Update");
            var toAddress = new MailAddress(weather.Email);
            const string fromPassword = "your-email-password";
            const string subject = "Weather Update";
            string body = $"Weather update for {weather.City}: {weather.Temperature}°C, {weather.Condition}";

            var smtp = new SmtpClient
            {
                Host = "smtp.example.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        [HttpGet("get-location")]
        //public async Task<IActionResult> GetLocation()
        //{
        //    try
        //    {
        //        // Fetch the public IP address of the current system using ipify service
        //        var client = _httpClientFactory.CreateClient();
        //        var ipResponse = await client.GetStringAsync("https://api.ipify.org");
        //        var ipAddress = ipResponse.Trim();  // The public IP address of the client

        //        // Log the fetched IP address
        //        _logger.LogInformation("Fetched public IP address: {IpAddress}", ipAddress);

        //        // Use the public IP to fetch location information
        //        //var locationResponse = await client.GetStringAsync($"http://ip-api.com/json/{ipAddress}");
        //        var locationResponse = await client.GetStringAsync($"http://ip-api.com/json/{ipAddress}?key=your-api-key");
        //        var location = JsonConvert.DeserializeObject<LocationInfo>(locationResponse);

        //        if (location == null || location.Status != "success")
        //        {
        //            _logger.LogWarning("Unable to fetch location information for IP address: {IpAddress}", ipAddress);
        //            return BadRequest("Unable to fetch location information.");
        //        }

        //        // Log successful location fetching
        //        _logger.LogInformation("Successfully fetched location for IP address: {IpAddress}. Location: {City}, {Region}, {Country}",
        //            ipAddress, location.City, location.RegionName, location.Country);

        //        var continent = await GetContinentFromCoordinates(location.Lat, location.Lon);

        //        var locationDto = new LocationDTO
        //        {
        //            City = location.City,
        //            Region = location.RegionName,
        //            Country = location.Country,
        //            Continent = location.Continent,
        //            Area = location.Org                };

        //        return Ok(locationDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error
        //        _logger.LogError(ex, "Error fetching location for IP address.");
        //        return BadRequest($"Error fetching location: {ex.Message}");
        //    }
        //}
        public async Task<IActionResult> GetLocation()
        {
            // Get the current public IP address or provide a specific one
            string ipAddress = ""; // Leave it empty to use the current IP address
            string url = string.IsNullOrEmpty(ipAddress)
                ? "http://ip-api.com/json/"
                : $"http://ip-api.com/json/{ipAddress}";

            try
            {
                // Fetch data from ip-api
                var locationResponse = await _httpClient.GetStringAsync(url);
                var location = JsonConvert.DeserializeObject<LocationInfo>(locationResponse);

                if (location.Status == "fail")
                {
                    return BadRequest($"Error fetching location: {location.Country}");
                }

                // Log and display the location
                Console.WriteLine($"Latitude: {location.Lat}, Longitude: {location.Lon}");

                return Ok(location); // You can return the location data as a JSON response
            }
            catch (HttpRequestException ex)
            {
                // Handle potential network errors or 403 errors
                Console.WriteLine($"Request failed: {ex.Message}");
                return StatusCode(500, "Internal server error while fetching location.");
            }
        }

        // Method to reverse geocode using latitude and longitude and GeoCoordinate
        //private async Task<string> GetContinentFromCoordinates(double lat, double lon)
        //{
        //    try
        //    {
        //        // Use GeoCoordinate to create a coordinate object
        //        var coordinate = new GeoCoordinate(lat, lon);

        //        // Use a reverse geocoding API like Nominatim to get location details
        //        var client = _httpClientFactory.CreateClient();
        //        var geoResponse = await client.GetStringAsync($"https://nominatim.openstreetmap.org/reverse?lat={lat}&lon={lon}&format=json");

        //        var geoLocation = JsonConvert.DeserializeObject<GeoLocation>(geoResponse);

        //        // Check if the continent exists in the response and return it
        //        if (geoLocation != null && geoLocation.Address != null)
        //        {
        //            return geoLocation.Address.Continent ?? "Unknown Continent";
        //        }
        //        return "Unknown Continent";
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error fetching continent information from coordinates.");
        //        return "Unknown Continent";
        //    }
        //}
    }
}

