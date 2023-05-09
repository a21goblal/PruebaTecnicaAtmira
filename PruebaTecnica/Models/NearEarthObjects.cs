namespace PruebaTecnica.Models
{
    public class NearEarthObjects
    {
        public LinkObject links { get; set; }
        public string id { get; set; }
        public string neo_reference_id { get; set; }
        public string name { get; set; }
        public string nasa_jpl_url { get; set; }
        public double absolute_magnitude_h { get; set; }
        public Diameter estimated_diameter { get; set; }
        public bool is_potentially_hazardous_asteroid { get; set; }
        public AproachData[] close_approach_data { get; set; }
        public bool is_sentry_object { get; set; }
    }
}