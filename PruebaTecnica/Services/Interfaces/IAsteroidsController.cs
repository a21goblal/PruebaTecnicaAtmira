using Microsoft.AspNetCore.Mvc;

namespace PruebaTecnica.Services.Interfaces
{
    public interface IAsteroidsController
    {
        Task<IActionResult> GetAsteroids(string days);
    }
}
