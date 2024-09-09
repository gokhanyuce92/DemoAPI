using AutoMapper;
using Demo.DTOs.ControllerActionRole;
using Demo.Entities;

namespace Demo.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddControllerActionRoleRequestDTO, ControllerActionRole>().ReverseMap();
        }
    }
}