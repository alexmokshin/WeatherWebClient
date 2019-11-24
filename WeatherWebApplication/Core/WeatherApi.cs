using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherWebApplication.Core
{
    public abstract class WeatherApi
    {
        public Uri Url { get; set; }

        public string WeatherServiceName { get; set; }

        protected string Token { get; set; }

        public bool ServiceAvailable { get; set; } = true;

        public abstract Task<Models.Forecast> GetForecastOnDay(Models.City city);

    }
}
