using AutoMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DTO.Mapping;

namespace DTO.V1.WholesalerRequest
{
    public class Wholesaler : IMapFrom<Models.V1.Wholesaler>
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Stock> Stocks { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Models.V1.Wholesaler, Wholesaler>()
                .ForMember(d => d.Id, s => s.MapFrom(entity => entity.Id))
                .ForMember(d => d.Name, s => s.MapFrom(entity => entity.Name))
                .ForMember(d => d.Stocks, s => s.MapFrom(entity => entity.Stocks));        
        }
    }
}
