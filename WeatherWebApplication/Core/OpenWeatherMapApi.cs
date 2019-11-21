using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WeatherWebApplication.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace WeatherWebApplication.Core
{
    public class OpenWeatherMapApi : WeatherApi
    {
        private static HttpClient client = new HttpClient();
        public OpenWeatherMapApi()
        {
            this.Token = "9b70f4ad5c7d79452694701a8d81c1b3";
            this.Uri = "http://api.openweathermap.org/data/2.5/weather";
        }
        public override async Task<Forecast> GetForecastOnDay(Models.City city)
        {
            if (city == null)
                throw new Exception("City is not be null");

            var forecast = new Models.Forecast();

            HttpResponseMessage response = await client.GetAsync(this.Uri + "?q=" + city.Name + "&units=metric&appid=" + this.Token);
            if (response.IsSuccessStatusCode)
            {
                
                var reponseJson = await response.Content.ReadAsStringAsync();
                dynamic responseObj = Newtonsoft.Json.Linq.JObject.Parse(reponseJson);
                forecast.Temp = (float)responseObj.main.temp;
                forecast.city = city;
                forecast.Humidity = (float)responseObj.main.humidity;
                forecast.Pressure = (float)responseObj.main.pressure;
            }


            return forecast;
        }

        
    }
}
