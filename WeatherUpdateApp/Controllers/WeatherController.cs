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
using System.Net.Mail;
using WeatherUpdateApp.Data;
using WeatherUpdateApp.Models;

namespace WeatherUpdateApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherContext _context;

        public WeatherController(WeatherContext context)
        {
            _context = context;
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
    }
}

