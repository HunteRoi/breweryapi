using AutoMapper;
using System.ComponentModel.DataAnnotations;
using DTO.Mapping;
using System;

namespace DTO.V1.WholesalerRequest
{
    public class Stock : IMapFrom<Models.V1.Stock>, IEquatable<Stock>
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int BeerId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Stock);
        }

        public bool Equals(Stock other)
        {
            if (other == null) return false;
            return BeerId == other.BeerId;
        }

        public override int GetHashCode()
        {
            return BeerId.GetHashCode();
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Models.V1.Stock, Stock>()
                .ForMember(d => d.BeerId, s => s.MapFrom(entity => entity.Beer.Id))
                .ForMember(d => d.Quantity, s => s.MapFrom(entity => entity.Quantity));
        }

        public Stock SetBeerId(int beerId)
        {
            BeerId = beerId;
            return this;
        }
    }
}
