using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherWebApplication.Core;
using WeatherWebApplication.Models;

namespace WeatherWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private object Mutex = new object();
        #region Testing values
        List<City> Cities = new List<City>()
        {
            new City()
                {
                    LocalName = "Москва",
                    Name = "Moscow"
                },
                new City()
                {
                    LocalName = "Екатеринбург",
                    Name = "Yekaterinburg"
                },
                new City()
                {
                    LocalName = "Лондон",
                    Name = "London"
                },
                new City()
                {
                    LocalName = "Севастополь",
                    Name = "Sevastopol"

                }
        };
        List<WeatherService> WeatherServices = new List<WeatherService>()
            {
                new WeatherService()
                {
                    WeatherApi = new AccuWeatherApi()
                },
                new WeatherService()
                {
                    WeatherApi = new OpenWeatherMapApi()
                }
            };
        #endregion
        // GET: Home
        public ActionResult Index()
        {
            ForecastViewModel forecastView = new ForecastViewModel()
            {
                cities = Cities,
                weatherServices = WeatherServices
            };

            return View("\\Pages\\Index.cshtml", forecastView);
        }

        [HttpPost]
        public async Task<IActionResult> GetForecast(Dictionary<string,bool> shit)
        {
            var result = shit.Where(item => item.Value == true);

            List<Forecast> forecasts = new List<Forecast>();
            List<City> selectedCities = new List<City>();
            List<WeatherService> selectedServices = new List<WeatherService>();

            foreach (var p in result)
            {

                var selectedCity = Cities.FindLast(city => city.Name == p.Key);
                if (selectedCity == null)
                {
                    var selectedService = WeatherServices.FindLast(service => service.WeatherApi.WeatherServiceName == p.Key);
                    selectedServices.Add(selectedService);
                }
                else
                {
                    selectedCities.Add(selectedCity);
                }
            }

            foreach (var p in selectedCities)
            {
                foreach (var q in selectedServices)
                {
                    var forecast = await q.WeatherApi.GetForecastOnDay(p);
                    lock (Mutex)
                    {
                        forecasts.Add(forecast);
                    }
                }
            }

            return PartialView("\\Pages\\Forecasts.cshtml", forecasts);
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        
    }
}