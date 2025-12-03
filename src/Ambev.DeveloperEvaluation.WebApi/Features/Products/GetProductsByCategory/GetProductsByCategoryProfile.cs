using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.GetProductsByCategory;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductsByCategory;

public class GetProductsByCategoryProfile : Profile
{
    public GetProductsByCategoryProfile()
    {
        CreateMap<GetProductsByCategoryRequest, GetProductsByCategoryQuery>()
            .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page ?? 1))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size ?? 10));
        CreateMap<GetProductsByCategoryResult, GetProductsByCategoryResponse>();
    }
}

