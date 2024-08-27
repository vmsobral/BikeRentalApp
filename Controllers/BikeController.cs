using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

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
    public IActionResult CreateBike([FromBody] Bike bike)
    {
        if (bike == null || string.IsNullOrWhiteSpace(bike.Plate))
        {
            return BadRequest("Invalid bike data.");
        }

        var createdBike = _bikeService.AddBike(bike);
        return CreatedAtAction(nameof(GetBikeById), new { id = createdBike.Id }, createdBike);
    }

    [HttpGet]
    public IActionResult GetAllBikes()
    {
        var bikes = _bikeService.GetAllBikes();
        return Ok(bikes);
    }

    [HttpGet("{id}")]
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

    [HttpPut("plate/{plate}")]
    public IActionResult UpdatePlate(string plate)
    {
        var bike = _bikeService.UpdatePlate(plate);
        if (bike == null)
        {
            return NotFound();
        }

        return Ok(bike);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBike(Guid id)
    {
        var success = _bikeService.DeleteBike(id);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}