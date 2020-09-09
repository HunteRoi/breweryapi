using AutoMapper;
using System.ComponentModel.DataAnnotations;
using DTO.Mapping;

namespace DTO.V1.WholesalerRequest
{
    public class Stock : IMapFrom<Models.V1.Stock>
    {
        [Required]
        public int BeerId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Models.V1.Stock, Stock>()
                .ForMember(d => d.BeerId, s => s.MapFrom(entity => entity.Beer.Id))
                .ForMember(d => d.Quantity, s => s.MapFrom(entity => entity.Quantity));
        }
    }
}
