using AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DTO.Mapping;

namespace DTO.V1.BreweryRequest
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

        public IEnumerable<Wholesaler> Wholesalers { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Models.V1.Beer, Beer>()
                .ForMember(d => d.Id, s => s.MapFrom(entity => entity.Id))
                .ForMember(d => d.Name, s => s.MapFrom(entity => entity.Name))
                .ForMember(d => d.AlcoholLevel, s => s.MapFrom(entity => entity.AlcoholLevel))
                .ForMember(d => d.Price, s => s.MapFrom(entity => entity.Price))
                .ForMember(d => d.Wholesalers, s => s.MapFrom(entity => entity.Stocks.Select(s => s.Wholesaler)));
        }
    }
}
