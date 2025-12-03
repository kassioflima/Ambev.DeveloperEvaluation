using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CreateProductProfile : Profile
{
    public CreateProductProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 
                src.Rating != null ? new ProductRating(src.Rating.Rate, src.Rating.Count) : null));
        CreateMap<CreateProductResult, CreateProductResponse>();
        CreateMap<ProductRating, ProductRatingResponse>();
    }
}

