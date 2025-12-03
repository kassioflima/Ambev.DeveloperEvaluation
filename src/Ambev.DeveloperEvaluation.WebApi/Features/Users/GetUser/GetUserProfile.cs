using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;

/// <summary>
/// Profile for mapping GetUser feature requests to commands
/// </summary>
public class GetUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUser feature
    /// </summary>
    public GetUserProfile()
    {
        CreateMap<Guid, GetUserCommand>()
            .ConstructUsing(id => new GetUserCommand(id));
        CreateMap<GetUserResult, GetUserResponse>()
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
