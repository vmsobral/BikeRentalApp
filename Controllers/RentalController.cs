using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BikeRentalApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalController : ControllerBase
{
    private readonly IRentalService _rentalService;

    public RentalController(IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRental([FromBody] Rental rental)
    {
        var createdRental = await _rentalService.CreateRentalAsync(rental);
        if (createdRental == null)
        {
            return BadRequest("Invalid rental data.");
        }
        return CreatedAtAction(nameof(GetRentalById), new { id = createdRental.Id }, createdRental);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRentalById(Guid id)
    {
        var rental = await _rentalService.GetRentalByIdAsync(id);
        if (rental == null)
        {
            return NotFound();
        }
        return Ok(rental);
    }

    [HttpPost("{id}/return")]
    public async Task<IActionResult> ReturnBike(Guid id, [FromBody] DateTime returnDate)
    {
        var rental = await _rentalService.ReturnBikeAsync(id, returnDate);
        if (rental == null)
        {
            return BadRequest("Invalid return data.");
        }
        return Ok(rental);
    }
}