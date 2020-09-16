using AutoMapper;
using System.ComponentModel.DataAnnotations;
using DTO.Mapping;
using System;

namespace DTO.V1.WholesalerRequest
{
    public class UpdateStock : IMapFrom<Models.V1.Stock>
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Models.V1.Stock, UpdateStock>()
                .ForMember(d => d.Quantity, s => s.MapFrom(entity => entity.Quantity));
        }

        public Stock Clone()
        {
            return (Stock)MemberwiseClone();
        }
    }
}
