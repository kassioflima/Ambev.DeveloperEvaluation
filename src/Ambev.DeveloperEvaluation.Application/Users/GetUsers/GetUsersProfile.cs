using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Profile for mapping between User entity and GetUserItemResult
/// </summary>
public class GetUsersProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUsers operation
    /// </summary>
    public GetUsersProfile()
    {
        CreateMap<User, GetUserItemResult>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name != null ? new UserNameDto
            {
                Firstname = src.Name.Firstname,
                Lastname = src.Name.Lastname
            } : null))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address != null ? new UserAddressDto
            {
                City = src.Address.City,
                Street = src.Address.Street,
                Number = src.Address.Number,
                Zipcode = src.Address.Zipcode,
                Geolocation = src.Address.Geolocation != null ? new UserGeolocationDto
                {
                    Lat = src.Address.Geolocation.Lat,
                    Long = src.Address.Geolocation.Long
                } : null
            } : null));
    }
}

