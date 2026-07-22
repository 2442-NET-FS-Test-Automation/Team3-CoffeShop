using AutoMapper;
using CoffeShop.Controllers.DTOs;
using CoffeShop.Data.Entities;

namespace CoffeShop.Controllers.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<InventoryItem, InventoryItemDto>()
            .ForCtorParam("Sku", o => o.MapFrom(s => s.product!.Sku))
            .ForCtorParam("Name", o => o.MapFrom(s => s.product!.Name));
    }

}