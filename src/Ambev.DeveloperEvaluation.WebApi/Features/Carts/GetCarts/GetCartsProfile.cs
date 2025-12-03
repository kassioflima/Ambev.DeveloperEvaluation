using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;

public class GetCartsProfile : Profile
{
    public GetCartsProfile()
    {
        CreateMap<GetCartsRequest, GetCartsQuery>()
            .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page ?? 1))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size ?? 10));
        CreateMap<GetCartsResult, GetCartsResponse>();
        CreateMap<GetCartItemResult, GetCartItemResponse>();
        CreateMap<CartProductItemResult, CartProductItemResponse>();
    }
}

