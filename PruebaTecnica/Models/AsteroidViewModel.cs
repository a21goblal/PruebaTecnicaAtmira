namespace PruebaTecnica.Models
{
    public class AsteroidViewModel
    {
        //TODO: separa el DTO de los modelos de deserialización de json, puedes usar 2 carpetas diferentes
        public string Nombre { get; set; }
        public double Diametro { get; set; }
        public string Velocidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Planeta { get; set; }
    }
}
