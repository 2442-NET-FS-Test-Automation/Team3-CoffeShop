using AutoMapper;
using CoffeShop.Controllers.DTOs;
using CoffeShop.Controllers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _service;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    public InventoryController(IInventoryService service, IMapper mapper, IMemoryCache cache)
    {
        _service = service;
        _mapper = mapper;
        _cache = cache;
    }

    [HttpGet]
    [ResponseCache(Duration = 30)]
    public async Task<ActionResult<IEnumerable<InventoryItemDto>>> Get()
    {
        var dtos = await _cache.GetOrCreateAsync("inventory:all", async entry =>
        {
            // Setting things about our cache entry - like "expire no matter what after 2 minutes"
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);

            // Actually get the items from DB 
            var items = await _service.AllAsync();

            // Return to front end (and also add to cache, since we're wrapped by _cache.GetOrCreateAsync)
            return _mapper.Map<List<InventoryItemDto>>(items);
        });

        return Ok(dtos);
    }

    [HttpGet("{sku}")]
    public async Task<ActionResult<InventoryItemDto>> GetBySku(string sku)
    {
        var item = await _service.BySkuAsync(sku);

        if (item is null)
        {
            return NotFound(); // 404 not found
        }
        else
        {
            var mappedItem = _mapper.Map<InventoryItemDto>(item);
            return Ok(mappedItem);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<InventoryItemDto>> Create(InventoryItemOpsDto newItem)
    {
        var created = await _service.AddAsync(newItem);
        var response = _mapper.Map<InventoryItemDto>(created);

        _cache.Remove("inventory:all");
        return CreatedAtAction(nameof(GetBySku), new { sku = response.Sku }, response);
    }

    [HttpPut]
    [Authorize(Roles = "Barista")]
    public async Task<ActionResult> Modify(InventoryItemOpsDto newItem)
    {
        var modified = await _service.ChangeAsync(newItem);
        var response = _mapper.Map<InventoryItemDto>(modified);

        _cache.Remove("inventory:all");

        return Ok("Object was updated");
    }

    [HttpDelete("{sku}")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult> Delete(string sku)
    {
        bool isDeleted = await _service.RemoveAsync(sku);

        if (isDeleted)
        {
            _cache.Remove("inventory:all");
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

}