using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherWebApplication.Models
{
    public class ForecastViewModel
    {
        public List<City> cities { get; set; }
        public List<WeatherService> weatherServices { get; set; }
    }
}
