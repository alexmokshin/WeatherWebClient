using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using WeatherWebApplication.Models;

namespace WeatherWebApplication.Core
{
    public class AccuWeatherApi : WeatherApi
    {
        public AccuWeatherApi()
        {
            this.Token = "A1lsqa8gB6LC3N58eWhrOQA3U4zeLUZ4";
            this.Uri = "http://dataservice.accuweather.com";
        }

        public override async Task<Forecast> GetForecastOnDay(City city)
        {
            if (city == null)
                throw new Exception("City is not be null");

            Forecast forecast = new Forecast();
            forecast.city = city;
            string cityLocationNumber = GetCityNumber(city).Result;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response =
                    await client.GetAsync(GetForecastUri(cityLocationNumber)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var reponseJson = await response.Content.ReadAsStringAsync();
                        var responseArray = JArray.Parse(reponseJson);
                        dynamic jobject = (JObject)responseArray.FirstOrDefault();

                        forecast.Temp = (float) jobject.Temperature.Metric.Value;
                        forecast.Humidity = (float) jobject.RelativeHumidity;
                        forecast.Pressure = (float) jobject.Pressure.Metric.Value;
                    }
                }
            }

            return forecast;
        }

        private string GetForecastUri(string cityNumber)
        {
            return $"{this.Uri}/currentconditions/v1/{cityNumber}?apikey={this.Token}&details=true";
        }

        private string GetAccuWeatherCityNumberUri(City city)
        {
            if (city == null)
                throw new Exception("City is not be null");

            return $"{this.Uri}/locations/v1/cities/search?apikey={this.Token}&q={city.Name}&offset=1";
        }

        
        public async Task<string> GetCityNumber(City city)
        {
            if (city == null)
                throw new Exception("City is not be null");

            string cityNumber = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(GetAccuWeatherCityNumberUri(city)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var reponseJson = await response.Content.ReadAsStringAsync();
                        var responseArray = JArray.Parse(reponseJson);
                        JObject jobject = (JObject)responseArray.FirstOrDefault();

                        cityNumber = jobject.GetValue("Key").ToString();
                    }
                    else
                    {
                        throw new Exceptions.WeatherApiException(response);
                    }
                }
            }

            return cityNumber;
        }

        
    }
}
