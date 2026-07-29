using AutoMapper;
using CoffeShop.Controllers.DTOs;
using CoffeShop.Data.Entities;

namespace CoffeShop.Controllers.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<InventoryItem, InventoryItemDto>()
            .ForCtorParam("ProductId", o => o.MapFrom(s => s.ProductId))
            .ForCtorParam("Sku", o => o.MapFrom(s => s.product!.Sku))
            .ForCtorParam("Name", o => o.MapFrom(s => s.product!.Name))
            .ForCtorParam("Price", o => o.MapFrom(s => s.product.Price));
    }

}
