using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaTecnica.Models;
using PruebaTecnica.Services;
using PruebaTecnica.Services.Interfaces;
using System.Net.Http;

namespace PruebaTecnica.Controllers
{
    [ApiController]
    public class AsteroidsController : Controller, IAsteroidsController
    {
        // Cliente Http para realizar las consultas a la API
        private readonly HttpClient _httpClient;

        // AutoMapper, necesario para convertir los resultados obtenidos en el ViewModel
        private readonly IMapper _autoMapper;

        // Variable con la fecha de hoy parseada, implementada como variable para futuras
        // ampliaciones
        private readonly string _today = DateTime.Today.ToString("yyyy-MM-dd");

        // Variable con la key de la API, implementada como variable para futuras modificaciones
        // de código.
        private readonly string _apiKey = "DEMO_KEY";     

        public AsteroidsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfiles>();
            });
            IMapper mapper = config.CreateMapper();
            _autoMapper = mapper;
        }


        /*
         * Método GetAsteroids
         * -------------------
         * Devuelve una lista de los tres asteroides más grandes con potencial de riesgo de impacto
         * en el planeta Tierra entre el día de hoy y la fecha obtenida al sumar en días el valor
         * introducido como parámetro.
         * 
         * Ejemplo: /asteroids?days=3
         * 
         * Valores admitidos: Entre 1 y 7
         */
        [HttpGet("asteroids")]
        public async Task<IActionResult> GetAsteroids(string days)
        {
            try
            {
                //TODO: las respuestas de control de errores deben devolverse con un formato json,
                //ahora solo devuelve un texto plano
                // Condicional que controla si se ha introducido valor.
                if (days is null)
                {
                    throw new ArgumentNullException(nameof(days), 
                        "El valor del parámetro days no puede ser nulo.");
                }

                // Condicional que controla si se ha introducido número.
                // En caso de ser correcto, se lo asigna a la variable "parsedDays"
                int parsedDays;
                if(!int.TryParse(days, out parsedDays))
                {
                    throw new ArgumentNullException(nameof(days), 
                        "El valor del parámetro days debe ser un número.");
                }

                // Condicional que comprueba el rango de números a sumar
                if (parsedDays < 1 || parsedDays > 7)
                {
                    throw new ArgumentOutOfRangeException(nameof(days), 
                        "Introduce un número entre 1 y 7.");
                }

                // Variable con la fecha final formateada al valor requerido por la URL
                string endDate = DateTime.Today.AddDays(parsedDays).ToString("yyyy-MM-dd");

                //TODO: extrae la petición a la API y la lógica de parseo/filtrado a un servicio
                //PETICIÓN A LA API

                //TODO: no hardcodees
                // Solicitud HttpGet
                HttpResponseMessage response = await _httpClient
                    .GetAsync($"https://api.nasa.gov/neo/rest/v1/feed?start_date={_today}&end_date={endDate}&api_key={_apiKey}");

                // Controlador para devolver una excepción en caso de respuesta fallida.
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, $"Ha ocurrido un error: {response.StatusCode}");
                }

                // Leer la respuesta y convertirla en JSON
                string json = await response.Content.ReadAsStringAsync();

                // Convertir JSON en objeto GeneralInfo
                GeneralInfo generalInfo = JsonConvert.DeserializeObject<GeneralInfo>(json);

                // Declaración de la lista con el ViewModel para agregarle elementos posteriormente
                List<AsteroidViewModel> asteroidsList = new();

                // Bucle que recorre todas las claves
                foreach (string date in generalInfo.near_earth_objects.Keys)
                {
                    // Bucle que recorre todos los valores dentro de cada clave
                    foreach (NearEarthObjects nearObject in generalInfo.near_earth_objects[date])
                    {
                        // Si el valor tiene potencial riesgo de impacto, se Mapea a ViewModel
                        // y se agrega a la lista "asteroidsList"
                        if (nearObject.is_potentially_hazardous_asteroid == true )
                        {
                            AsteroidViewModel model = _autoMapper
                                .Map<AsteroidViewModel>(nearObject);
                            asteroidsList.Add(model);
                        }
                    }
                }

                //TODO: controla el mensaje de respuesta http cuando la lista que devuelve es vacía, actualmente devuelve
                //200 y hay otro statuscode para una petición buena con respuesta vacía, investigalo
                /*
                 * Se devuelve la lista con los siguientes requisitos:
                 *  - Todos los elementos cuyo planeta sea "Earth"
                 *  - Ordenado de mayor a menor por Diametro
                 *  - Los tres primeros.
                 */
                return Ok(asteroidsList
                    .Where(y => y.Planeta == "Earth")
                    .OrderByDescending(x => x.Diametro)
                    .Take(3));
            }

            // Catch separados por cada excepción para futuras posibles modificaciones.
            catch (ArgumentOutOfRangeException error)
            {
                return BadRequest(error.Message);
            }
            catch(ArgumentNullException error)
            {
                return BadRequest(error.Message);
            }
        }
    }
}
