using AutoMapper;

namespace DTO.Mapping
{
    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }
}
