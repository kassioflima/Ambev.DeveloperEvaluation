using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser;

/// <summary>
/// Profile for mapping between Application and API UpdateUser responses
/// </summary>
public class UpdateUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateUser feature
    /// </summary>
    public UpdateUserProfile()
    {
        CreateMap<UpdateUserRequest, UpdateUserCommand>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => 
                Enum.Parse<UserStatus>(src.Status)))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => 
                Enum.Parse<UserRole>(src.Role)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => 
                src.Name != null ? new UserName(src.Name.Firstname, src.Name.Lastname) : null))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => 
                src.Address != null ? new UserAddress(
                    src.Address.City,
                    src.Address.Street,
                    src.Address.Number,
                    src.Address.Zipcode,
                    src.Address.Geolocation != null ? new UserGeolocation(
                        src.Address.Geolocation.Lat,
                        src.Address.Geolocation.Long) : null) : null));

        CreateMap<UpdateUserResult, UpdateUserResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => 
                src.Name != null ? new UserNameResponse
                {
                    Firstname = src.Name.Firstname,
                    Lastname = src.Name.Lastname
                } : null))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => 
                src.Address != null ? new UserAddressResponse
                {
                    City = src.Address.City,
                    Street = src.Address.Street,
                    Number = src.Address.Number,
                    Zipcode = src.Address.Zipcode,
                    Geolocation = src.Address.Geolocation != null ? new UserGeolocationResponse
                    {
                        Lat = src.Address.Geolocation.Lat,
                        Long = src.Address.Geolocation.Long
                    } : null
                } : null));
    }
}

