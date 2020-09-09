using AutoMapper;
using System.ComponentModel.DataAnnotations;
using DTO.Mapping;

namespace DTO.V1.BreweryRequest
{
    public class Wholesaler : IMapFrom<Models.V1.Wholesaler>
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Models.V1.Wholesaler, Wholesaler>()
                .ForMember(d => d.Id, s => s.MapFrom(entity => entity.Id))
                .ForMember(d => d.Name, s => s.MapFrom(entity => entity.Name));
        }
    }
}
