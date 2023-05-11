using Microsoft.AspNetCore.Mvc;
using Moq;
using PruebaTecnica.Models;
using PruebaTecnica.Services.Interfaces;

namespace TestPruebaTecnica.Mock
{
    public class MockAsteridsController
    {
        [Test]
        public async Task GetAsteroids_ReturnsAListOfAsteroids()
        {
            // Lista mockeada de la API
            List<AsteroidViewModel> asteroids = new() 
            {
                new AsteroidViewModel
                {
                    Nombre = "Asteroid 1",
                    Diametro = 1000,
                    Planeta = "Earth",
                    Velocidad = "2563.2563"
                },
                new AsteroidViewModel
                {
                    Nombre = "Asteroid 2",
                    Diametro = 2000,
                    Planeta = "Earth",
                    Velocidad = "2563.2563"
                },
                new AsteroidViewModel
                {
                    Nombre = "Asteroid 3",
                    Diametro = 3000,
                    Planeta = "Earth",
                    Velocidad = "2563.2563"
                }
            };

            // Instancia de la interfaz del controlador
            Mock<IAsteroidsController> mockController = new();

            // Configuración del método GetAsteroids para que devuelva la lista de asteroides
            mockController.Setup(c => c.GetAsteroids(It.IsAny<string>()))
                .ReturnsAsync(new OkObjectResult(asteroids));

            // Se obtiene el resultado del método GetAsteroids
            IActionResult result = await mockController.Object.GetAsteroids("3");

            // Se verifica que se ha devuelto una respuesta Ok y que tiene los asteroides simulados
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            OkObjectResult okResult = (OkObjectResult)result;
            List <AsteroidViewModel> resultAsteroids = (List<AsteroidViewModel>)okResult.Value;
            Assert.That(resultAsteroids, Has.Count.EqualTo(3));
            Assert.That(resultAsteroids[0].Diametro, Is.EqualTo(1000));
            Assert.That(resultAsteroids[1].Nombre, Is.EqualTo("Asteroid 2"));
        }
    }
}
