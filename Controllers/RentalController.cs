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
    public IActionResult CreateRental([FromBody] Rental rental)
    {
        var createdRental = _rentalService.CreateRental(rental);
        if (createdRental == null)
        {
            return BadRequest("Invalid rental data.");
        }
        return CreatedAtAction(nameof(GetRentalById), new { id = createdRental.Id }, createdRental);
    }

    [HttpGet("{id}")]
    public IActionResult GetRentalById(Guid id)
    {
        var rental = _rentalService.GetRentalById(id);
        if (rental == null)
        {
            return NotFound();
        }
        return Ok(rental);
    }

    [HttpPost("{id}/return")]
    public IActionResult ReturnBike(Guid id, [FromBody] DateTime returnDate)
    {
        var rental = _rentalService.ReturnBike(id, returnDate);
        if (rental == null)
        {
            return BadRequest("Invalid return data.");
        }
        return Ok(rental);
    }
}