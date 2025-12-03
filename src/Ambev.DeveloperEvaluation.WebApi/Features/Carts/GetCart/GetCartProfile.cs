using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

public class GetCartProfile : Profile
{
    public GetCartProfile()
    {
        CreateMap<Guid, GetCartQuery>().ConstructUsing(id => new GetCartQuery(id));
        CreateMap<GetCartRequest, GetCartQuery>().ConstructUsing(req => new GetCartQuery(req.Id));
        CreateMap<GetCartResult, GetCartResponse>();
        CreateMap<CartProductResult, CartProductResponse>();
    }
}

