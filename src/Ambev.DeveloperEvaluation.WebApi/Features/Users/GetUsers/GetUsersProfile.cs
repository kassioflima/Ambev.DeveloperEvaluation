using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users.GetUsers;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUsers;

/// <summary>
/// Profile for mapping between Application and API GetUsers responses
/// </summary>
public class GetUsersProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUsers feature
    /// </summary>
    public GetUsersProfile()
    {
        CreateMap<GetUsersRequest, GetUsersQuery>()
            .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page ?? 1))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size ?? 10));
        CreateMap<GetUsersResult, GetUsersResponse>();
        CreateMap<GetUserItemResult, GetUserItemResponse>();
        CreateMap<UserNameDto, UserNameResponse>();
        CreateMap<UserAddressDto, UserAddressResponse>();
        CreateMap<UserGeolocationDto, UserGeolocationResponse>();
    }
}

