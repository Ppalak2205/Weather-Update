using Microsoft.EntityFrameworkCore;
using WeatherUpdateApp.Models;

namespace WeatherUpdateApp.Data
{
    public class WeatherContext : DbContext
    {
        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options) { }

        public DbSet<Weather> WeatherUpdates { get; set; }
    }
}
