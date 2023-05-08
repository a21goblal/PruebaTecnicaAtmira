
using Newtonsoft.Json;

namespace PruebaTecnica.Models
{
    public class AsteroidViewModel
    {
        public string Nombre { get; set; }
        public double Diametro { get; set; }
        public double Velocidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Planeta { get; set; }
    }
}
