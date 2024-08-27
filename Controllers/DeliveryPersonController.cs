using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BikeRentalApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeliveryPersonController : ControllerBase
{
    private readonly IDeliveryPersonService _deliveryPersonService;

    public DeliveryPersonController(IDeliveryPersonService deliveryPersonService)
    {
        _deliveryPersonService = deliveryPersonService;
    }

    [HttpPost]
    public IActionResult CreateDeliveryPerson([FromBody] DeliveryPerson deliveryPerson)
    {
        if (deliveryPerson == null || string.IsNullOrWhiteSpace(deliveryPerson.Cnpj))
        {
            return BadRequest("Invalid delivery person data.");
        }

        var createdDeliveryPerson = _deliveryPersonService.AddDeliveryPerson(deliveryPerson);
        return CreatedAtAction(nameof(GetDeliveryPersonById), new { id = createdDeliveryPerson.Id }, createdDeliveryPerson);
    }

    [HttpGet("{id}")]
    public IActionResult GetDeliveryPersonById(Guid id)
    {
        var deliveryPerson = _deliveryPersonService.GetDeliveryPersonById(id);
        if (deliveryPerson == null)
        {
            return NotFound();
        }
        return Ok(deliveryPerson);
    }

    [HttpGet("cnpj/{cnpj}")]
    public IActionResult GetDeliveryPersonByCnpj(string cnpj)
    {
        var deliveryPerson = _deliveryPersonService.GetDeliveryPersonByCnpj(cnpj);
        if (deliveryPerson == null)
        {
            return NotFound();
        }
        return Ok(deliveryPerson);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateDeliveryPerson(Guid id, [FromBody] DeliveryPerson updatedDeliveryPerson)
    {
        if (updatedDeliveryPerson == null || id != updatedDeliveryPerson.Id)
        {
            return BadRequest("Invalid delivery person data.");
        }

        var deliveryPerson = _deliveryPersonService.UpdateDeliveryPerson(updatedDeliveryPerson);
        if (deliveryPerson == null)
        {
            return NotFound();
        }

        return Ok(deliveryPerson);
    }

    [HttpPost("{id}/cnh-image")]
    public async Task<IActionResult> UploadCnhImage(Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (extension != ".png" && extension != ".bmp")
        {
            return BadRequest("Invalid file type. Only PNG and BMP are allowed.");
        }

        try
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var success = _deliveryPersonService.SaveCnhImage(id, imageBytes, file.FileName);
                
                if (success)
                {
                    return NoContent();
                }
                return NotFound("Delivery person not found.");
            }
        }
        catch
        {
            return StatusCode(500, "Error saving the file.");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteDeliveryPerson(Guid id)
    {
        var success = _deliveryPersonService.DeleteDeliveryPerson(id);
        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}