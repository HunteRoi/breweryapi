using AutoMapper;
using DTO.Mapping;
using System.ComponentModel.DataAnnotations;

namespace DTO.V1.BeerRequest
{
    public class CreateBeer : IMapFrom<Models.V1.Beer>
    {
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
            profile.CreateMap<Models.V1.Beer, CreateBeer>()
                .ForMember(d => d.Name, s => s.MapFrom(entity => entity.Name))
                .ForMember(d => d.AlcoholLevel, s => s.MapFrom(entity => entity.AlcoholLevel))
                .ForMember(d => d.Price, s => s.MapFrom(entity => entity.Price))
                .ForMember(d => d.BreweryId, s => s.MapFrom(entity => entity.Brewery.Id));
        }

        public Beer Clone()
        {
            return (Beer)MemberwiseClone();
        }
    }
}
