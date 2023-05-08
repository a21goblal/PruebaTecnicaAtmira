using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaTecnica.Models;
using System.Runtime.CompilerServices;

namespace PruebaTecnica.Controllers
{
    [ApiController]
    public class AsteroidsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _autoMapper;
        private readonly string _today = DateTime.Today.ToString("yyyy-MM-dd");
        private readonly string _apiKey = "DEMO_KEY";

        public AsteroidsController(IMapper mapper)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.nasa.gov/neo/rest/v1/");
            _autoMapper = mapper;
        }

        [HttpGet("asteroids/{days}")]
        public async Task<IActionResult> Index(int days)
        {
            string endDate = DateTime.Today.AddDays(days).ToString("yyyy-MM-dd");
            // Solicitud HttpGet a la API
            HttpResponseMessage response = await _httpClient.GetAsync($"feed?start_date={_today}&end_date={endDate}&api_key={_apiKey}");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }

            // Leer la respuesta y convertirla en JSON
            string json = await response.Content.ReadAsStringAsync();

            // Convertir JSON en objeto
            var generalInfo = JsonConvert.DeserializeObject<GeneralInfo>(json);
            List<AsteroidViewModel> asteroidsList = new();

            foreach (string date in generalInfo.near_earth_objects.Keys)
            {
                foreach (NearEarthObjects nearObject in generalInfo.near_earth_objects[date])
                {
                    if(nearObject.is_potentially_hazardous_asteroid == true )
                    {
                        AsteroidViewModel model = _autoMapper.Map<AsteroidViewModel>(nearObject);
                        asteroidsList.Add(model);
                    }
                }
            }

            return Ok(asteroidsList
                .OrderByDescending(x => x.Diametro).Take(3));
        }
    }
}
