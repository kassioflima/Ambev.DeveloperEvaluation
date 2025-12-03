using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

public class UpdateProductProfile : Profile
{
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductRequest, UpdateProductCommand>()
            .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 
                src.Rating != null ? new ProductRating(src.Rating.Rate, src.Rating.Count) : null));
        CreateMap<UpdateProductResult, UpdateProductResponse>();
        CreateMap<ProductRating, ProductRatingResponse>();
    }
}

