using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

/// <summary>
/// Profile for mapping between Product entity and UpdateProductResult
/// </summary>
public class UpdateProductProfile : Profile
{
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductCommand, Domain.Entities.Product>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        CreateMap<Domain.Entities.Product, UpdateProductResult>();
    }
}

