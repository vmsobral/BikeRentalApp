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
        if (bike == null || string.IsNullOrWhiteSpace(bike.Plate))
        {
            return BadRequest("Invalid bike data.");
        }

        var createdBike = await _bikeService.CreateBikeAsync(bike);
        return CreatedAtAction(nameof(GetBikeById), new { id = createdBike.Id }, createdBike);
    }

    [HttpGet]
    public IActionResult GetAllBikes()
    {
        var bikes = _bikeService.GetAllBikes();
        return Ok(bikes);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetBikeById(Guid id)
    {
        var bike = _bikeService.GetBikeById(id);
        if (bike == null)
        {
            return NotFound();
        }
        return Ok(bike);
    }

    [HttpGet("plate/{plate}")]
    public IActionResult GetBikeByPlate(string plate)
    {
        var bike = _bikeService.GetBikeByPlate(plate);
        if (bike == null)
        {
            return NotFound();
        }
        return Ok(bike);
    }

    [HttpPut("plate/{oldPlate}")]
    public IActionResult UpdatePlate(string oldPlate, [FromBody] string newPlate)
    {
        try
        {
            _bikeService.UpdatePlate(oldPlate, newPlate);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBike(Guid id)
    {
        try
        {
            _bikeService.DeleteBike(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
        }
    }
}