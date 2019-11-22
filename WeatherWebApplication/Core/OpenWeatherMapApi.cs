using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using WeatherWebApplication.Models;

namespace WeatherWebApplication.Core
{
    public class OpenWeatherMapApi : WeatherApi
    {
        public OpenWeatherMapApi() : base()
        {
            this.WeatherServiceName = "OpenWeatherMap";
            this.Token = "9b70f4ad5c7d79452694701a8d81c1b3";
            this.Url = new Uri("http://api.openweathermap.org/data/2.5/weather");
        }

        public override async Task<Forecast> GetForecastOnDay(Models.City city)
        {
            var forecast = new Models.Forecast();
            forecast.city = city ?? throw new Exception("City is not be null");
            forecast.WeatherApi = this;

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response =
                    await client.GetAsync(this.Url + "?q=" + city.Name + "&units=metric&appid=" + this.Token))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var reponseJson = await response.Content.ReadAsStringAsync();
                        dynamic responseObj = Newtonsoft.Json.Linq.JObject.Parse(reponseJson);
                        forecast.Temp = (float) responseObj.main.temp;
                        forecast.Humidity = (float) responseObj.main.humidity;
                        forecast.Pressure = (float) responseObj.main.pressure;
                    }
                    else
                    {
                        throw new Exceptions.WeatherApiException(response);
                    }
                }
            }


            return forecast;
        }

        
    }
}
