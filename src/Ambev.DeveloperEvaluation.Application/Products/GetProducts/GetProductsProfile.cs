using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts;

/// <summary>
/// Profile for mapping between Product entity and GetProductItemResult
/// </summary>
public class GetProductsProfile : Profile
{
    public GetProductsProfile()
    {
        CreateMap<Product, GetProductItemResult>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating != null ? new ProductRatingDto
            {
                Rate = src.Rating.Rate,
                Count = src.Rating.Count
            } : null));
    }
}

