using AutoMapper;
using UserManagement.Application.DTOs.Requests;
using UserManagement.Application.DTOs.Responses;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserRequest, User>()
                    .ForMember(d => d.Id, opt => opt.Ignore())
                    .ForMember(d => d.PasswordHash, opt => opt.Ignore())
                    .ForMember(d => d.DateCreated, opt => opt.Ignore())
                    .ForMember(d => d.DateModified, opt => opt.Ignore())
                    .ForMember(d => d.Clients, opt => opt.Ignore());

            CreateMap<UpdateUserRequest, User>()
                    .ForMember(d => d.UserName, opt => opt.Ignore())
                    .ForMember(d => d.PasswordHash, opt => opt.Ignore())
                    .ForMember(d => d.DateCreated, opt => opt.Ignore())
                    .ForMember(d => d.DateModified, opt => opt.Ignore())
                    .ForMember(d => d.Clients, opt => opt.Ignore());

            CreateMap<User, UserResponse>();
            CreateMap<User, UserLoginResponse>()
                    .ForMember(d => d.ApiKey, opt => opt.MapFrom(src => src.Clients.FirstOrDefault() != null
                                                           ? src.Clients.First().ApiKey
                                                           : string.Empty));
        }
    }
}
