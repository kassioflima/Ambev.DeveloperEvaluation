using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts;

public class GetProductsRequest
{
    [FromQuery(Name = "_page")]
    public int? Page { get; set; }
    
    [FromQuery(Name = "_size")]
    public int? Size { get; set; }
    
    [FromQuery(Name = "_order")]
    public string? Order { get; set; }
    
    public string? Title { get; set; }
    public string? Category { get; set; }
    
    public decimal? Price { get; set; }
    
    [FromQuery(Name = "_minPrice")]
    public decimal? MinPrice { get; set; }
    
    [FromQuery(Name = "_maxPrice")]
    public decimal? MaxPrice { get; set; }
}

