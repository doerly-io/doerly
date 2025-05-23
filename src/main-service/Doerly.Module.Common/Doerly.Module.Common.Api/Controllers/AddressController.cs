using Doerly.Domain.Models;
using Doerly.Infrastructure.Api;
using Doerly.Module.Common.DataAccess.Address;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Common.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : BaseApiController
{
    private readonly AddressDbContext _addressDbContext;

    public AddressController(AddressDbContext addressDbContext)
    {
        _addressDbContext = addressDbContext;
    }

    [HttpGet("regions")]
    public async Task<IActionResult> GetRegions()
    {
        var regions = await _addressDbContext.Regions
            .Select(r => new
            {
                r.Id,
                r.Name
            })
            .ToListAsync();

        return Ok(HandlerResult.Success(regions));
    }
    
    [HttpGet("regions/{regionId}/cities")]
    public async Task<IActionResult> GetRegionCities(int regionId)
    {
        var cities = await _addressDbContext.Cities
            .Where(c => c.RegionId == regionId)
            .Select(c => new
            {
                c.Id,
                c.Name
            })
            .ToListAsync();

        return Ok(HandlerResult.Success(cities));
    }
    
    [HttpGet("regions/{regionId}/cities/{cityId}")]
    public async Task<IActionResult> GetCity(int regionId, int cityId)
    {
        var city = await _addressDbContext.Cities
            .Where(c => c.RegionId == regionId && c.Id == cityId)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.RegionId,
                RegionName = c.Region.Name
            })
            .FirstOrDefaultAsync();

        if (city == null)
        {
            return NotFound();
        }

        return Ok(HandlerResult.Success(city));
    }

    [HttpGet("regions/{regionId}/cities/{cityId}/streets")]
    public async Task<IActionResult> GetStreets(int regionId, int cityId)
    {
        var streets = await _addressDbContext.Streets
            .Where(s => s.CityId == cityId && s.City.RegionId == regionId)
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.ZipCode
            })
            .ToListAsync();

        return Ok(HandlerResult.Success(streets));
    }
} 
