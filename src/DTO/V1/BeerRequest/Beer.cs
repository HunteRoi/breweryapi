using AutoMapper;
using DTO.Mapping;
using System.ComponentModel.DataAnnotations;

namespace DTO.V1.BeerRequest
{
    public class Beer : IMapFrom<Models.V1.Beer>
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal AlcoholLevel { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int BreweryId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Models.V1.Beer, Beer>()
                .ForMember(d => d.Id, s => s.MapFrom(entity => entity.Id))
                .ForMember(d => d.Name, s => s.MapFrom(entity => entity.Name))
                .ForMember(d => d.AlcoholLevel, s => s.MapFrom(entity => entity.AlcoholLevel))
                .ForMember(d => d.Price, s => s.MapFrom(entity => entity.Price))
                .ForMember(d => d.BreweryId, s => s.MapFrom(entity => entity.Brewery.Id));
        }
    }
}
