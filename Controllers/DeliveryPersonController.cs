using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Services.Interfaces;
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
    public async Task<IActionResult> CreateDeliveryPerson([FromBody] DeliveryPerson deliveryPerson)
    {
        if (deliveryPerson == null || string.IsNullOrWhiteSpace(deliveryPerson.Cnpj))
        {
            return BadRequest("Invalid delivery person data.");
        }

        var createdDeliveryPerson = await _deliveryPersonService.AddDeliveryPersonAsync(deliveryPerson);
        return CreatedAtAction(nameof(GetDeliveryPersonById), new { id = createdDeliveryPerson.Id }, createdDeliveryPerson);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDeliveryPersonById(Guid id)
    {
        var deliveryPerson = await _deliveryPersonService.GetDeliveryPersonByIdAsync(id);
        if (deliveryPerson == null)
        {
            return NotFound();
        }
        return Ok(deliveryPerson);
    }

    [HttpGet("cnpj/{cnpj}")]
    public async Task<IActionResult> GetDeliveryPersonByCnpj(string cnpj)
    {
        var deliveryPerson = await _deliveryPersonService.GetDeliveryPersonByCnpjAsync(cnpj);
        if (deliveryPerson == null)
        {
            return NotFound();
        }
        return Ok(deliveryPerson);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDeliveryPerson(Guid id, [FromBody] DeliveryPerson updatedDeliveryPerson)
    {
        if (updatedDeliveryPerson == null || id != updatedDeliveryPerson.Id)
        {
            return BadRequest("Invalid delivery person data.");
        }

        await _deliveryPersonService.UpdateDeliveryPersonAsync(updatedDeliveryPerson);
        return NoContent();
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
                await _deliveryPersonService.SaveCnhImageAsync(id, imageBytes, file.FileName);
                
                return NotFound("Delivery person not found.");
            }
        }
        catch
        {
            return StatusCode(500, "Error saving the file.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDeliveryPerson(Guid id)
    {
        await _deliveryPersonService.DeleteDeliveryPersonAsync(id);
        return NoContent();
    }
}