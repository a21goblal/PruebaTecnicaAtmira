using System.Numerics;

namespace PruebaTecnica.Models
{
    public class AproachData
    {
        public string close_approach_date { get; set; }
        public string close_approach_date_full { get; set; }
        public long epoch_date_close_approach { get; set; }
        public Velocity relative_velocity { get; set; }
        public Distance miss_distance { get; set; }
        public string orbiting_body { get; set; }
        public bool is_sentry_object { get; set; }
    }
}