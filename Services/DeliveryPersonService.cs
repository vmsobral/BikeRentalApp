using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using BikeRentalApp.Database;
using BikeRentalApp.Domain.Entities;
using BikeRentalApp.Services.Interfaces;

namespace BikeRentalApp.Services;

public class DeliveryPersonService : IDeliveryPersonService
{
    private readonly BikeRentalDbContext _context;

    public DeliveryPersonService(BikeRentalDbContext context)
    {
        _context = context;
    }

    public async Task<DeliveryPerson> AddDeliveryPersonAsync(DeliveryPerson deliveryPerson)
    {
        if (!Enum.IsDefined(typeof(CNHType), deliveryPerson.CnhType))
        {
            throw new ArgumentException($"Invalid CNH type: {deliveryPerson.CnhType}");
        }

        if (await _context.DeliveryPersons.AnyAsync(dp => dp.CnhNumber == deliveryPerson.CnhNumber))
        {
            throw new ArgumentException("A delivery person with the same CNH number already exists.");
        }

        if (await _context.DeliveryPersons.AnyAsync(dp => dp.Cnpj == deliveryPerson.Cnpj))
        {
            throw new ArgumentException("A delivery person with the same CNPJ already exists.");
        }

        deliveryPerson.Id = Guid.NewGuid();
        _context.DeliveryPersons.Add(deliveryPerson);
        await _context.SaveChangesAsync();

        return deliveryPerson;
    }

    public async Task<DeliveryPerson> GetDeliveryPersonByIdAsync(Guid id)
    {
        return await _context.DeliveryPersons.FindAsync(id);
    }

    public async Task<DeliveryPerson> GetDeliveryPersonByCnpjAsync(string cnpj)
    {
        return await _context.DeliveryPersons
                .FirstOrDefaultAsync(dp => dp.Cnpj == cnpj);
    }

    public async Task<DeliveryPerson> GetDeliveryPersonByCnhNumberAsync(string cnhNumber)
    {
        return await _context.DeliveryPersons
                .FirstOrDefaultAsync(dp => dp.CnhNumber == cnhNumber);
    }

    public async Task UpdateDeliveryPersonAsync(DeliveryPerson updatedDeliveryPerson)
    {
        var id = updatedDeliveryPerson.Id;
        var deliveryPerson = await _context.DeliveryPersons.FindAsync(id);
        if (deliveryPerson == null)
        {
            throw new ArgumentException($"Delivery Person with id ${id} not found.");
        }

        deliveryPerson.Name = updatedDeliveryPerson.Name;
        deliveryPerson.Cnpj = updatedDeliveryPerson.Cnpj;
        deliveryPerson.BirthDate = updatedDeliveryPerson.BirthDate;
        deliveryPerson.CnhNumber = updatedDeliveryPerson.CnhNumber;
        deliveryPerson.CnhType = updatedDeliveryPerson.CnhType;

        await _context.SaveChangesAsync();
    }

    public async Task SaveCnhImageAsync(Guid id, byte[] imageBytes, string fileName)
    {
        var deliveryPerson = await _context.DeliveryPersons.FindAsync(id);
        if (deliveryPerson == null)
        {
            throw new ArgumentException($"Delivery Person with id ${id} not found.");
        }
        
        string extension = Path.GetExtension(fileName).ToLower();
        if (extension != ".png" && extension != ".bmp")
        {
            throw new ArgumentException("CNH image file must be on PNG or BMP format.");
        }

        var _imageDirectory = "cnh-images"; //TODO: put on environment var
        string newFileName = $"{id}{extension}";
        string filePath = Path.Combine(_imageDirectory, newFileName);

        try
        {
            Directory.CreateDirectory(_imageDirectory);
            File.WriteAllBytes(filePath, imageBytes);
            deliveryPerson.CnhImagePath = filePath;
        }
        catch
        {
            throw new IOException("Couldn't save CNH image file");
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteDeliveryPersonAsync(Guid id)
    {
        var deliveryPerson = await _context.DeliveryPersons.FindAsync(id);
        if (deliveryPerson == null)
        {
            throw new ArgumentException($"Delivery Person with id ${id} not found.");
        }

        _context.DeliveryPersons.Remove(deliveryPerson);
        await _context.SaveChangesAsync();
    }
}