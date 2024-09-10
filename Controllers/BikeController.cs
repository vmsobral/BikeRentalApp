using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeRentalApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BikeController : ControllerBase
{
    private readonly IBikeService _bikeService;

    public BikeController(IBikeService bikeService)
    {
        _bikeService = bikeService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBike([FromBody] Bike bike)
    {
        if (bike == null || string.IsNullOrWhiteSpace(bike.LicensePlate))
        {
            return BadRequest("Invalid bike data.");
        }

        var createdBike = await _bikeService.CreateBikeAsync(bike);
        return CreatedAtAction(nameof(GetBikeById), new { id = createdBike.Id }, createdBike);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBikes()
    {
        var bikes = await _bikeService.GetAllBikesAsync();
        return Ok(bikes);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBikeById(Guid id)
    {
        var bike = await _bikeService.GetBikeByIdAsync(id);
        if (bike == null)
        {
            return NotFound();
        }
        return Ok(bike);
    }

    [HttpGet("plate/{plate}")]
    public async Task<IActionResult> GetBikeByPlate(string plate)
    {
        var bike = await _bikeService.GetBikeByLicensePlateAsync(plate);
        if (bike == null)
        {
            return NotFound();
        }
        return Ok(bike);
    }

    [HttpPut("plate/{oldPlate}")]
    public async Task<IActionResult> UpdatePlate(string oldPlate, [FromBody] string newPlate)
    {
        await _bikeService.UpdateLicensePlateAsync(oldPlate, newPlate);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBike(Guid id)
    {
        await _bikeService.DeleteBikeAsync(id);
        return NoContent();
    }
}