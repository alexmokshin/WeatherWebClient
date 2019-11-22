using NUnit.Framework;
using WeatherWebApplication.Core;
using WeatherWebApplication.Models;
using WeatherWebApplication.Pages;
using System;

namespace Tests
{
    public class Tests
    {
        public City city = new City() { Name = "Yekaterinburg" };
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestOpenWeatherMapApi()
        {
            WeatherApi api = new OpenWeatherMapApi();
            var response = api.GetForecastOnDay(city).Result;
            Assert.IsNotNull(response);
            Assert.NotZero(response.Temp);
            Assert.NotZero(response.Humidity);
            Assert.NotZero(response.Pressure);


        }

        [Test]
        public void TestAccuWeatherApi()
        {
            var accuweatherApi = new AccuWeatherApi(); 
            
            Forecast forecast = accuweatherApi.GetForecastOnDay(city).Result;
            
            Assert.NotZero(forecast.Temp);
            Assert.NotZero(forecast.Humidity);
            Assert.NotZero(forecast.Pressure);
        }


    }
}