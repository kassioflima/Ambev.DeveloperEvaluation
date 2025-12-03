using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts;

public class GetCartsRequest
{
    [FromQuery(Name = "_page")]
    public int? Page { get; set; }
    
    [FromQuery(Name = "_size")]
    public int? Size { get; set; }
    
    [FromQuery(Name = "_order")]
    public string? Order { get; set; }
    
    public Guid? UserId { get; set; }
    
    [FromQuery(Name = "_minDate")]
    public DateTime? MinDate { get; set; }
    
    [FromQuery(Name = "_maxDate")]
    public DateTime? MaxDate { get; set; }
}

