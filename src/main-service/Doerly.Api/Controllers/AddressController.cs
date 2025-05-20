using Doerly.DataAccess;
using Doerly.Infrastructure.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doerly.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressController : BaseApiController
{
    private readonly AddressDbContext _addressDbContext;

    public AddressController(AddressDbContext addressDbContext)
    {
        _addressDbContext = addressDbContext;
    }

    [HttpGet("regions/{countryId}")]
    public async Task<IActionResult> GetRegions()
    {
        var regions = await _addressDbContext.Regions
            .Select(r => new
            {
                r.Id,
                r.Name
            })
            .ToListAsync();

        return Ok(regions);
    }

    [HttpGet("cities/{regionId}")]
    public async Task<IActionResult> GetCities(int regionId)
    {
        var cities = await _addressDbContext.Cities
            .Where(c => c.RegionId == regionId)
            .Select(c => new
            {
                c.Id,
                c.Name
            })
            .ToListAsync();

        return Ok(cities);
    }

    [HttpGet("streets/{cityId}")]
    public async Task<IActionResult> GetStreets(int cityId)
    {
        var streets = await _addressDbContext.Streets
            .Where(s => s.CityId == cityId)
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.ZipCode
            })
            .ToListAsync();

        return Ok(streets);
    }
} 