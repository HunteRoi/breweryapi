using AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DTO.Mapping;

namespace DTO.V1.BreweryRequest
{
    public class Brewery : IMapFrom<Models.V1.Brewery>
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Beer> Beers { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Models.V1.Brewery, Brewery>()
                .ForMember(d => d.Id, s => s.MapFrom(entity => entity.Id))
                .ForMember(d => d.Name, s => s.MapFrom(entity => entity.Name))
                .ForMember(d => d.Beers, s => s.MapFrom(entity => entity.Beers));
        }
    }
}
