using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts;

public class GetCartsProfile : Profile
{
    public GetCartsProfile()
    {
        CreateMap<Cart, GetCartItemResult>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
        CreateMap<CartItem, CartProductItemResult>();
    }
}

