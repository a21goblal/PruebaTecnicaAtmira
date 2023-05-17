using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Controllers;
using PruebaTecnica.Services;

namespace TestPruebaTecnica
{
    [TestFixture]
    public class AsteroidsControllerTests
    {
        // Controlador necesario para las pruebas
        private AsteroidsController _controller;

        // Configuración necesaria para las pruebas
        [SetUp]
        public void SetUp()
        {
            HttpClient client = new HttpClient();
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfiles>();
            });
            //TODO: este mapper no hace nada
            IMapper mapper = config.CreateMapper();

            _controller = new AsteroidsController(client);
        }

        // En este test se comprueba el correcto funcionamiento para los datos correctos.
        [Test]
        public async Task GetAsteroids_TestOk()
        {
            // Arrange
            string days = "5";

            // Act
            var result = await _controller.GetAsteroids(days);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }

        // En este test se comprueba el correcto funcionamiento para los datos fuera de rango.
        [TestCase("-2")]
        [TestCase("8")]
        public async Task GetAsteroids_TestWrong_InvalidDays(string days)
        {
            // Act
            var result = await _controller.GetAsteroids(days);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        // En este test se comprueba el correcto funcionamiento para los datos nulos.
        [Test]
        public async Task GetAsteroids_TestWrong_NullDays()
        {
            // Act
            var result = await _controller.GetAsteroids(null);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        // En este test se comprueba el correcto funcionamiento para los datos con tipo incorrecto.
        [Test]
        public async Task GetAsteroids_TestWrong_StringDays()
        {
            // Arrange
            string days = "Hola";

            // Act
            var result = await _controller.GetAsteroids(days);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }
    }
}