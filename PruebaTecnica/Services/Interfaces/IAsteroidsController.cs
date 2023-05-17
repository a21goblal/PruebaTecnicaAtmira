using Microsoft.AspNetCore.Mvc;

namespace PruebaTecnica.Services.Interfaces
{
    //TODO: tienes una clase de controlador dentro de una carpeta de servicios, el archivo
    //que contiene la interfaz debe ir junto a su controlador, ya sea en una carpeta interfaces
    //o si la aplicación no tiene muchos controladores, en la misma carpeta de controladores
    public interface IAsteroidsController
    {
        Task<IActionResult> GetAsteroids(string days);
    }
}
