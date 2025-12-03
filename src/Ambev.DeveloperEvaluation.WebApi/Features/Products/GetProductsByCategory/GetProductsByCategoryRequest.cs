using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProductsByCategory;

public class GetProductsByCategoryRequest
{
    public string Category { get; set; } = string.Empty;
    
    [FromQuery(Name = "_page")]
    public int? Page { get; set; }
    
    [FromQuery(Name = "_size")]
    public int? Size { get; set; }
    
    [FromQuery(Name = "_order")]
    public string? Order { get; set; }
}

