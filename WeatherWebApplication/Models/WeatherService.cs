using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherWebApplication.Models
{
    public class WeatherService
    {
        public String Name { get; set; }

        public Core.WeatherApi WeatherApi { get; set; }
    }
}
