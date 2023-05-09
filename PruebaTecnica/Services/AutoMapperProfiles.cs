using AutoMapper;
using PruebaTecnica.Models;

namespace PruebaTecnica.Services
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            // Mapeo para los objetos NearEarthObjects (Entrante) y AsteroidViewModel (Saliente)
            CreateMap<NearEarthObjects, AsteroidViewModel>()
                    .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.name))
                    .ForMember(dest => dest.Diametro, opt => opt.MapFrom(src => (
                            src.estimated_diameter.kilometers.estimated_diameter_max +
                            src.estimated_diameter.kilometers.estimated_diameter_min) /
                            2))
                    .ForMember(dest => dest.Velocidad, opt => opt.MapFrom(src => 
                            src.close_approach_data[0].relative_velocity.kilometers_per_hour))
                    .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => 
                            src.close_approach_data[0].close_approach_date))
                    .ForMember(dest => dest.Planeta, opt => opt.MapFrom(src => 
                            src.close_approach_data[0].orbiting_body));
        }
    }
}
