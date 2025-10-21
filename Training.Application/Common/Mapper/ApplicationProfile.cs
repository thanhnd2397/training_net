using AutoMapper;

namespace Training.Application.Common.Mapper;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<CreateUserRequest, User>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => EncryptUtil.HashPassword(src.Password)))
            .ForMember(dest => dest.CreateDt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.DeleteFlg, opt => opt.MapFrom(_ => false))
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(_ => 2)); 
    }
}