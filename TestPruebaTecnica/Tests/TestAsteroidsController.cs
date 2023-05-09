using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Controllers;
using PruebaTecnica.Models;
using PruebaTecnica.Services;
using System.Reflection.Metadata;

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
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<AutoMapperProfiles>();
            });
            IMapper mapper = config.CreateMapper();

            _controller = new AsteroidsController(mapper);
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
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        // En este test se comprueba el correcto funcionamiento para los datos fuera de rango.
        [TestCase("-2")]
        [TestCase("8")]
        public async Task GetAsteroids_TestWrong_InvalidDays(string days)
        {
            // Act
            var result = await _controller.GetAsteroids(days);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        // En este test se comprueba el correcto funcionamiento para los datos nulos.
        [Test]
        public async Task GetAsteroids_TestWrong_NullDays()
        {
            // Arrange
            string days = null;

            // Act
            var result = await _controller.GetAsteroids(days);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
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
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}