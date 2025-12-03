using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

public class GetProductsProfile : Profile
{
    public GetProductsProfile()
    {
        CreateMap<GetProductsRequest, GetProductsQuery>()
            .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page ?? 1))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size ?? 10));
        CreateMap<GetProductsResult, GetProductsResponse>();
        CreateMap<GetProductItemResult, GetProductItemResponse>();
        CreateMap<ProductRatingDto, ProductRatingResponse>();
    }
}

