using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using PruebaTecnica.Controllers;
using System.Net;

namespace TestPruebaTecnica.Mock
{
    public class MockAsteridsController
    {
        //TODO: Sólo pruebas los códigos de respuesta, la gracia de mockear es que
        //pruebes que obtienes los objetos con las mismas propiedades que en el json
        [Test]
        public async Task GetAsteroids_ReturnsAListOfAsteroids()
        {
            //TODO: nada de rutas absolutas que me fallan los test jiji
            //además, evita duplicar la ruta en cada método de prueba
            string path = "C:\\Proyectos\\PRUEBAS DE NIVEL\\ALEJANDROPruebaTecnicaAtmira\\TestPruebaTecnica\\Resources\\MockResponse.json";

            using StreamReader jsonStream = File.OpenText(path);
            var json = jsonStream.ReadToEnd();

            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>();
            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            };

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            HttpClient httpClient = new HttpClient(handlerMock.Object);
            
            var asteroidsController = new AsteroidsController(httpClient);

            // Ejecutar el método GetAsteroids y obtener el resultado
            var result = await asteroidsController.GetAsteroids("7") as OkObjectResult;

            // Verificar que el resultado es una lista de tres elementos
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task GetAsteroids_ReturnsA400([Values("hola", "9", "-1")] string days)
        {
            string path = "C:\\Proyectos\\PRUEBAS DE NIVEL\\ALEJANDROPruebaTecnicaAtmira\\TestPruebaTecnica\\Resources\\MockResponse.json";
            using StreamReader jsonStream = File.OpenText(path);
            var json = jsonStream.ReadToEnd();

            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>();
            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            };

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            HttpClient httpClient = new HttpClient(handlerMock.Object);

            var asteroidsController = new AsteroidsController(httpClient);

            // Ejecutar el método GetAsteroids y obtener el resultado
            var result = await asteroidsController.GetAsteroids(days) as ObjectResult;

            //TODO: has copiado y pegado el mismo comentario y no tiene sentido
            // Verificar que el resultado es una lista de tres elementos
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task GetAsteroids_ReturnsA429()
        {
            Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>();
            HttpResponseMessage response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.TooManyRequests,
                ReasonPhrase = "Too many requests"
            };

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            HttpClient httpClient = new HttpClient(handlerMock.Object);

            var asteroidsController = new AsteroidsController(httpClient);

            // Ejecutar el método GetAsteroids y obtener el resultado
            var result = await asteroidsController.GetAsteroids("7") as ObjectResult;

            // Verificar que el resultado es lo esperado
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            Assert.That(result.StatusCode, Is.EqualTo(429));
        }
    }
}
