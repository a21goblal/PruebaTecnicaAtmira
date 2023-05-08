namespace PruebaTecnica.Models
{
    public class GeneralInfo
    {
        public Link links { get; set; }
        public int element_count { get; set; }
        public Dictionary<string, NearEarthObjects[]> near_earth_objects { get; set; }
    }
}
